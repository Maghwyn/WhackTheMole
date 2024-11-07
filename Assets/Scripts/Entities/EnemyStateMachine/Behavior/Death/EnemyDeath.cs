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

		Destroy(enemy);
	}

	public override void DoPhysicsLogic()
	{
		base.DoPhysicsLogic();
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