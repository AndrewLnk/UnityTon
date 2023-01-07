using System;
using UnityEngine;

namespace Assets.TonIntegration.Scripts.Core.Keepers
{
    [Serializable]
    public class TonProjectKeeper
    {
        [Header("Wallet Address")]
        public string MainWallet;
        [Header("Client API Token")]
        public string ClientToken;
        [Header("Encrypting App Password")]
        public string AppPassword;
    }
}
