using System;

namespace Assets.TonIntegration.Scripts.Core.TonAPI.Requests.JsonRequestsParse
{
    [Serializable]
    public class WalletTransactionsTrace
    {
        public WalletTransactionTrace[] transactions;
    }
    
    [Serializable]
    public class WalletTransactionTrace
    {
        public string hash;
        public WalletTransactionInMessage in_msg;
        public WalletTransactionInMessage[] out_msgs;
    }
    
    [Serializable]
    public class WalletTransactionInMessage
    {
        public double value;
    }
}
