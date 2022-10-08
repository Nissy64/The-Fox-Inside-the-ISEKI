using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInput input;
    private float moveInput;
    private bool jumpInput;
    [Header("Run")]
    public float palyerSpeed = 5;
    public float acceleration;
    public float decceleration;
    public float velPower;
    [Header("Jump")]
    public float playerGravityScale = 5;
    public float playerJumpForce = 1;
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float fallMultiplier;
    [Header("Check Ground")]
    private bool isGround;
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize;
    public LayerMask groundLayer;

    void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out input);
    }

    void FixedUpdate()
    {
        isGround = GroundChecker.IsGround(groundCheckPosition, groundCheckSize, groundLayer);

        CoyoteTimer();
        Movement();

        if(isGround)
        {
            rb.gravityScale = 3;
        }
        else
        {
            rb.gravityScale = playerGravityScale;
            rb.AddForce(Vector2.up * fallMultiplier);
        }

        if(rb.velocity.y > 0)
        {
            fallMultiplier = playerGravityScale * 1.25f;
        }
        if(rb.velocity.y <= 0)
        {
            fallMultiplier = 0;
        }
    }

    void OnEnable()
    {
        input.actions["Move"].performed += OnMove;
        input.actions["Move"].canceled += OnMoveStop;

        input.actions["Jump"].started += OnJump;
        input.actions["Jump"].canceled += OnJump;
    }

    void OnDisable() 
    {
        input.actions["Move"].performed -= OnMove;
        input.actions["Move"].canceled -= OnMoveStop;

        input.actions["Jump"].started -= OnJump;
        input.actions["Jump"].canceled -= OnJump;
    }

    private void OnMoveStop(InputAction.CallbackContext obj)
    {
        moveInput = 0;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        moveInput = obj.ReadValue<float>();
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        jumpInput = obj.ReadValue<float>() > 0.01f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);
    }

    private void Movement()
    {
        Run();

        if(jumpInput && coyoteTimeCounter > 0f)
        {
            Jump();
        }

        print(rb.velocity);
    }

    private void Run()
    {
        float targetSpeed = moveInput * palyerSpeed;
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
}