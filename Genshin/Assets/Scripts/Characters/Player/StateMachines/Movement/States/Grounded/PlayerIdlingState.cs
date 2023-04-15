using UnityEngine;

namespace RPGStateMachineSystem
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        public PlayerIdlingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            stateMachine.ReusableData.BackwardsCameraRecenteringData = groundedData.IdleData.BackwardsCameraRecenteringData;

            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StationaryForce;

            ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
        }

        public override void Update()
        {
            base.Update();

            if (stateMachine.ReusableData.AttackInput)
            {
                OnAttack();
                return;
            }


            if (stateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                OnMove();
                return;
            }


        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally())
            {
                return;
            }

            ResetVelocity();
        }
    }
}