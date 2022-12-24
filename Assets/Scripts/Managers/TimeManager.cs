using UnityEngine;

namespace Managers
{
    public class TimeManager : MonoBehaviour
    {
        [ReadOnly]
        public static float fixedDeltaTimer = 0;
        [ReadOnly]
        public static float deltaTimer = 0;

        void FixedUpdate()
        {
            FixedDeltaTimer();
        }

        void Update()
        {
            DeltaTimer();
        }

        private void FixedDeltaTimer()
        {
            fixedDeltaTimer += Time.deltaTime;
        }

        private void DeltaTimer()
        {
            deltaTimer += Time.deltaTime;
        }

        public float CooldownTimer(float cooldownCounter)
        {
            if(cooldownCounter > 0)
            {
                cooldownCounter -= Time.deltaTime;
            }

            if(cooldownCounter < 0)
            {
                cooldownCounter = 0;
            }

            return cooldownCounter;
        }

        public float ResetCooldownTimer(float cooldown)
        {
            return cooldown;
        }
    }
}