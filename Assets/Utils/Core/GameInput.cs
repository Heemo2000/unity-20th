using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Input;

namespace Game.Utils.Core
{
    public class GameInput : MonoBehaviour
    {
        
        [SerializeField]private Vector2 lookSpeed = Vector2.one;

        public Action OnPausePressed;
        public Action OnInteractPressed;
        
        private PlayerInputActions playerInputActions;
        private Vector2 currentLookDelta = Vector2.zero;

        public Camera LookCamera { get => Camera.main; }

        public Vector2 GetSmoothLookDelta()
        {
            Vector2 targetLookDelta = GetLookDelta();
            currentLookDelta.x = Mathf.Lerp(currentLookDelta.x, targetLookDelta.x, lookSpeed.x * Time.deltaTime);
            currentLookDelta.y = Mathf.Lerp(currentLookDelta.y, targetLookDelta.y, lookSpeed.y * Time.deltaTime);
            return currentLookDelta;
        }
        public Vector2 GetLookDelta()
        {
            Vector2 centreOfScreen = new Vector2(Screen.width/2.0f, Screen.height/2.0f);
            Vector2 lookDelta = playerInputActions.Player.Look.ReadValue<Vector2>() - centreOfScreen;
            return lookDelta;
        }
        public Vector2 GetMovementInputNormalized()
        {
            return GetMovementInput().normalized;
        }
        public Vector2 GetMovementInput()
        {
            return playerInputActions.Player.Movement.ReadValue<Vector2>();
        }

        private void OnPausePress(InputAction.CallbackContext context)
        {
            OnPausePressed?.Invoke();   
        }

        private void OnInteraction(InputAction.CallbackContext context)
        {
            OnInteractPressed?.Invoke();
        }
        
        private void Awake() 
        {
            playerInputActions = new PlayerInputActions();
        }

        // Start is called before the first frame update
        void Start()
        {
            playerInputActions.Enable();
            
            playerInputActions.Player.Interact.Enable();
            playerInputActions.Player.Interact.started += OnInteraction;
            playerInputActions.Player.PauseToggle.started += OnPausePress;
        }

        private void OnDestroy() 
        {
            playerInputActions.Disable();
            
            playerInputActions.Player.Interact.Disable();
            playerInputActions.Player.Interact.started -= OnInteraction;
            playerInputActions.Player.PauseToggle.started -= OnPausePress;
        }
    }
}
