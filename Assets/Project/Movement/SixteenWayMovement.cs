using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Project.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class SixteenWayMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10.0f;
        [SerializeField] private float moveDirectionLerpingSpeed = 1.0f;

        public Action<float, float> OnIdle;
        public Action<float, float> OnMovement;
        

        private Rigidbody2D moveRB;
        private CapsuleCollider2D moveCollider;
        private Vector2 previousMoveDirection = Vector2.zero;
        private Vector2 currentMoveDirection = Vector2.zero;

        public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

        public void HandleMovement(Vector2 moveDirection)
        {
            currentMoveDirection = Vector2.Lerp(currentMoveDirection, moveDirection, moveDirectionLerpingSpeed * Time.deltaTime);
            moveRB.velocity = currentMoveDirection * moveSpeed;

            //Only track non zero move input for previousMoveDirection variable.

            if(moveDirection.x != 0.0f || moveDirection.y != 0.0f)
            {
                previousMoveDirection = moveDirection;
            }

            //If the moveDirection variable is 0, then invoke invoke OnIdle event, otherwise OnMovement method.
            if(moveDirection.x == 0.0f && moveDirection.y == 0.0f)
            {
                Debug.Log("Idle");
                OnIdle?.Invoke(previousMoveDirection.x, previousMoveDirection.y);
            }
            else
            {
                Debug.Log("Moving");
                OnMovement?.Invoke(currentMoveDirection.x, currentMoveDirection.y);
            }
        }

        private void Awake()
        {
            moveRB = GetComponent<Rigidbody2D>();
            moveRB.isKinematic = false;
            moveRB.gravityScale = 0.0f;
            moveRB.drag = Constants.MOVEMENT_DRAG;
            moveRB.angularDrag = Constants.MOVEMENT_ANGULAR_DRAG;

            moveCollider = GetComponent<CapsuleCollider2D>();
            moveCollider.isTrigger = false;
        }


    }
}
