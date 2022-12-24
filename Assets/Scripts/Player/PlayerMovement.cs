using System;
using System.Collections;
using UnityEngine;
using Objects;
using Managers;

namespace Player 
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Managers
        [Header("Managers")]
        public GameManager gameManager;
        public InputManager inputManager;
        #endregion

        #region Run
        [Header("Run")]
        public Transform playerTransform;
        public Rigidbody2D rb;
        public float palyerSpeed = 5;
        public float acceleration;
        public float decceleration;
        public float velPower;
        #endregion

        #region Dash
        [Header("Dash")]
        public TrailRenderer trail;
        public float dashingPower = 24;
        public float dashingTime = 0.2f;
        [ReadOnly]
        public bool canDash = true;
        [ReadOnly]
        public bool isDashing;
        #endregion

        #region Jump
        [Header("Jump")]
        public float playerFalloffMultiplier = 5;
        public float playerJumpForce = 1;
        public float palyerAirJumpForceMultiplier = 2;
        public float coyoteTime = 0.2f;
        [ReadOnly]
        public float coyoteTimeCounter;
        public float jumpBufferTime = 0.2f;
        [ReadOnly]
        public float jumpBufferCounter = 0;
        #endregion

        #region CheckGround
        [Header("Check Ground")]
        public GroundChecker groundChecker;
        #endregion

        #region Animation
        [Header("Animation")]
        public Animator animator;
        #endregion

        void Awake()
        {
            canDash = true;
            animator.SetBool("IsGameOver", false);
        }

        void FixedUpdate()
        {
            Movement();

            CoyoteTimer();
        }

        void Update()
        {
            if(!groundChecker.isGround) animator.SetFloat("Speed", 0);
            else animator.SetFloat("Speed", Mathf.Abs(inputManager.moveInput));
        }

        private void Movement()
        {
            if(gameManager.isGameOver) return;
            if(isDashing) return;

            FlipSprite();

            Run();

            if(inputManager.jumpInput)
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            if(coyoteTimeCounter > 0)
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsDashing", false);
                canDash = true;

                if(jumpBufferCounter > 0)
                {
                    Jump();
                    animator.SetBool("IsJumping", true);
                }
            }

            if(canDash && inputManager.dashInput)
            {
                StartDash();
                StartCoroutine(StopDashing());
            }

            if(rb.velocity.y > 8f && coyoteTimeCounter > 0)
            {
                animator.SetBool("IsJumping", true);
            }

            if(coyoteTimeCounter <= 0)
            {
                if(rb.velocity.y > 0)
                {
                    JumpFalloff();
                }
            }
        }

        private void Run()
        {
            float targetSpeed = inputManager.moveInput * palyerSpeed;
            float speedDif = targetSpeed - rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

            rb.AddForce(movement * Vector2.right);
        }

        public void Jump()
        {
            float multiplier = 1;

            if(groundChecker.isGround)
            {
                multiplier = 1;
            }
            else
            {
                multiplier *= palyerAirJumpForceMultiplier;
            }

            rb.velocity = Vector2.up * playerJumpForce * multiplier;

            coyoteTimeCounter = 0;
            jumpBufferCounter = 0;
        }

        private void JumpFalloff()
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * playerFalloffMultiplier * Time.deltaTime;
        }

        private void CoyoteTimer()
        {
            if(groundChecker.isGround)
            {
                coyoteTimeCounter = coyoteTime;
            }
            else
            {
                coyoteTimeCounter -= Time.deltaTime;
            }

            if(isDashing)
            {
                coyoteTimeCounter = 0;
            }
        }

        private void FlipSprite()
        {
            Vector3 facingRight = Vector3.zero;
            Vector3 facingLeft = Vector3.up * 180;

            if(inputManager.moveInput > 0)
            {
                playerTransform.eulerAngles = facingRight;
            }
            if(inputManager.moveInput < 0)
            {
                playerTransform.eulerAngles = facingLeft;
            }
        }

        private void StartDash()
        {
            isDashing = true;
            trail.emitting = true;
            Vector2 dashingDirection = new Vector2(inputManager.moveInput, inputManager.verticalInput);

            if(inputManager.moveInput == 0 && inputManager.verticalInput != 0)
            {
                dashingDirection = new Vector2(inputManager.moveInput, inputManager.verticalInput);
            }
            else if(inputManager.moveInput != 0 && inputManager.verticalInput == 0)
            {
                dashingDirection = new Vector2(inputManager.moveInput, 0.5f);
            }

            if(dashingDirection == Vector2.zero || dashingDirection.y == -1)
            {
                dashingDirection = new Vector2(GetDirection01(), 0.5f);
            }

            if(isDashing)
            {
                rb.velocity = dashingDirection.normalized * dashingPower;

                animator.SetBool("IsJumping", false);
                animator.SetBool("IsDashing", true);
                coyoteTimeCounter = 0;
                canDash = false;
                return;
            }
        }

        private IEnumerator StopDashing()
        {
            yield return new WaitForSeconds(dashingTime);
            isDashing = false;
            trail.emitting = false;
        }

        private int GetDirection01()
        {
            int direction = 1;
            Vector3 facingRight = Vector3.zero;
            Vector3 facingLeft = Vector3.up * 180;

            if(playerTransform.eulerAngles == facingRight)
            {
                direction = 1;
            }

            if(playerTransform.eulerAngles == facingLeft)
            {
                direction = -1;
            }

            return direction;
        }

        public IEnumerator PlayerGameOver()
        {
            rb.simulated = false;
            animator.SetBool("IsGameOver", true);

            yield return new WaitForSeconds(0.25f);

            gameManager.isGameOver = true;
        }
    }
}