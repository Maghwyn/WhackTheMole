public class EnemyStateMachine
{
	public EnemyState currentEnemyState;

	public void Initialize(EnemyState startingState, IEnemy.MachineBehavior withBehavior)
	{
		currentEnemyState = startingState;
		currentEnemyState.EnterState(withBehavior);
	}

	public void ChangeState(EnemyState newState, IEnemy.MachineBehavior withBehavior)
	{
		currentEnemyState.ExitState();
		currentEnemyState = newState;
		currentEnemyState.EnterState(withBehavior);
	}
}
