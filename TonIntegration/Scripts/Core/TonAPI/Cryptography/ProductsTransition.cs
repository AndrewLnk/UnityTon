using System;
using System.Collections.Generic;
using System.Linq;
using Assets.TonIntegration.Scripts.Core.Scriptable;
using Assets.TonIntegration.Scripts.Core.TonAPI.Requests.JsonRequestsParse;
using UnityEngine;

namespace Assets.TonIntegration.Scripts.Core.TonAPI.Cryptography
{
    public static class ProductsTransition
    {
        public static EventHandler<string> FetchErrors;
        
        public static IapTransaction[] FetchProducts(Transactions transactions, string userName, string userPassword)
        {
            if (TonProjectSettings.Instance.TonProductsKeeper.Products == null || 
                TonProjectSettings.Instance.TonProductsKeeper.Products.Length == 0)
                return Array.Empty<IapTransaction>();

            if (transactions?.transactions == null)
                return Array.Empty<IapTransaction>();

            var key = $"{TonProjectSettings.Instance.TonProjectKeeper.AppPassword}{userPassword}";
            var settingsProducts = TonProjectSettings.Instance.TonProductsKeeper.Products;
            var products = new List<IapTransaction>();
            foreach (var transaction in transactions.transactions)
            {
                var readableIap = string.Empty;
                try
                {
                    readableIap = Md5.Md5Decryption(transaction.comment, key);
                }
                catch
                {
                    // ignore
                }

                if (!readableIap.StartsWith(userName))
                {
                    const string message = "Incorrect name, password, or transfer was not found!";
                    FetchErrors?.Invoke(null, message);
                    continue;
                }

                readableIap = readableIap.Remove(0, userName.Length);

                var iap = settingsProducts.FirstOrDefault(e => e.IAPKey.Equals(readableIap));

                if (iap == null)
                {
                    const string message = "Purchase does not exist in the app!";
                    FetchErrors?.Invoke(null, message);
                    continue;
                }

                if (Mathf.Abs(iap.TonCoinPrice - transaction.amount) > 0.05f)
                {
                    const string message = "The purchase does not match the suggested price!";
                    FetchErrors?.Invoke(null, message);
                    products.Add(new IapTransaction(){Key = message});
                    continue;
                }
                
                products.Add(new IapTransaction(){Key = readableIap, Amount = iap.TonCoinPrice});
            }

            return products.ToArray();
        }

        public static string CreateTransferComment(string aipKey, string userName, string userPassword)
        {
            if (TonProjectSettings.Instance == null)
                return string.Empty;
            
            var key = $"{TonProjectSettings.Instance.TonProjectKeeper.AppPassword}{userPassword}";
            var comment = $"{userName}{aipKey}";
            return Md5.Md5Encryption(comment, key);
        }
    }
}
