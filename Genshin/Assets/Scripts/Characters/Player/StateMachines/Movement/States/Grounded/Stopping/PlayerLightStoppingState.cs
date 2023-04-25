namespace RPGStateMachineSystem
{
    public class PlayerLightStoppingState : PlayerStoppingState
    {
        public PlayerLightStoppingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementDecelerationForce = groundedData.StopData.LightDecelerationForce;

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeakForce;
        }
    }
}