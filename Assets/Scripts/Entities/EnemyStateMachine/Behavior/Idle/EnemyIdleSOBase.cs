using UnityEngine;

public class EnemyIdleSOBase : ScriptableObject, IEnemyBehavior
{
	protected Enemy enemy;
	protected Transform transform;
	protected GameObject gameObject;

	protected float stateTime;
	protected float stateDuration;

	public virtual void Initialize(GameObject gameObject, Enemy enemy)
	{
		this.enemy = enemy;
		this.gameObject = gameObject;
		transform = gameObject.transform;

		stateTime = 0f;
		stateDuration = 0.5f;
	}

	public virtual void DoEnterLogic() {}
	public virtual void DoExitLogic() { ResetValues(); }
	public virtual void DoFrameUpdateLogic() {}
	public virtual void DoPhysicsLogic() {}
	public virtual void DoStateChange() {}
	public virtual void DoAnimationStartLogic() {}
	public virtual void DoAnimationSoundTriggerEventLogic() {}
	public virtual void DoAnimationEndTriggerEventLogic() {}
	public virtual void ResetValues()
	{
		stateTime = 0f;
		stateDuration = 0.5f;
	}
}
