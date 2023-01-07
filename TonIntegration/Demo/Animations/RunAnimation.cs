using UnityEngine;

namespace Assets.TonIntegration.Demo.Animations
{
    public class RunAnimation : MonoBehaviour
    {
        public Animation Animation;

        public void Play(string animName) => Animation?.Play(animName);
    }
}
