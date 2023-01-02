using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Objects
{
    public class AntiGravityPlatform : MonoBehaviour
    {
        public SpriteRenderer apSpriteRenderer;
        public Rigidbody2D apRb;
        public BoxCollider2D apBoxCollider;
        public float shakeRange = 0.0625f;
        public float shakeDelay = 0.05f;
        public float fallDelay = 1;
        public float fallGravityScale = -2;
        public float destroyDelay = 2;
        public string playerTag = "Player";
        [ReadOnly]
        public bool isFalling = false;
        [ReadOnly]
        public bool isCollapsing = false;
        private WaitForSeconds fallWaitSec;
        private WaitForSeconds shakeWaitSec;
        private WaitForSeconds colliderDisableWaitSec;
        private Vector2 cpStartPos;

        void Start()
        {
            fallWaitSec = new WaitForSeconds(fallDelay);
            shakeWaitSec = new WaitForSeconds(shakeDelay);
            colliderDisableWaitSec = new WaitForSeconds(destroyDelay / 2);

            apRb.gravityScale = fallGravityScale;

            cpStartPos  = apRb.position;
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

            apRb.bodyType = RigidbodyType2D.Dynamic;

            apSpriteRenderer.DOFade(0, destroyDelay);

            Destroy(gameObject, destroyDelay);

            yield return colliderDisableWaitSec;

            apBoxCollider.enabled = false;
        }

        private IEnumerator Shake()
        {
            yield return shakeWaitSec;

            apRb.position = new Vector2(cpStartPos.x + shakeRange, apRb.position.y);

            yield return shakeWaitSec;

            apRb.position = new Vector2(cpStartPos.x - shakeRange, apRb.position.y);
        }
    }
}