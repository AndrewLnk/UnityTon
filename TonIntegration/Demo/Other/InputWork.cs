using UnityEngine;
using UnityEngine.UI;

namespace Assets.TonIntegration.Demo.Other
{
    public class InputWork : MonoBehaviour
    {
        public InputField InputField;
        public DemoEntry DemoEntry;

        public void UpdateUserName() => DemoEntry.TonConnection.UserName = InputField.text;
        public void UpdatePassword() => DemoEntry.TonConnection.UserPassword = InputField.text;
    }
}
