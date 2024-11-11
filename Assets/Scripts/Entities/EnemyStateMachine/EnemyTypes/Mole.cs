using Unity.VisualScripting;
using UnityEngine;

public class Mole: Enemy
{
	#region SM Variables
	private EnemyIdleState _idleState;
	private EnemyUpState _upState;
	private EnemyDownState _downState;
	private EnemyEscapedState _escapedState;
	private EnemyDeathState _deathState;
	#endregion

	#region SO Variables

	[Header("Behaviors")]
	[SerializeField] private EnemyIdleSOBase _enemyIdleHiddenBase;
	[SerializeField] private EnemyIdleSOBase _enemyIdleVisibleBase;
	[SerializeField] private EnemyUpSOBase _enemyUpBase;
	[SerializeField] private EnemyDownSOBase _enemyDownBase;
	[SerializeField] private EnemyEscapedSOBase _enemyEscapedDoDamageBase;
	[SerializeField] private EnemyDeathSOBase _enemyInstantDeathBase;
	[SerializeField] private EnemyDeathSOBase _enemyDelayedDeathBase;

	private EnemyIdleSOBase EnemyIdleHiddenBaseInstance;
	private EnemyIdleSOBase EnemyIdleVisibleBaseInstance;
	private EnemyUpSOBase EnemyUpBaseInstance;
	private EnemyDownSOBase EnemyDownBaseInstance;
	private EnemyEscapedSOBase EnemyEscapedDoDamageBaseInstance;
	private EnemyDeathSOBase EnemyInstantDeathBaseInstance;
	private EnemyDeathSOBase EnemyDelayedDeathBaseInstance;

	#endregion

	#region MONOBEHAVIOR

	protected override void Awake()
	{
		base.Awake();

		EnemyIdleHiddenBaseInstance = Instantiate(_enemyIdleHiddenBase);
		EnemyIdleVisibleBaseInstance = Instantiate(_enemyIdleVisibleBase);
		EnemyUpBaseInstance = Instantiate(_enemyUpBase);
		EnemyDownBaseInstance = Instantiate(_enemyDownBase);
		EnemyEscapedDoDamageBaseInstance = Instantiate(_enemyEscapedDoDamageBase);
		EnemyInstantDeathBaseInstance = Instantiate(_enemyInstantDeathBase);
		EnemyDelayedDeathBaseInstance = Instantiate(_enemyDelayedDeathBase);

		_idleState = new EnemyIdleState(this, base.stateMachine, IEnemy.MachineBehavior.IdleHidden);
		_upState = new EnemyUpState(this, base.stateMachine, IEnemy.MachineBehavior.Up);
		_downState = new EnemyDownState(this, base.stateMachine, IEnemy.MachineBehavior.Down);
		_escapedState = new EnemyEscapedState(this, base.stateMachine, IEnemy.MachineBehavior.EscapedDoDamage);
		_deathState = new EnemyDeathState(this, base.stateMachine, IEnemy.MachineBehavior.InstantDeath);
	}

	protected override void Start()
	{
		base.Start();

		EnemyIdleHiddenBaseInstance.Initialize(gameObject, this);
		EnemyIdleVisibleBaseInstance.Initialize(gameObject, this);
		EnemyUpBaseInstance.Initialize(gameObject, this);
		EnemyDownBaseInstance.Initialize(gameObject, this);
		EnemyEscapedDoDamageBaseInstance.Initialize(gameObject, this);
		EnemyInstantDeathBaseInstance.Initialize(gameObject, this);
		EnemyDelayedDeathBaseInstance.Initialize(gameObject, this);

		base.stateMachine.Initialize(_idleState, IEnemy.MachineBehavior.IdleHidden);
	}

	public override EnemyState GetState(IEnemy.MachineState state)
	{
		switch (state)
		{
			case IEnemy.MachineState.Idle:
				return _idleState;
			case IEnemy.MachineState.Up:
				return _upState;
			case IEnemy.MachineState.Down:
				return _downState;
			case IEnemy.MachineState.Escaped:
				return _escapedState;
			case IEnemy.MachineState.Death:
				return _deathState;

			default:
				Debug.LogWarning($"State {state} not found for {this.name}");
				return null;
		}
	}

	public override IEnemyBehavior GetStateBehavior(IEnemy.MachineBehavior behavior)
	{
		switch (behavior)
		{
			case IEnemy.MachineBehavior.IdleHidden:
				return EnemyIdleHiddenBaseInstance;
			case IEnemy.MachineBehavior.IdleVisible:
				return EnemyIdleVisibleBaseInstance;
			case IEnemy.MachineBehavior.Up:
				return EnemyUpBaseInstance;
			case IEnemy.MachineBehavior.Down:
				return EnemyDownBaseInstance;
			case IEnemy.MachineBehavior.EscapedDoDamage:
				return EnemyEscapedDoDamageBaseInstance;
			case IEnemy.MachineBehavior.InstantDeath:
				return EnemyInstantDeathBaseInstance;
			case IEnemy.MachineBehavior.DelayedDeath:
				return EnemyDelayedDeathBaseInstance;

			default:
				Debug.LogWarning($"Behavior {behavior} not found for {this.name}");
				return null;
		}
	}

	public override void InstantKill()
	{
		base.stateMachine.ChangeState(GetState(IEnemy.MachineState.Death), IEnemy.MachineBehavior.InstantDeath);
	}

	public override void DelayedKill()
	{
		base.stateMachine.ChangeState(GetState(IEnemy.MachineState.Death), IEnemy.MachineBehavior.DelayedDeath);
	}

	#endregion
}