using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private PlayerInput input;
    public bool jumpInput;
    public float moveInput;

    void Awake() 
    {
        TryGetComponent(out input);
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
}
