using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;


namespace GenshinImpactMovementSystem
{
    public class PlayerAttackingState : PlayerGroundedState
    {
        private bool shouldRotating;

        public PlayerAttackingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
            DisableCameraRecentering();

            stateMachine.ReusableData.MovementSpeedModifier = 0f;
            shouldRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;
            Rotate();

            ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (shouldRotating)
            {
                RotateTowardsTargetRotation();
            }
        }

        private void Rotate()
        {
            if (shouldRotating)
            {
                UpdateTargetRotation(GetMovementInputDirection());
            }
        }

        protected override void OnMovementPerformed(InputAction.CallbackContext context)
        {

        }

        protected float GetNormalizedTime(Animator animator, string tag)
        {
            AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
            AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

            if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
            {
                return nextInfo.normalizedTime;
            }
            else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
            {
                return currentInfo.normalizedTime;
            }
            else
            {
                return 0f;
            }
        }

    }
}