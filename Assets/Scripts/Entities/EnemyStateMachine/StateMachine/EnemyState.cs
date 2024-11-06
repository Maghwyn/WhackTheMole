public class EnemyState
{
	protected Enemy Enemy;
	protected EnemyStateMachine EnemyStateMachine;

	public EnemyState(Enemy Enemy, EnemyStateMachine EnemyStateMachine)
	{
		this.Enemy = Enemy;
		this.EnemyStateMachine = EnemyStateMachine;
	}

	public virtual void EnterState(IEnemy.MachineBehavior withBehavior) {}
	public virtual void ExitState() {}
	public virtual void FrameUpdate() {}
	public virtual void PhysicsUpdate() {}
	public virtual void AnimationTriggerEvent(IEnemy.MachineState animationTriggerType, Enemy.TriggerType triggerType) {}
}
