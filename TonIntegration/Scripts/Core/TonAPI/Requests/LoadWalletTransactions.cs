using System;
using System.Collections;
using System.Collections.Generic;
using Assets.TonIntegration.Scripts.Core.TonAPI.Requests.JsonRequestsParse;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.TonIntegration.Scripts.Core.TonAPI.Requests
{
    public class LoadWalletTransactions : MonoBehaviour
    {
        public EventHandler<Transactions> ResponseReceiver;
        
        // Create and setup game object as process
        public static LoadWalletTransactions CreateRequest()
        {
            var go = new GameObject("Request Load Wallet Data")
            {
                hideFlags = HideFlags.HideInHierarchy
            };
            var request = go.AddComponent<LoadWalletTransactions>();
            return request;
        }
        
        // Start request
        public void StartRequest(string itemId, string token)
        {
            StartCoroutine(RequestUploadNewProcess(itemId, token));
        }

        // Main request process
        private IEnumerator RequestUploadNewProcess(string wallet, string token)
        {
            var request = UnityWebRequest.Get(ServerStaticInfo.GetWalletDataUrl(wallet));
            request.SetRequestHeader("Authorization", $"Bearer {token}");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                // If request failed
                ResponseReceiver?.Invoke(this, null);
            }
            else
            {
                ResponseReceiver?.Invoke(this, ParseResponse(request.downloadHandler.text));
            }
            
            // Destroy this process object
            if (Application.isPlaying || !Application.isEditor) Destroy(gameObject);
            else DestroyImmediate(gameObject);
        }

        private Transactions ParseResponse(string response)
        {
            WalletTransactionsTrace result = null;
            try
            {
                result = JsonUtility.FromJson<WalletTransactionsTrace>(response);
            }
            catch
            {
                // ignored
            }

            if (result?.transactions == null)
                return new Transactions() {transactions = Array.Empty<Transaction>()};

            var transactionsList = new List<Transaction>();
            foreach (var transaction in result.transactions)
            {
                var item = new Transaction();
                if (transaction.out_msgs != null && transaction.out_msgs.Length > 0)
                    item.amount = -(float)(transaction.out_msgs[0].value / 1000000000);
                else if (transaction.in_msg != null)
                    item.amount = (float)(transaction.in_msg.value / 1000000000);
                else
                    continue;

                item.hash = transaction.hash;
                transactionsList.Add(item);
            }

            var package = new Transactions() {transactions = transactionsList.ToArray()};
            return package;
        }
    }
}
