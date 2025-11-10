using UnityEngine;

namespace Game.Project.PlayerMgmt
{
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator animator;
        private int idleXHash;
        private int idleYHash;

        private int moveXHash;
        private int moveYHash;

        private int isIdleHash;

        public void SetIdleAnimation(float idleX, float idleY)
        {
            animator.SetFloat(idleXHash, idleX);
            animator.SetFloat(idleYHash, idleY);
            animator.SetBool(isIdleHash, true);
        }

        public void SetMovementAnimation(float moveX, float moveY)
        {
            animator.SetBool(isIdleHash, false);
            animator.SetFloat(moveXHash, moveX);
            animator.SetFloat(moveYHash, moveY);
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();

            isIdleHash = Animator.StringToHash(Constants.IS_IDLE_ANIM_PARAM);

            idleXHash = Animator.StringToHash(Constants.IDLE_X_ANIM_PARAM);
            idleYHash = Animator.StringToHash(Constants.IDLE_Y_ANIM_PARAM);

            moveXHash = Animator.StringToHash(Constants.MOVE_X_ANIM_PARAM);
            moveYHash = Animator.StringToHash(Constants.MOVE_Y_ANIM_PARAM);
        }
    }
}
