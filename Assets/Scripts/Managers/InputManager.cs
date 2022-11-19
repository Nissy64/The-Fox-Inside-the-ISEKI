using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        public PlayerInput input;
        [ReadOnly]
        public float moveInput;
        [ReadOnly]
        public bool jumpInput;
        [ReadOnly]
        public bool dashInput;
        [ReadOnly]
        public float verticalInput;

        void OnEnable()
        {
            input.actions["Move"].performed += OnMove;
            input.actions["Move"].canceled += OnMoveStop;

            input.actions["Jump"].started += OnJump;
            input.actions["Jump"].canceled += OnJump;

            input.actions["Dash"].started += OnDash;
            input.actions["Dash"].canceled += OnDash;

            input.actions["Vertical"].performed += OnVertical;
            input.actions["Vertical"].canceled += OnVerticalStop;
        }

        void OnDisable() 
        {
            input.actions["Move"].performed -= OnMove;
            input.actions["Move"].canceled -= OnMoveStop;

            input.actions["Jump"].started -= OnJump;
            input.actions["Jump"].canceled -= OnJump;

            input.actions["Dash"].started -= OnDash;
            input.actions["Dash"].canceled -= OnDash;

            input.actions["Vertical"].performed -= OnVertical;
            input.actions["Vertical"].canceled -= OnVerticalStop;
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

        private void OnDash(InputAction.CallbackContext obj)
        {
            dashInput = obj.ReadValue<float>() > 0.01f;
        }

        private void OnVerticalStop(InputAction.CallbackContext obj)
        {
            verticalInput = 0;
        }

        private void OnVertical(InputAction.CallbackContext obj)
        {
            verticalInput = obj.ReadValue<float>();
        }
    }
}