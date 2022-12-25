using System.Collections;
using UnityEngine;

namespace Objects 
{
    public class JumpPad : MonoBehaviour
    {
        public Animator jumpPadAnimator;
        public float bounce = 5f;
        private Rigidbody2D playerRb;

        void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Animator>().SetBool("IsJumping", true);
                playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                jumpPadAnimator.SetBool("IsRiding", true);
                Jump();
                StartCoroutine(StopAnimation());
            }
        }

        private IEnumerator StopAnimation()
        {
            yield return new WaitForSeconds(0.6f);
            jumpPadAnimator.SetBool("IsRiding", false);
        }

        private void Jump()
        {
            playerRb.AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
        }
    }
}