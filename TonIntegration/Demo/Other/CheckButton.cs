using System.Collections;
using System.Linq;
using Assets.TonIntegration.Scripts.Core.Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.TonIntegration.Demo.Other
{
    public class CheckButton : MonoBehaviour
    {
        public Animation Animation;
        public Animation AnimationInput;
        public string AnimShow;
        public string AnimHide;
        [Space]
        public Animation AnimationPurchased;
        public GameObject ObjectWhenPurchased;
        [Space] 
        public DemoEntry DemoEntry;

        private bool inWork;
        
        public void Check()
        {
            if (inWork)
                return;

            StartCoroutine(CheckWithDelay());
        }

        public void FinishCheck()
        {
            if (TonProjectSettings.Instance?.TonProductsKeeper?.Products == null)
            {
                Animation.Play(AnimShow);
                AnimationInput.Play(AnimShow);
                return;
            }

            if (DemoEntry.TonConnection.LoadedProducts.Length == 0)
            {
                Animation.Play(AnimShow);
                AnimationInput.Play(AnimShow);
                return;
            }

            var product =  DemoEntry.TonConnection.LoadedProducts
                .FirstOrDefault(e=>e.Key.Equals(DemoEntry.QrCodeView.IAPKeyFromTonSettings));
            
            if (product == null)
            {
                Animation.Play(AnimShow);
                AnimationInput.Play(AnimShow);
                return;
            }
            
            ObjectWhenPurchased.SetActive(true);
            AnimationPurchased.Play(AnimShow);
        }

        private IEnumerator CheckWithDelay()
        {
            inWork = true;
            Animation.Play(AnimHide);
            AnimationInput.Play(AnimHide);
            yield return new WaitForSeconds(0.3f);
            DemoEntry.TonConnection.StartLoadProductsFromWallet();
            inWork = false;
        }
    }
}
