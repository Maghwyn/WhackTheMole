using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Logic/Idle Logic/Idle Visible", fileName = "Idle-Idle-Visible")]
public class EnemyIdleVisible : EnemyIdleSOBase
{

	public override void Initialize(GameObject gameObject, Enemy enemy)
	{
		base.Initialize(gameObject, enemy);
	}

	public override void DoEnterLogic()
	{
		base.DoEnterLogic();
		stateDuration = 1f;
	}

	public override void DoExitLogic()
	{
		base.DoExitLogic();
	}

	public override void DoFrameUpdateLogic()
	{
		base.DoFrameUpdateLogic();
		stateTime += Time.deltaTime;

		DoStateChange();
	}

	public override void DoPhysicsLogic()
	{
		base.DoPhysicsLogic();
	}

	public override void DoStateChange()
	{
		base.DoStateChange();

		if (stateTime >= stateDuration)
		{
			enemy.stateMachine.ChangeState(enemy.GetState(IEnemy.MachineState.Down), IEnemy.MachineBehavior.Down);
		}
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