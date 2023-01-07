using System.Linq;
using Assets.TonIntegration.Scripts.Core.Keepers;
using UnityEditor;
using UnityEngine;

namespace Assets.TonIntegration.Scripts.Core.Scriptable
{
    [CreateAssetMenu(menuName = "TON/ProjectSettings", fileName = "TonSettings")]
    public class TonProjectSettings : ScriptableObject
    {
        private static TonProjectSettings instance;
        
        public static TonProjectSettings Instance
        {
            get
            {
                if (instance == null)
                    instance = Resources.FindObjectsOfTypeAll<TonProjectSettings>().FirstOrDefault();

                return instance;
            }
        }
        
        [Header("Project Settings")]
        public TonProjectKeeper TonProjectKeeper;
        [Header("Products Settings")]
        public TonProductsKeeper TonProductsKeeper;
    }

    internal static class TonMenuItems
    {
        [MenuItem("TON/Settings")]
        private static void OpenSettings()
        {
            if (TonProjectSettings.Instance != null)
            {
                Selection.activeObject = TonProjectSettings.Instance;
            }
            else
            {
                Debug.LogError("Please create \"Ton Project Settings\": AssetMenu/TON/ProjectSettings");
            }
        }
    }
}
