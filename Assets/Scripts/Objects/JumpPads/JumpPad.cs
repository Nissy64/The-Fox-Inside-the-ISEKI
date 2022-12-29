using System.Collections;
using UnityEngine;

namespace Objects 
{
    public class JumpPad : MonoBehaviour
    {
        public Animator jumpPadAnimator;
        public float bounce = 5f;
        private Rigidbody2D playerRb;
        private WaitForSeconds stopAnimWaitSec;

        void Awake()
        {
            stopAnimWaitSec = new WaitForSeconds(0.6f);
        }

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
            yield return stopAnimWaitSec;
            jumpPadAnimator.SetBool("IsRiding", false);
        }

        private void Jump()
        {
            playerRb.velocity = Vector2.zero;
            playerRb.AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
        }
    }
}