using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Objects
{
    public class CollapsePlatform : MonoBehaviour
    {
        public Rigidbody2D cpRb;
        public BoxCollider2D cpBoxCollider;
        public float shakeRange = 0.0625f;
        public float shakeDelay = 0.05f;
        public float fallDelay = 1;
        public float destroyDelay = 2;
        public string playerTag = "Player";
        [ReadOnly]
        public bool isFalling = false;
        [ReadOnly]
        public bool isCollapsing = false;
        private WaitForSeconds fallWaitSec;
        private WaitForSeconds shakeWaitSec;
        private Vector2 cpStartPos;

        void Start()
        {
            fallWaitSec = new WaitForSeconds(fallDelay);
            shakeWaitSec = new WaitForSeconds(shakeDelay);

            cpRb.gravityScale *= 3;

            cpStartPos  = cpRb.position;
        }

        void Update()
        {
            if(isCollapsing && !isFalling)
            {
                StartCoroutine(Shake());
            }

            if(isFalling)
            {
                StopCoroutine(Shake());
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.gameObject.CompareTag("Player"))
            {
                isCollapsing = true;

                StartCoroutine(Fall());
            }
        }

        private IEnumerator Fall()
        {
            yield return fallWaitSec;

            isFalling = true;

            cpRb.bodyType = RigidbodyType2D.Dynamic;

            cpBoxCollider.enabled = false;

            Destroy(gameObject, destroyDelay);
        }

        private IEnumerator Shake()
        {
            yield return shakeWaitSec;

            cpRb.position = new Vector2(cpStartPos.x + shakeRange, cpRb.position.y);

            yield return shakeWaitSec;

            cpRb.position = new Vector2(cpStartPos.x - shakeRange, cpRb.position.y);
        }
    }
}