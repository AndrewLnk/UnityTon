using Assets.TonIntegration.Scripts;
using UnityEngine;

namespace Assets.TonIntegration.Demo
{
    public class DemoEntry : MonoBehaviour
    {
        public QrCodeView QrCodeView;
        public TonConnection TonConnection;

        private void Start()
        {
            QrCodeView.FindProductLocalKeyAndGenerate();
        }
    }
}
