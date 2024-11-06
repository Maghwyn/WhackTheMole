using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Logic/Escaped Logic/Escaped", fileName = "Escaped-Escaped")]
public class EnemyEscaped : EnemyEscapedSOBase
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

		// Nothing happens, the enemy just escaped and self-destroy
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