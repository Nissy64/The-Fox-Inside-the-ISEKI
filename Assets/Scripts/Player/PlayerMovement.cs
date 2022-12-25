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
        [ReadOnly]
        public bool isJumping;
        #endregion

        #region CornerCorrection
        [Header("Corner Correction")]
        public float topRaycastLength;
        public Vector3 edgeRaycastOffset;
        public Vector3 innerRaycastOffset;
        [ReadOnly]
        public bool canCornerCorrect;
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
            CheckCollisions();

            Movement();

            CoyoteTimer();
        }

        void Update()
        {
            if(!groundChecker.isGround) animator.SetFloat("Speed", 0);
            else animator.SetFloat("Speed", Mathf.Abs(inputManager.moveInput));
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawLine(transform.position + edgeRaycastOffset, transform.position + edgeRaycastOffset + Vector3.up * topRaycastLength);
            Gizmos.DrawLine(transform.position - edgeRaycastOffset, transform.position - edgeRaycastOffset + Vector3.up * topRaycastLength);
            Gizmos.DrawLine(transform.position + innerRaycastOffset, transform.position + innerRaycastOffset + Vector3.up * topRaycastLength);
            Gizmos.DrawLine(transform.position - innerRaycastOffset, transform.position - innerRaycastOffset + Vector3.up * topRaycastLength);

            Gizmos.DrawLine(transform.position - innerRaycastOffset + Vector3.up * topRaycastLength,
                            transform.position - innerRaycastOffset + Vector3.up * topRaycastLength + Vector3.left * topRaycastLength);
            Gizmos.DrawLine(transform.position + innerRaycastOffset + Vector3.up * topRaycastLength,
                            transform.position + innerRaycastOffset + Vector3.up * topRaycastLength + Vector3.right * topRaycastLength);
        }

        private void Movement()
        {
            if(gameManager.isGameOver) return;
            if(isDashing) return;

            FlipSprite();

            Run();

            if(rb.velocity.y > 0.1f && canCornerCorrect)
            {
                CornerCorrect(rb.velocity.y);
            }

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

                if(jumpBufferCounter > 0 && !isJumping)
                {
                    StartCoroutine(Jump());
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

        public IEnumerator Jump()
        {
            WaitForSeconds waitSec = new WaitForSeconds(0.5f);
            coyoteTimeCounter = 0;
            jumpBufferCounter = 0;
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

            animator.SetBool("IsJumping", true);
            isJumping = true;

            yield return waitSec;

            isJumping = false;
        }

        private void CornerCorrect(float YVelocity)
        {
            RaycastHit2D hit = Physics2D.Raycast(playerTransform.position - innerRaycastOffset + Vector3.up * topRaycastLength, Vector3.left, topRaycastLength, groundChecker.groundLayer);
            if(hit.collider != null)
            {
                float newPos = Vector3.Distance(new Vector3(hit.point.x, playerTransform.position.y, 0) + Vector3.up * topRaycastLength, 
                    playerTransform.position - edgeRaycastOffset + Vector3.up * topRaycastLength);
                playerTransform.position = new Vector3(playerTransform.position.x + newPos, playerTransform.position.y, playerTransform.position.z);
                rb.velocity = new Vector2(rb.velocity.x, YVelocity);
                return;
            }

            hit = Physics2D.Raycast(playerTransform.position - innerRaycastOffset + Vector3.up * topRaycastLength, Vector3.right, topRaycastLength, groundChecker.groundLayer);
            if(hit.collider != null)
            {
                float newPos = Vector3.Distance(new Vector3(hit.point.x, playerTransform.position.y, 0) + Vector3.up * topRaycastLength, 
                    playerTransform.position + edgeRaycastOffset + Vector3.up * topRaycastLength);
                playerTransform.position = new Vector3(playerTransform.position.x - newPos, playerTransform.position.y, playerTransform.position.z);
                rb.velocity = new Vector2(rb.velocity.x, YVelocity);
            }
        }

        private void CheckCollisions()
        {
            bool rightEdge = Physics2D.Raycast(playerTransform.position + edgeRaycastOffset, Vector2.up, topRaycastLength, groundChecker.groundLayer);
            bool rightInner = Physics2D.Raycast(playerTransform.position + innerRaycastOffset, Vector2.up, topRaycastLength, groundChecker.groundLayer);
            bool leftEdge = Physics2D.Raycast(playerTransform.position - edgeRaycastOffset, Vector2.up, topRaycastLength, groundChecker.groundLayer);
            bool leftInner = Physics2D.Raycast(playerTransform.position - innerRaycastOffset, Vector2.up, topRaycastLength, groundChecker.groundLayer);

            print("rightEdge: " + rightEdge);
            print("rightInner: " + rightInner);
            print("leftEdge: " + leftEdge);
            print("leftInner: " + leftInner);

            canCornerCorrect = !(rightInner && leftInner) && ((rightEdge && !rightInner) || (leftEdge && !leftInner));
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
            WaitForSeconds waitSec = new WaitForSeconds(dashingTime);

            yield return waitSec;
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
            WaitForSeconds waitSec = new WaitForSeconds(0.25f);
            rb.simulated = false;
            animator.SetBool("IsGameOver", true);

            yield return waitSec;

            gameManager.isGameOver = true;
        }
    }
}