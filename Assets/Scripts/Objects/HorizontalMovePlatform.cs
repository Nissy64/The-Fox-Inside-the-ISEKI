using UnityEngine;
using Managers;
using Player;

namespace Objects
{
    public class HorizontalMovePlatform : MonoBehaviour
    {
        public Transform mpTransform;
        public Rigidbody2D mpRb;
        public Rigidbody2D playerRb;
        public PlayerMovement playerMovement;
        public float playerOnMpSpeedMultiply = 2;
        private float defaultPlayerSpeed;
        private float playerOnMpSpeed;
        public string playerTag;
        public float startMpPosition = 0;
        public float endMpPosition;
        public float mpSpeed = 0.5f;
        public FolderManager.GizmosFiles startMpPositionIcon;
        public FolderManager.GizmosFiles endMpPositionIcon;
        [ReadOnly]
        public bool isPlayerOnMp = false;
        private Vector2 prevPosition;
        private Vector2 mpVelocity;

        void Awake()
        {
            prevPosition = mpTransform.position;
            defaultPlayerSpeed = playerMovement.palyerSpeed;
            playerOnMpSpeed = playerMovement.palyerSpeed * playerOnMpSpeedMultiply * mpSpeed;
        }

        void FixedUpdate()
        {
            MpVelocity();

            MovingForce();

            YoyoMove.HorizontalMove(startMpPosition, endMpPosition, mpSpeed, mpRb, Managers.TimeManager.fixedDeltaTimer);
        }

        void Update()
        {
            
        }

        void OnDrawGizmosSelected()
        {
            Vector3 vec3StartPos = new Vector3(startMpPosition, mpTransform.position.y, 0);
            Vector3 vec3EndPos = new Vector3(endMpPosition, mpTransform.position.y, 0);

            Gizmos.DrawIcon(vec3StartPos, FolderManager.GetGizmosFiles(startMpPositionIcon), true);
            Gizmos.DrawIcon(vec3EndPos, FolderManager.GetGizmosFiles(endMpPositionIcon), true);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.CompareTag(playerTag))
            {
                isPlayerOnMp = true;
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if(collider.CompareTag(playerTag))
            {
                isPlayerOnMp = false;
            }
        }

        private void MpVelocity()
        {
            if(Mathf.Approximately(Time.deltaTime, 0)) return;

            var position = new Vector2(mpTransform.position.x, mpTransform.position.y);

            mpVelocity = (position - prevPosition) / Time.deltaTime;

            prevPosition = position;
        }

        private void MovingForce()
        {
            if(isPlayerOnMp)
            {
                playerRb.velocity += new Vector2(mpVelocity.x - playerRb.velocity.x, 0);
                playerMovement.palyerSpeed = playerOnMpSpeed;
            }
            else
            {
                playerMovement.palyerSpeed = defaultPlayerSpeed;
            }
        }
    }
}