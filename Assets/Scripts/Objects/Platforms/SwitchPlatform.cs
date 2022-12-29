using System.Collections;
using UnityEngine;

namespace Objects
{
    public class SwitchPlatform : MonoBehaviour
    {
        public BoxCollider2D spBoxCollider;
        public SpriteRenderer spSpriteRenderer;
        public Vector2 spSize = Vector2.one * 4;
        public float switchSec;
        public Animator spAnimator;
        [ReadOnly]
        public bool isActive;
        private WaitForSeconds switchWaitSec;

        void Awake()
        {
            switchWaitSec = new WaitForSeconds(switchSec);
        }

        void Update()
        {
            StartCoroutine(Switch());
        }

        private IEnumerator Switch()
        {
            if(isActive)
            {
                yield return switchWaitSec;
                isActive = false;

                spBoxCollider.enabled = isActive;
                spAnimator.SetBool("IsActive", isActive);
            }
            else
            {
                yield return switchWaitSec;
                isActive = true;

                spBoxCollider.enabled = isActive;
                spAnimator.SetBool("IsActive", isActive);
            }
        }
    }
}