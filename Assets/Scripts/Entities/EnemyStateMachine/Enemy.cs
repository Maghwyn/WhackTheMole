using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy: MonoBehaviour, IEnemy
{
	#region STATE MACHINE
	public EnemyStateMachine stateMachine { get; protected set; }
	#endregion

	#region PREFAB CACHED COMPONENTS
	private Dictionary<IEnemy.MachineBehavior, GameObject> _attackPrefabsCache;
	#endregion

	#region MECHANICS
	public EnemyMovementMechanic movement { get ; protected set; }
	#endregion

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
	}

	protected virtual void Start() {}

	protected virtual void Update()
	{
		stateMachine.currentEnemyState.FrameUpdate();
	}

	protected virtual void FixedUpdate()
	{
		stateMachine.currentEnemyState.PhysicsUpdate();
	}
	#endregion

	#region STATE MACHINE SET STATE
	public virtual EnemyState GetState(IEnemy.MachineState state) { return null; }
	public virtual IEnemyBehavior GetStateBehavior(IEnemy.MachineBehavior behavior) { return null; }
	#endregion

	#region PREFABS
	public GameObject GetAttackPrefabForBehavior(IEnemy.MachineBehavior behavior)
	{
		if (_attackPrefabsCache.TryGetValue(behavior, out GameObject prefab))
			return prefab;
			
		Debug.LogWarning($"No attack prefab found for behavior: {behavior.HumanName()}");
		return null;
	}
	#endregion

	#region ENUM SETTINGS

	public enum TriggerType {
		SoundFX,
		AnimationEnd,
	}
	#endregion
}