using System;
using UnityEngine;

namespace Assets.TonIntegration.Scripts.Core.Keepers
{
    [Serializable]
    public class TonProductsKeeper
    {
        [Space] 
        public IapItem[] Products;
        
        [Serializable]
        public class IapItem
        {
            public string IAPKey;
            public float TonCoinPrice;
        }
    }
}
