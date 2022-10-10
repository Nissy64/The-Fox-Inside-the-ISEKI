using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Inputs
    private PlayerInput input;
    private bool jumpInput;
    private float moveInput;
    [Header("Run")]
    public float palyerSpeed = 5;
    public float acceleration;
    public float decceleration;
    public float velPower;
    private Rigidbody2D rb;
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

        JumpFalloff();

        FlipSprite();
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

    private void JumpFalloff()
    {
        if(!(coyoteTimeCounter > 0f) && rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * playerFalloffMultiplier * Time.deltaTime;
        }
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

        if(moveInput > 0)
        {
            transform.eulerAngles = facingRight;
        }
        if(moveInput < 0)
        {
            transform.eulerAngles = facingLeft;
        }
    }

    
}