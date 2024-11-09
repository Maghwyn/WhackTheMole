public interface IEnemy
{
	EnemyStateMachine stateMachine { get; }
	EnemyMovementMechanic movement { get; }
	EnemyState GetState(MachineState state);
	IEnemyBehavior GetStateBehavior(MachineBehavior behavior);

	public enum MachineState {
		Idle,
		Up,
		Down,
		Escaped,
		Death,
	}

	public enum MachineBehavior {
		IdleHidden,
		IdleVisible,
		Up,
		Down,
		EscapedDoDamage,
		Escaped,
		InstantDeath,
		DelayedDeath,
	}
}