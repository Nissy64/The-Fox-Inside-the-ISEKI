using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Inputs")]
	public PlayerInputs playerInputs;
	[Header("Run")]
	public Transform playerTransform;
	public Rigidbody2D rb;
	public float palyerSpeed = 5;
	public float acceleration;
	public float decceleration;
	public float velPower;
	[Header("Dash")]
	public TrailRenderer trail;
	public float dashingPower = 24;
	public float dashingTime = 0.2f;
	public float dashCooldown = 1f;
	private float dashCooldownCounter = 1f;
	private bool canDash = true;
	private bool isDashing;
	[Header("Jump")]
	public float playerFalloffMultiplier = 5;
	public float playerJumpForce = 1;
	public float coyoteTime = 0.2f;
	private float coyoteTimeCounter;
	[Header("Check Ground")]
	public Transform groundCheckPosition;
	public Vector2 groundCheckSize;
	public LayerMask groundLayer;
	private bool isGround;
	[Header("Animation")]
	public Animator animator;
	[Header("Interact")]
	public Vector2 interactPosition;
	public float interactRadius;

	void Awake()
	{
		canDash = true;
		dashCooldownCounter = 0;
	}

	void FixedUpdate()
	{
		isGround = GroundChecker.IsGround(groundCheckPosition, groundCheckSize, groundLayer);
		dashCooldownCounter = CoolTimer(dashCooldownCounter);
		if(!isGround) animator.SetFloat("Speed", 0);
		else animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

		CoyoteTimer();
		Movement();
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);
	}

	private void Movement()
	{
		if(isDashing) return;

		FlipSprite();

		Run();

		if(canDash && playerInputs.dashInput && dashCooldownCounter == 0)
		{
			StartDash();
			StartCoroutine(StopDashing());
		}

		if(coyoteTimeCounter > 0f)
		{
			canDash = true;
			animator.SetBool("IsJumping", false);
			animator.SetBool("IsDashing", false);

			if(playerInputs.jumpInput)
			{
				Jump();
			}
		}

		if(rb.velocity.y > 5)
		{
			animator.SetBool("IsJumping", true);
		}

		if(coyoteTimeCounter > 0f) return;
		if(rb.velocity.y > 0)
		{
			JumpFalloff();
		}
	}

	private void Run()
	{
		float targetSpeed = playerInputs.moveInput * palyerSpeed;
		float speedDif = targetSpeed - rb.velocity.x;
		float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
		float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

		rb.AddForce(movement * Vector2.right);
	}

	private void Jump()
	{
		rb.velocity = Vector2.up * playerJumpForce;

		coyoteTimeCounter = 0;
	}

	private void JumpFalloff()
	{
		rb.velocity += Vector2.up * Physics2D.gravity.y * playerFalloffMultiplier * Time.deltaTime;
	}

	private void CoyoteTimer()
	{
		if(isGround)
		{
			coyoteTimeCounter = coyoteTime;
		}
		else
		{
			coyoteTimeCounter -= Time.deltaTime;
		}
	}

	private void FlipSprite()
	{
		Vector3 facingRight = Vector3.zero;
		Vector3 facingLeft = Vector3.up * 180;

		if(playerInputs.moveInput > 0)
		{
			playerTransform.eulerAngles = facingRight;
		}
		if(playerInputs.moveInput < 0)
		{
			playerTransform.eulerAngles = facingLeft;
		}
	}

	private void StartDash()
	{
		isDashing = true;
		canDash = false;
		trail.emitting = true;
		Vector2 dashingDirection = new Vector2(playerInputs.moveInput, playerInputs.verticalInput);

		if(dashingDirection == Vector2.zero)
		{
			dashingDirection = new Vector2(GetDirection(), 0);
		}

		if(isDashing)
		{
			rb.velocity = dashingDirection.normalized * dashingPower;
			animator.SetBool("IsJumping", false);
			animator.SetBool("IsDashing", true);
			return;
		}
	}

	private IEnumerator StopDashing()
	{
		yield return new WaitForSeconds(dashingTime);
		isDashing = false;
		trail.emitting = false;
		dashCooldownCounter = ResetCooldownTimer(dashCooldown);
	}

	private float CoolTimer(float cooldownCounter)
	{
		if(cooldownCounter > 0)
		{
			cooldownCounter -= Time.deltaTime;
		}

		if(cooldownCounter < 0)
		{
			cooldownCounter = 0;
		}

		return cooldownCounter;
	}

	private float ResetCooldownTimer(float cooldown)
	{
		return cooldown;
	}

	private int GetDirection()
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
}