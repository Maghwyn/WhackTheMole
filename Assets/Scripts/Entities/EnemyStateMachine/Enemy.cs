using System;
using UnityEngine;

public class Enemy: MonoBehaviour, IEnemy
{
	[Header("Enemy game data")]
	[SerializeField] public int scorePoint;
	[SerializeField] public MoleType type;

	#region STATE MACHINE
	public EnemyStateMachine stateMachine { get; protected set; }
	#endregion

	#region MECHANICS
	public EnemyMovementMechanic movement { get ; protected set; }
	#endregion

	#region PHYSICS
	private Collider2D _collider;
	#endregion

	#region FLAGS
	protected bool isFrozen = false;
	#endregion

	public event Action OnSelfDestroy;

	protected virtual void AnimationSoundTriggerEvent(IEnemy.MachineState animationTriggerType)
	{
		stateMachine.currentEnemyState.AnimationTriggerEvent(animationTriggerType, TriggerType.SoundFX);
	}

	protected virtual void AnimationEndTriggerEvent(IEnemy.MachineState animationTriggerType)
	{
		stateMachine.currentEnemyState.AnimationTriggerEvent(animationTriggerType, TriggerType.AnimationEnd);
	}

	#region MONOBEHAVIOUR
	protected virtual void Awake()
	{
		stateMachine = new EnemyStateMachine();
		movement = GetComponent<EnemyMovementMechanic>();
		_collider = GetComponent<Collider2D>();
	}

	protected virtual void Start() {}

	protected virtual void Update()
	{
		if (isFrozen) return;
		stateMachine.currentEnemyState.FrameUpdate();
	}

	protected virtual void FixedUpdate()
	{
		if (isFrozen) return;
		stateMachine.currentEnemyState.PhysicsUpdate();
	}
	#endregion

	#region STATE MACHINE SET STATE
	public virtual EnemyState GetState(IEnemy.MachineState state) { return null; }
	public virtual IEnemyBehavior GetStateBehavior(IEnemy.MachineBehavior behavior) { return null; }
	public virtual void InstantKill() {}
	public virtual void DelayedKill() {}
	public void Freeze()
	{
		isFrozen = true;
		_collider.enabled = false;
	}

	public void UnFreeze()
	{
		isFrozen = false;
		_collider.enabled = true;
	}
	#endregion

	public void InvokeOnSelfDestroy()
	{
		OnSelfDestroy?.Invoke();
	}

	#region ENUM SETTINGS

	public enum TriggerType {
		SoundFX,
		AnimationEnd,
	}
	#endregion
}