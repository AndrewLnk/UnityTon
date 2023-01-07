using Assets.TonIntegration.Scripts.Core.Scriptable;
using UnityEditor;
using UnityEngine;

namespace Assets.TonIntegration.Scripts.Core.Main
{
    [CustomEditor(typeof(TonConnection))]
    public class TonConnectionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var player = target as TonConnection;
            DrawInspector(player);
        }
        
        private void DrawInspector(TonConnection item)
        {
            var so = serializedObject;

            EditorGUILayout.PrefixLabel("User Name");
            item.UserName = EditorGUILayout.TextField(item.UserName);
            EditorGUILayout.PrefixLabel("User Password");
            item.UserPassword = EditorGUILayout.TextField(item.UserPassword);
            EditorGUILayout.Space(5);
            if (TonProjectSettings.Instance != null)
            {
                GUI.backgroundColor = Color.white;
                var hasWarning = false;
                if (string.IsNullOrEmpty(TonProjectSettings.Instance.TonProjectKeeper.MainWallet))
                {
                    EditorGUILayout.Space(5);
                    EditorGUILayout.HelpBox("Please set a wallet address in the Ton Project Settings.", MessageType.Warning);
                    EditorGUILayout.Space(5);
                    hasWarning = true;
                }
                if (string.IsNullOrEmpty(TonProjectSettings.Instance.TonProjectKeeper.ClientToken))
                {
                    EditorGUILayout.Space(5);
                    EditorGUILayout.HelpBox("Please set a client token in the Ton Project Settings.", MessageType.Warning);
                    EditorGUILayout.Space(5);
                    hasWarning = true;
                }
                if (string.IsNullOrEmpty(TonProjectSettings.Instance.TonProjectKeeper.AppPassword))
                {
                    EditorGUILayout.Space(5);
                    EditorGUILayout.HelpBox("Please set an app password in the Ton Project Settings.", MessageType.Warning);
                    EditorGUILayout.Space(5);
                    hasWarning = true;
                }

                if (!hasWarning)
                {
                    GUI.backgroundColor = item.LoadedProducts != null && item.LoadedProducts.Length > 0 ? Color.green : Color.yellow;
                    if (item.LoadedProducts != null)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(item.LoadedProducts)));
                    }
                    EditorGUILayout.Space(5);
                    GUI.backgroundColor = Color.green;
                    if (GUILayout.Button("Load Products From Wallet", GUILayout.Height(30)))
                    {
                        item.StartLoadProductsFromWallet();
                        GUI.FocusControl(null);
                    }
                    EditorGUILayout.Space(5);
                    if (item.FinishedFetchFromWallet != null)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(item.FinishedFetchFromWallet)));
                    }
                    serializedObject.ApplyModifiedProperties();
                }
            }
            else
            {
                GUI.backgroundColor = Color.white;
                EditorGUILayout.HelpBox("Warning: You need to create \"Ton Project Settings\" " +
                                        "\n (AssetMenu / TON / ProjectSettings).", MessageType.Error);
            }
            
            EditorGUILayout.Space(3);
            EditorGUILayout.PrefixLabel("Additional tools");
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Select Ton Settings", GUILayout.Height(20)))
            {
                Selection.activeObject = TonProjectSettings.Instance;
                GUI.FocusControl(null);
            }
            if (GUILayout.Button("Get Client Token", GUILayout.Height(20)))
            {
                Application.OpenURL("https://t.me/tonapi_bot");
                GUI.FocusControl(null);
            }
            
            GUI.backgroundColor = Color.white;
        }
    }
}