using UnityEngine;

public class EnemyDeathSOBase : ScriptableObject, IEnemyBehavior
{
	protected Enemy enemy;
	protected Transform transform;
	protected GameObject gameObject;

	public virtual void Initialize(GameObject gameObject, Enemy enemy)
	{
		this.enemy = enemy;
		this.gameObject = gameObject;
		transform = gameObject.transform;
	}

	public virtual void DoEnterLogic() {}
	public virtual void DoExitLogic() { ResetValues(); }
	public virtual void DoFrameUpdateLogic() {}
	public virtual void DoPhysicsLogic() {}
	public virtual void DoStateChange() {}
	public virtual void DoAnimationStartLogic() {}
	public virtual void DoAnimationSoundTriggerEventLogic() {}
	public virtual void DoAnimationEndTriggerEventLogic() {}
	public virtual void ResetValues() {}
}
