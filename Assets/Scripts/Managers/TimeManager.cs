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
    }
}