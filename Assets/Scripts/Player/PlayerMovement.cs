using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace Player 
{
	public class PlayerMovement : MonoBehaviour
	{
		[Header("Managers")]
		public GameManager gameManager;
		public InputManager inputManager;
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
		[SerializeField, ReadOnly]
		private float coyoteTimeCounter;
		[Header("Check Ground")]
		public GroundChecker groundChecker;
		[Header("Energy")]
		public Image[] energyUIs;
		public Sprite emptyEnergy;
		public Sprite glowingEnergy;
		public int maxEnergy = 3;
		public int dashConsumeEnergy = 1;
		public float energyChargeCooldown = 2;
		public float noEnergyChergeCooldown = 5;
		[SerializeField, ReadOnly]
		private float energyChargeCooldownCounter = 1;
		[SerializeField, ReadOnly]
		private int currentEnergey = 0;
		private bool isNoEnergy = false;
		[Header("Animation")]
		public Animator animator;

		void Awake()
		{
			canDash = true;
			currentEnergey = maxEnergy;
			energyChargeCooldownCounter = ResetCooldownTimer(energyChargeCooldown);
			animator.SetBool("IsGameOver", false);

			foreach(Image eUI in energyUIs)
			{
				eUI.sprite = glowingEnergy;
			}
		}

		void FixedUpdate()
		{
			Movement();

			CoyoteTimer();
			if(coyoteTimeCounter > 0) canDash = true;
		}

		void Update()
		{
			if(currentEnergey == 0 && !isNoEnergy)
			{
				energyChargeCooldownCounter = ResetCooldownTimer(noEnergyChergeCooldown);
				isNoEnergy = true;
			}

			if(!(currentEnergey == maxEnergy)) energyChargeCooldownCounter = CoolTimer(energyChargeCooldownCounter);

			ChargeEnergy();
			EnergyUI();

			if(!groundChecker.isGround) animator.SetFloat("Speed", 0);
			else animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
		}

		private void Movement()
		{
			if(gameManager.isGameOver) return;
			if(isDashing) return;

			FlipSprite();

			Run();

			if(coyoteTimeCounter > 0)
			{
				animator.SetBool("IsJumping", false);
				animator.SetBool("IsDashing", false);

				if(inputManager.jumpInput)
				{
					coyoteTimeCounter = 0;
					animator.SetBool("IsJumping", true);
					Jump();
				}
			}

			if(canDash && inputManager.dashInput && currentEnergey > 0)
			{
				StartDash();
				StartCoroutine(StopDashing());
			}

			if(rb.velocity.y > 5)
			{
				animator.SetBool("IsJumping", true);
			}

			if(coyoteTimeCounter > 0) return;
			if(rb.velocity.y > 0)
			{
				JumpFalloff();
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
			rb.velocity = Vector2.up * playerJumpForce;

			coyoteTimeCounter = 0;
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

			if(dashingDirection == Vector2.zero || dashingDirection.y == -1)
			{
				dashingDirection = new Vector2(GetDirection(), 0);
			}

			if(isDashing)
			{
				rb.velocity = dashingDirection.normalized * dashingPower;
				animator.SetBool("IsJumping", false);
				animator.SetBool("IsDashing", true);
				currentEnergey -= dashConsumeEnergy;
				canDash = false;
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

		public IEnumerator PlayerGameOver()
		{
			rb.simulated = false;
			animator.SetBool("IsGameOver", true);

			yield return new WaitForSeconds(0.25f);

			gameManager.isGameOver = true;
		}

		private void ChargeEnergy()
		{
			if(energyChargeCooldownCounter == 0)
			{
				isNoEnergy = false;

				if(currentEnergey < maxEnergy)
				{
					currentEnergey += 1;
					energyChargeCooldownCounter = ResetCooldownTimer(energyChargeCooldown);
				}
			}
		}

		private void EnergyUI()
		{
			foreach(Image eUI in energyUIs)
			{
				if(int.Parse(eUI.name.Substring(7)) == currentEnergey)
				{
					eUI.sprite = emptyEnergy;
				}
				else if(int.Parse(eUI.name.Substring(7)) < currentEnergey)
				{
					eUI.sprite = glowingEnergy;
				}
			}
		}
	}
}