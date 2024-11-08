using System;
using UnityEngine;

public class TeleportEvent : MonoBehaviour
{
	public event Action OnAnchorEnter;
	public event Action OnAnchorExit;

	public void OnAnchorEntered()
	{
		OnAnchorEnter?.Invoke();
	}

	public void OnAnchorExite()
	{
		OnAnchorExit?.Invoke();
	}
}
