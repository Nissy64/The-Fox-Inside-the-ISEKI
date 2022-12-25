using UnityEngine;
using Managers;

namespace Objects
{
    public class VerticalMovePlatform : MonoBehaviour
    {
        public Transform mpTransform;
        public Rigidbody2D mpRb;
        public Rigidbody2D playerRb;
        public float playerOnMpForceMultiply = 10;
        public string playerTag;
        public float startMpPosition = 0;
        public float endMpPosition;
        public FolderManager.GizmosFiles startMpPositionIcon;
        public FolderManager.GizmosFiles endMpPositionIcon;
        public float mpSpeed = 2;
        [ReadOnly]
        public Vector2 mpVelocity;
        private Vector2 prevPosition;
        private bool isPlayerOnMp = false;

        void Awake()
        {
            prevPosition = mpTransform.position;
        }

        void Start()
        {
            
        }

        void FixedUpdate()
        {
            DowndingForce();

            YoyoMove.VerticalMove(startMpPosition, endMpPosition, mpSpeed, mpRb, Managers.TimeManager.fixedDeltaTimer);
        }

        void Update()
        {
            MpVelocity();
        }

        void OnDrawGizmosSelected()
        {
            Vector3 vec3StartPos = new Vector3(mpTransform.position.x, startMpPosition, 0);
            Vector3 vec3EndPos = new Vector3(mpTransform.position.x, endMpPosition, 0);

            Gizmos.DrawIcon(vec3StartPos, FolderManager.GetGizmosFiles(startMpPositionIcon), true);
            Gizmos.DrawIcon(vec3EndPos, FolderManager.GetGizmosFiles(endMpPositionIcon), true);
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if(collider.CompareTag(playerTag))
            {
                isPlayerOnMp = true;
                playerRb.position = new Vector2(playerRb.position.x, mpTransform.position.y + 1.4375f);
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
            playerRb.AddForce(Vector2.down * Mathf.Abs(mpVelocity.y) * playerOnMpForceMultiply * Time.deltaTime);
        }
    }
}