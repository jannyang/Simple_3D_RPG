namespace RPGStateMachineSystem
{
    public class PlayerLandingState : PlayerGroundedState
    {
        public PlayerLandingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.LandingParameterHash);

            DisableCameraRecentering();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.LandingParameterHash);
        }
    }
}