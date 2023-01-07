using System;
using System.Globalization;
using System.Linq;
using Assets.TonIntegration.Scripts.Core.Keepers;
using Assets.TonIntegration.Scripts.Core.Scriptable;
using Assets.TonIntegration.Scripts.Core.TonAPI.Cryptography;
using Assets.TonIntegration.Scripts.Core.TonAPI.Requests;
using QrCodeGenerator;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.TonIntegration.Scripts
{
    public class QrCodeView : MonoBehaviour
    {
        [Header("Settings")]
        public string IAPKeyFromTonSettings;
        [Header("Other")]
        public Image ImageTarget;

        public void OnEnable() => FindProductLocalKeyAndGenerate();

        public void FindProductLocalKeyAndGenerate()
        {
            if (TonProjectSettings.Instance == null)
                return;

            var product = TonProjectSettings.Instance.TonProductsKeeper.Products
                .FirstOrDefault(e => e.IAPKey.Equals(IAPKeyFromTonSettings));

            if (product == null)
            {
                Debug.LogWarning("The name of the product does not match any existing one in Ton Settings!");
                return;
            }

            Generate(product);
        }

        public void Generate(TonProductsKeeper.IapItem iapItem)
        {
            if (TonProjectSettings.Instance == null)
                return;

            var connect = FindObjectOfType<TonConnection>();

            if (connect == null)
            {
                Debug.LogWarning("The TonConnection class is not found (or disabled) in the scene!");
                return;
            }

            var username = connect.UserName;
            var password = connect.UserPassword;
            double price = Math.Round(iapItem.TonCoinPrice * 1000000000);
            var comment = ProductsTransition.CreateTransferComment(iapItem.IAPKey, username, password);

            if (comment.Length > 100)
            {
                Debug.LogWarning("Too long a transfer comment!");
                return;
            }
            
            if (string.IsNullOrEmpty(username) || username.Length < 1)
            {
                Debug.LogWarning("The username is too short!");
                return;
            }
            
            if (string.IsNullOrEmpty(password) || password.Length < 1)
            {
                Debug.LogWarning("The password is too short!");
                return;
            }
            
            if (price <= 0.001f)
            {
                Debug.LogWarning("The transfer amount is too small!");
                return;
            }
            
            var address = ServerStaticInfo.GetTonKeeperQrAddress(TonProjectSettings.Instance.TonProjectKeeper.MainWallet,
                price.ToString(CultureInfo.InvariantCulture), comment);
            
            StartGenerate(address);
        }
        
        // Start is called before the first frame update
        private void StartGenerate(string address)
        {
            var im = new QrRawData(address);
            var result = im.CreateImage(out var QrCodeVersion);

            var side = (int) Mathf.Sqrt(result.Length);
            var texture2D = new Texture2D(side, side, TextureFormat.RGBA32, false);
            for (var x = 0; x < side; x++)
            {
                for (var y = 0; y < side; y++)
                {
                    texture2D.SetPixel(x, y, result[x, y] ? Color.black : Color.white);
                }
            }
            texture2D.filterMode = FilterMode.Point;
            texture2D.Apply();
            
            var sprite = Sprite.Create(texture2D, new Rect(0, 0, side, side), new Vector2(0, 0));
            ImageTarget.sprite = sprite;
            ImageTarget.color = Color.white;
            ImageTarget.preserveAspect = true;
        }
    }
}
