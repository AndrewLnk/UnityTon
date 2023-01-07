using System;

namespace Assets.TonIntegration.Scripts.Core.TonAPI.Requests.JsonRequestsParse
{
    [Serializable]
    public class Transaction
    {
        public string hash;
        public float amount;
        public string comment;
        public bool success;
    }
}
