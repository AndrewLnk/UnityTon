using System;
using Assets.TonIntegration.Scripts.Core.TonAPI.Cryptography;
using Assets.TonIntegration.Scripts.Core.TonAPI.Requests.JsonRequestsParse;
using UnityEngine;

namespace Assets.TonIntegration.Scripts.Core.TonAPI.Requests
{
    public class TonDataObject : MonoBehaviour
    {
        public Transactions Transactions;

        private string token;
        private string wallet;
        private string user;
        private string password;
        private Action<IapTransaction[]> loadedProducts;
        
        public static TonDataObject Create(string mainWallet, string clientToken, string username, string userPassword)
        {
            var go = new GameObject("TON Data"){hideFlags = HideFlags.HideInHierarchy};
            var request = go.AddComponent<TonDataObject>();
            request.token = clientToken;
            request.wallet = mainWallet;
            request.user = username;
            request.password = userPassword;
            return request;
        }

        public void UpdateData(Action<IapTransaction[]> fetchedProducts)
        {
            loadedProducts = fetchedProducts;
            var request = LoadWalletTransactions.CreateRequest();
            request.ResponseReceiver += (r,e) => Transactions = e;
            request.ResponseReceiver += FetchTransactions;
            request.StartRequest(wallet, token);
        }

        private void FetchTransactions(object sender, Transactions transactions)
        {
            var request = LoadTransactionsData.CreateRequest();
            request.ResponseReceiver += (o, b) =>
            {
                var products = ProductsTransition.FetchProducts(transactions, user, password);
                loadedProducts.Invoke(products);
            };
            request.StartRequest(transactions, token);
        }
    }
}
