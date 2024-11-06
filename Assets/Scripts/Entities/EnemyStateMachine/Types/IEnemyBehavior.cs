public interface IEnemyBehavior
{	
	public void DoEnterLogic();
	public void DoExitLogic();
	public void DoFrameUpdateLogic();
	public void DoPhysicsLogic();
	public void DoStateChange();
	public void DoAnimationStartLogic();
	public void DoAnimationSoundTriggerEventLogic();
	public void DoAnimationEndTriggerEventLogic();
	public void ResetValues();
}