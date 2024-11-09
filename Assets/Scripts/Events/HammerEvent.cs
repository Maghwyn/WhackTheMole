using System;
using UnityEngine;

public class HammerEvent : MonoBehaviour
{
	public event Action OnHammerGrab;
	public event Action OnHammerDrop;

	public bool isGrabbed { get; private set; } = false;

	public void OnHammerGrabbed()
	{
		isGrabbed = true;
		OnHammerGrab?.Invoke();
	}

	public void OnHammerDropped()
	{
		isGrabbed = false;
		OnHammerDrop?.Invoke();
	}
}
