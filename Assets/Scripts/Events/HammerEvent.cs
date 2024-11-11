using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// TODO [FUTURE PROJECT] : Make this a Generic class that can be extended
public class HammerEvent : MonoBehaviour
{
	public event Action OnHammerHandGrab;
	public event Action OnHammerHandDrop;

	public bool isGrabbedByHand { get; private set; } = false;
	public bool isGrabbedBySocket { get; private set; } = false;

	public void OnHammerGrabbed(SelectEnterEventArgs eventArgs)
	{
		string interactorTag = eventArgs.interactorObject.transform.tag;

		switch (interactorTag)
		{
			case "RightHandInteractor":
				isGrabbedByHand = true;
				OnHammerHandGrab?.Invoke();
				break;

			case "LeftHandInteractor":
				isGrabbedByHand = true;
				OnHammerHandGrab?.Invoke();
				break;

			case "SocketInteractor":
				isGrabbedBySocket = true;
				break;

			default:
				Debug.Log("Unknown interactor detected.");
				break;
		}
	}

	public void OnHammerDropped(SelectExitEventArgs eventArgs)
	{
		string interactorTag = eventArgs.interactorObject.transform.tag;

		switch (interactorTag)
		{
			case "RightHandInteractor":
				isGrabbedByHand = false;
				OnHammerHandDrop?.Invoke();
				break;

			case "LeftHandInteractor":
				isGrabbedByHand = false;
				OnHammerHandDrop?.Invoke();
				break;

			case "SocketInteractor":
				isGrabbedBySocket = false;
				break;

			default:
				Debug.Log("Unknown interactor detected.");
				break;
		}
	}
}
