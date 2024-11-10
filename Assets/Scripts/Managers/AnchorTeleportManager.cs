public class AnchorTeleportManager : Singleton<AnchorTeleportManager>
{
	private TeleportEvent _teleportEvent;

	public void RegisterTeleport(TeleportEvent newTeleportEvent)
	{
		if (_teleportEvent != null && _teleportEvent != newTeleportEvent)
		{
			_teleportEvent.OnAnchorExited();
		}

		_teleportEvent = newTeleportEvent;
	}
}
