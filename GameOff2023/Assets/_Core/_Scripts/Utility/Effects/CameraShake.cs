using HeathenEngineering.Events;
using UnityEngine;

namespace _Core._Scripts.Utility.Effects
{
    public class CameraShake : MonoBehaviour
    {
        public Animator CameraAnimator;
        private static readonly int Shake = Animator.StringToHash("Shake");

        public GameEvent ShakeEvent;
        
        public void CamShake()
        {
            CameraAnimator.SetTrigger(Shake);
        }
    }
}
