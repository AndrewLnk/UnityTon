using System.Linq;
using Assets.TonIntegration.Scripts;
using Assets.TonIntegration.Scripts.Core.Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.TonIntegration.Demo.Other
{
    public class PriceButtonSetter : MonoBehaviour
    {
        public Text Text;
        public DemoEntry DemoEntry;
        
        // Start is called before the first frame update
        private void Start()
        {
            if (TonProjectSettings.Instance?.TonProductsKeeper?.Products == null)
                return;

            var product = TonProjectSettings.Instance.TonProductsKeeper.Products
                .FirstOrDefault(e => e.IAPKey.Equals(DemoEntry.QrCodeView.IAPKeyFromTonSettings));
            
            if (product == null)
                return;

            Text.text += $"{product.TonCoinPrice} TON";
        }
    }
}
