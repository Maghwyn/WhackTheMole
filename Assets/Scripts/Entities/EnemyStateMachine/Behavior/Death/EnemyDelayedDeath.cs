using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Logic/Death Logic/Delayed Death", fileName = "Death-Delayed-Death")]
public class EnemyDelayedDeath : EnemyDeathSOBase
{
	protected float stateTime;
	protected float stateDuration;
	protected bool isDeathAnimationActivated;

	public override void Initialize(GameObject gameObject, Enemy enemy)
	{
		base.Initialize(gameObject, enemy);
	}

	public override void DoEnterLogic()
	{
		base.DoEnterLogic();

		stateTime = 0f;
		stateDuration = 0.5f;
		isDeathAnimationActivated = false;
	}

	public override void DoExitLogic()
	{
		base.DoExitLogic();
	}

	public override void DoFrameUpdateLogic()
	{
		base.DoFrameUpdateLogic();
		stateTime += Time.deltaTime;

		if (stateTime >= stateDuration)
		{
			enemy.InvokeOnSelfDestroy();
			Destroy(enemy.gameObject);
		}
		else if (!isDeathAnimationActivated)
		{
			isDeathAnimationActivated = true;
			Vector3 enemyScale = enemy.transform.localScale;
			enemyScale.z /= 2;
			enemy.transform.localScale = enemyScale;
		}
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
		stateTime = 0f;
		stateDuration = 0.5f;
	}
}