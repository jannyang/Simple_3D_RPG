using UnityEngine;
using UnityEngine.InputSystem;

namespace RPGStateMachineSystem
{
    public class PlayerAirborneState : PlayerBaseState
    {
        public PlayerAirborneState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.AirborneParameterHash);

            ResetSprintState();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.AirborneParameterHash);
        }

        protected virtual void ResetSprintState()
        {
            stateMachine.ReusableData.ShouldSprint = false;
        }

        protected override void OnContactWithGround(Collider collider)
        {
            stateMachine.ChangeState(stateMachine.LightLandingState);
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            stateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            stateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;
        }


        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            float fallStartY = stateMachine.ReusableData.playerPositionOnFall.y;
            float playerY = stateMachine.Player.Rigidbody.position.y;
            if (fallStartY > playerY - 1.8)     // TODO 플레이어 기본 높이로 설정
            { 
                stateMachine.ChangeState(stateMachine.FlyingState);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.FallingState);
            }

        }
    }
}