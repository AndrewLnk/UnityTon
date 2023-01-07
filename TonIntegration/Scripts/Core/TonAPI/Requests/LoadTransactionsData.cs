using System;
using System.Collections;
using Assets.TonIntegration.Scripts.Core.TonAPI.Requests.JsonRequestsParse;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.TonIntegration.Scripts.Core.TonAPI.Requests
{
    public class LoadTransactionsData : MonoBehaviour
    {
        public EventHandler<bool> ResponseReceiver;
        private int requests;

        public static LoadTransactionsData CreateRequest()
        {
            var go = new GameObject("Request Load Transactions By Hash")
            {
                hideFlags = HideFlags.HideInHierarchy
            };
            var request = go.AddComponent<LoadTransactionsData>();
            return request;
        }
        
        public void StartRequest(Transactions transactions, string token)
        {
            foreach (var transaction in transactions.transactions)
            {
                StartCoroutine(RequestUploadNewProcess(transaction, token));
                requests++;
            }
        }
        
        private IEnumerator RequestUploadNewProcess(Transaction transaction, string token)
        {
            var request = UnityWebRequest.Get(ServerStaticInfo.GetTransactionDataUrl(transaction.hash));
            request.SetRequestHeader("Authorization", $"Bearer {token}");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                SetupResponse(transaction, request.downloadHandler.text);
            }

            requests--;
            if (requests <= 0)
            {
                ResponseReceiver?.Invoke(this, true);
                // Destroy this process object
                if (Application.isPlaying || !Application.isEditor) Destroy(gameObject);
                else DestroyImmediate(gameObject);
            }
        }

        private void SetupResponse(Transaction transaction, string response)
        {
            TransactionTrace fromJson = null;
            try
            {
                fromJson = JsonUtility.FromJson<TransactionTrace>(response);
            }
            catch
            {
                // ignored
            }
            
            if (fromJson?.children == null || fromJson.children.Length == 0)
                return;
            
            if (fromJson.children[0]?.annotations.Length == 0)
                return;
            
            if (fromJson?.annotations == null || fromJson?.annotations.Length == 0)
                return;
            
            if (fromJson.children[0].annotations[0].data == null)
                return;
            
            transaction.success = fromJson.children[0].success;
            transaction.comment = fromJson.children[0].annotations[0].data.text;
        }
    }
}
