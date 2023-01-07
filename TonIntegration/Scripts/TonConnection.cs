using Assets.TonIntegration.Scripts.Core.Scriptable;
using Assets.TonIntegration.Scripts.Core.TonAPI.Requests;
using Assets.TonIntegration.Scripts.Core.TonAPI.Requests.JsonRequestsParse;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.TonIntegration.Scripts
{
    public class TonConnection : MonoBehaviour
    {
        [SerializeField] public string UserName;
        [SerializeField] public string UserPassword;
        [SerializeField] public IapTransaction[] LoadedProducts;
        public UnityEvent FinishedFetchFromWallet;

        private TonDataObject tonDataObject;
        public void StartLoadProductsFromWallet()
        {
            if (TonProjectSettings.Instance == null)
                return;

            var oldDataObject = FindObjectsOfType<TonDataObject>();
            foreach (var item in oldDataObject)
            {
                if (Application.isPlaying || !Application.isEditor) Destroy(item.gameObject);
                else DestroyImmediate(item.gameObject);
            }

            var wallet = TonProjectSettings.Instance.TonProjectKeeper.MainWallet;
            var token = TonProjectSettings.Instance.TonProjectKeeper.ClientToken;
            tonDataObject = TonDataObject.Create(wallet, token, UserName, UserPassword);
            tonDataObject.UpdateData(e =>
            {
                LoadedProducts = (IapTransaction[]) e.Clone();
                FinishedFetchFromWallet?.Invoke();
            });
        }
    }
}
