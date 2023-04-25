using UnityEngine;

namespace RPGStateMachineSystem
{
    public class PlayerFallingState : PlayerAirborneState
    {
        public PlayerFallingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.FallParameterHash);

            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            stateMachine.ReusableData.playerPositionOnFall = stateMachine.Player.transform.position;

            ResetVerticalVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.FallParameterHash);
            stateMachine.ReusableData.playerPositionOnFall = Vector3.down;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            LimitVerticalVelocity();
        }

        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            if (playerVerticalVelocity.y >= -airborneData.FallData.FallSpeedLimit)
            {
                return;
            }

            Vector3 limitedVelocityForce = new Vector3(0f, -airborneData.FallData.FallSpeedLimit - playerVerticalVelocity.y, 0f);

            stateMachine.Player.Rigidbody.AddForce(limitedVelocityForce, ForceMode.VelocityChange);
        }

        protected override void ResetSprintState()
        {
        }

        protected override void OnContactWithGround(Collider collider)
        {
            float fallStartY = stateMachine.ReusableData.playerPositionOnFall.y;
            float fallDistance = fallStartY - stateMachine.Player.transform.position.y;

            if (fallDistance < airborneData.FallData.MinimumDistanceToBeConsideredHardFall)
            {
                stateMachine.ChangeState(stateMachine.LightLandingState);

                return;
            }

            if (stateMachine.ReusableData.ShouldWalk && !stateMachine.ReusableData.ShouldSprint || stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.HardLandingState);

                return;
            }

            stateMachine.ChangeState(stateMachine.RollingState);

        }
    }
}