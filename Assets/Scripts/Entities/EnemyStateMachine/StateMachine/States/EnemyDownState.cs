public class EnemyDownState : EnemyState
{
	private IEnemy.MachineBehavior _currentBehavior;

	public EnemyDownState(Enemy Enemy, EnemyStateMachine EnemyStateMachine, IEnemy.MachineBehavior defaultBehavior) : base(Enemy, EnemyStateMachine)
	{
		_currentBehavior = defaultBehavior;
	}

	public override void AnimationTriggerEvent(IEnemy.MachineState animationTriggerType, Enemy.TriggerType triggerType)
	{
		base.AnimationTriggerEvent(animationTriggerType, triggerType);

		if (triggerType == Enemy.TriggerType.SoundFX) {
			Enemy.GetStateBehavior(_currentBehavior).DoAnimationSoundTriggerEventLogic();
		} else if (triggerType == Enemy.TriggerType.AnimationEnd) {
			Enemy.GetStateBehavior(_currentBehavior).DoAnimationEndTriggerEventLogic();
		}
	}

	public override void EnterState(IEnemy.MachineBehavior withBehavior)
	{
		base.EnterState(withBehavior);
		_currentBehavior = withBehavior;
		Enemy.GetStateBehavior(_currentBehavior).DoEnterLogic();
	}

	public override void ExitState()
	{
		base.ExitState();

		Enemy.GetStateBehavior(_currentBehavior).DoExitLogic();
	}

	public override void FrameUpdate()
	{
		base.FrameUpdate();

		Enemy.GetStateBehavior(_currentBehavior).DoFrameUpdateLogic();
	}

	public override void PhysicsUpdate()
	{
		base.PhysicsUpdate();

		Enemy.GetStateBehavior(_currentBehavior).DoPhysicsLogic();
	}
}