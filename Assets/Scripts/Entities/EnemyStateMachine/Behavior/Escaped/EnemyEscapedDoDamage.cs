using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Logic/Escaped Logic/Escaped Do Damage", fileName = "Escaped-Escaped-Do-Damage")]
public class EnemyEscapedDoDamage : EnemyEscapedSOBase
{
	private MiniGameDataManager _miniGameDataManager;

	public override void Initialize(GameObject gameObject, Enemy enemy)
	{
		base.Initialize(gameObject, enemy);
		_miniGameDataManager = FindObjectOfType<MiniGameDataManager>();
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

		_miniGameDataManager.HandleMoleEscapedDoDamage();
		enemy.InvokeOnSelfDestroy();
		Destroy(enemy.gameObject);
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