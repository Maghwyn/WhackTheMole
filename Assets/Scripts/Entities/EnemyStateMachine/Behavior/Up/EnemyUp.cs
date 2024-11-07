using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Logic/Up Logic/Up", fileName = "Up-Up")]
public class EnemyUp : EnemyUpSOBase
{
	public override void Initialize(GameObject gameObject, Enemy enemy)
	{
		base.Initialize(gameObject, enemy);
	}

	public override void DoEnterLogic()
	{
		base.DoEnterLogic();
		enemy.movement.OnPositionReached += OnPositionReached;
	}

	public override void DoExitLogic()
	{
		base.DoExitLogic();
		enemy.movement.OnPositionReached -= OnPositionReached;
	}

	public override void DoFrameUpdateLogic()
	{
		base.DoFrameUpdateLogic();

		DoStateChange();
	}

	public override void DoPhysicsLogic()
	{
		base.DoPhysicsLogic();

		enemy.movement.PerformMovement(EnemyMovementMechanic.MovementDirection.Up);
	}

	public override void DoStateChange()
	{
		base.DoStateChange();

		if (!isPositionReached) return;
		enemy.stateMachine.ChangeState(enemy.GetState(IEnemy.MachineState.Idle), IEnemy.MachineBehavior.IdleVisible);
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

	private void OnPositionReached()
	{
		isPositionReached = true;
	}
}