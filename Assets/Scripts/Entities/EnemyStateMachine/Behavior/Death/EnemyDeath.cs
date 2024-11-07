using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Logic/Death Logic/Death", fileName = "Death-Death")]
public class EnemyDeath : EnemyDeathSOBase
{
	public override void Initialize(GameObject gameObject, Enemy enemy)
	{
		base.Initialize(gameObject, enemy);
	}

	public override void DoEnterLogic()
	{
		base.DoEnterLogic();
	}

	public override void DoExitLogic()
	{
		base.DoExitLogic();
	}

	public override void DoFrameUpdateLogic()
	{
		base.DoFrameUpdateLogic();

		DoStateChange();
	}

	public override void DoPhysicsLogic()
	{
		base.DoPhysicsLogic();

		// Perform movement
	}

	public override void DoStateChange()
	{
		base.DoStateChange();

		// If Z transform hasn't reached the point (don't do anything)
	}

	public override void DoAnimationStartLogic()
	{
		base.DoAnimationStartLogic();
	}

	public override void DoAnimationSoundTriggerEventLogic()
	{
		base.DoAnimationSoundTriggerEventLogic();
	}

	public override void DoAnimationEndTriggerEventLogic()
	{
		base.DoAnimationEndTriggerEventLogic();
	}

	public override void ResetValues()
	{
		base.ResetValues();
	}
}