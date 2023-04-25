namespace RPGStateMachineSystem
{
    public class PlayerMovingState : PlayerGroundedState
    {
        public PlayerMovingState(PlayerStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(stateMachine.Player.AnimationData.MovingParameterHash);
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.MovingParameterHash);
        }

        public override void Update()
        {
            base.Update();

            if (stateMachine.ReusableData.AttackInput)
            {
                OnAttack();
                return;
            }
        }
    }
}