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
    public float jumpForce = 1;
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
        Movement();
        isGround = GroundChecker.IsGround(groundCheckPosition, groundCheckSize, groundLayer);
        print(jumpInput);
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

        if(jumpInput && isGround)
        {
            Jump();
        }
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
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}