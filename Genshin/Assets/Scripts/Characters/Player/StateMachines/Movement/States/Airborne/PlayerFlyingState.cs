using UnityEngine;

namespace RPGStateMachineSystem
{
    public class PlayerFlyingState : PlayerAirborneState
    {

        public PlayerFlyingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.FlyParameterHash);

            //stateMachine.ReusableData.MovementSpeedModifier = 0f;

            ResetVerticalVelocity();

            // TODO 회전 처리

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.FlyParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            LimitVerticalVelocity();
        }

        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            if (playerVerticalVelocity.y >= -airborneData.FlyData.FallSpeedLimit)
            {
                return;
            }

            Vector3 limitedVelocityForce = new Vector3(0f, -airborneData.FlyData.FallSpeedLimit - playerVerticalVelocity.y, 0f);

            stateMachine.Player.Rigidbody.AddForce(limitedVelocityForce, ForceMode.VelocityChange);
        }


        protected override void OnContactWithGround(Collider collider)
        {
            stateMachine.ChangeState(stateMachine.RollingState);
        }
    }
}