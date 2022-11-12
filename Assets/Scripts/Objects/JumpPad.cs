using UnityEngine;

namespace Objects 
{
    public class JumpPad : MonoBehaviour
    {
        public Animator playerAnimator;
        public Animator myAnimator;
        public Rigidbody2D playerRb;
        public float bounce = 5f;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                playerAnimator.SetBool("IsJumping", true);
                myAnimator.SetBool("IsRiding", true);
                Invoke(nameof(Jump), 0.05f);
                Invoke(nameof(StopAnimation), 0.6f);
            }
        }

        private void StopAnimation()
        {
            myAnimator.SetBool("IsRiding", false);
        }

        private void Jump()
        {
            playerRb.velocity = Vector2.up * bounce;
        }
    }
}