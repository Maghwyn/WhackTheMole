using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class TeleportEvent : MonoBehaviour
{
	public event Action OnAnchorEnter;
	public event Action OnAnchorExit;

	public TeleportationProvider teleportationProvider;

	public void OnAnchorEntered()
	{
		if (AnchorTeleportManager.Instance != null)
		{
			AnchorTeleportManager.Instance.RegisterTeleport(this);
		}

		OnAnchorEnter?.Invoke();
	}

	public void OnAnchorExited()
	{
		OnAnchorExit?.Invoke();
	}
}
