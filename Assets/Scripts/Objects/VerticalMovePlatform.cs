using UnityEngine;
using DG.Tweening;
using System.Collections;
using Managers;

namespace Objects
{
    public class VerticalMovePlatform : MonoBehaviour
    {
        public Transform mpTransform;
        public Transform playerTransform;
        public Rigidbody2D mpRb;
        public Rigidbody2D playerRb;
        public Animator mpAnimator;
        public string playerTag;
        [ReadOnly] 
        public float startMpPosition = 0;
        public float endMpPosition;
        public FolderManager.GizmosFiles startMpPositionIcon;
        public FolderManager.GizmosFiles endMpPositionIcon;
        public float mpDuration = 2;
        public string mpAnimState = "State";
        public int mpWaitSecond = 1;
        [Range(1, 100)]
        public float downdingForceMultiply = 10;
        [ReadOnly]
        public Vector2 mpVelocity;
        private Vector2 prevPosition;
        private bool isPlayerOnMp = false;
        private MonoBehaviour monoB;

        void Awake()
        {
            startMpPosition = mpTransform.position.y;
            prevPosition = mpTransform.position;
            monoB = gameObject.GetComponent<MonoBehaviour>();
        }

        void Start()
        {
            // YoyoMove.Move
            //     (new Vector2(mpTransform.position.x, startMpPosition), new Vector2(mpTransform.position.x, endMpPosition), 
            //     mpDuration, mpDuration, 
            //     mpWaitSecond, mpWaitSecond, 
            //     mpRb, 
            //     mpAnimator, mpAnimState,
            //     monoB
            //     );
        }

        void FixedUpdate()
        {
            DowndingForce();
        }

        void Update()
        {
            MpVelocity();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawIcon(mpTransform.position, FolderManager.GetGizmosFiles(startMpPositionIcon), true);
            Gizmos.DrawIcon(new Vector3(mpTransform.position.x, endMpPosition, 0), FolderManager.GetGizmosFiles(endMpPositionIcon), true);
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if(collider.CompareTag(playerTag))
            {
                isPlayerOnMp = true;
                playerRb.position = new Vector2(playerTransform.position.x, mpTransform.position.y + 1.4375f);
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if(collider.CompareTag(playerTag))
            {
                isPlayerOnMp = true;
            }
        }

        private void MpVelocity()
        {
            if(Mathf.Approximately(Time.deltaTime, 0)) return;

            var position = new Vector2(mpTransform.position.x, mpTransform.position.y);

            mpVelocity = (position - prevPosition) / Time.deltaTime;

            prevPosition = position;
        }

        private void DowndingForce()
        {
            if(!isPlayerOnMp) return;
            if(!(mpVelocity.y > 0)) return;
            playerRb.AddForce(Vector2.down * Mathf.Abs(mpVelocity.y) * downdingForceMultiply * Time.deltaTime);
        }
    }
}