namespace Assets.TonIntegration.Scripts.Core.TonAPI.Requests
{
    public static class ServerStaticInfo
    {
        public static string GetWalletDataUrl(string wallet) => 
            $"https://tonapi.io/v1/blockchain/getTransactions?account={wallet}";
        public static string GetTransactionDataUrl(string hash) => 
            $"https://tonapi.io/v1/trace/getAnnotatedTrace?hash={hash}";

        public static string GetTonKeeperQrAddress(string wallet, string amount, string comment) =>
            $"https://app.tonkeeper.com/transfer/{wallet}?amount={amount}&text={comment}";
    }
}
