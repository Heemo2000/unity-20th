using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Project.Movement;
using Game.Utils.Core;

namespace Game.Project.PlayerMgmt
{
    public class PlayerHandler : MonoBehaviour
    {
        [SerializeField] private PlayerAnimator animator;
        private SixteenWayMovement movement;
        private GameInput gameInput;
        private Vector2 moveInput = Vector2.zero;


        private void Awake()
        {
            movement = GetComponent<SixteenWayMovement>();
        }
        // Start is called before the first frame update
        void Start()
        {
            movement.OnIdle += animator.SetIdleAnimation;
            movement.OnMovement += animator.SetMovementAnimation;
            
        }

        private void OnDestroy()
        {
            movement.OnIdle -= animator.SetIdleAnimation;
            movement.OnMovement -= animator.SetMovementAnimation;
        }

        // Update is called once per frame
        void Update()
        {
            if (gameInput == null && ServiceLocator.Global.TryGet<GlobalReferencesManager>(out var manager))
            {
                gameInput = manager.GameInput;
            }

            if(gameInput != null)
            {
                moveInput = gameInput.GetMovementInputNormalized();
            }
        }

        private void FixedUpdate()
        {
            movement.HandleMovement(moveInput);
        }
    }
}
