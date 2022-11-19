using UnityEngine;

namespace Player
{
    public class GroundChecker : MonoBehaviour
    {
		public Vector2 groundCheckSize;
		public LayerMask groundLayer;
        [ReadOnly]
        public bool isGround;

        public void IsGround()
        {
            if(Physics2D.OverlapBox(transform.position, groundCheckSize, 0, groundLayer))
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }
        }

        void FixedUpdate()
        {
            IsGround();
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(transform.position, groundCheckSize);
        }
    }
}