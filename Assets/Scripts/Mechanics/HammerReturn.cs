using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HammerReturn : MonoBehaviour
{
	[SerializeField] private XRGrabInteractable _hammer;
	[SerializeField] private float _hammerDistanceCheckingInterval = 0.5f;

	private XRSocketInteractor _socket;
	private Coroutine _returnToSocketCoroutine;
	public event Action OnSocketReturn;

	public bool isSnapped => _socket.isPerformingManualInteraction;

	private void Awake()
	{
		_socket = GetComponent<XRSocketInteractor>();
	}

	public void OnSocketSnapEnter()
	{
		StopReturnToSocket();
	}

	public void OnSocketSnapExit()
	{
		StartReturnToSocket();
	}

	public void StartReturnToSocket()
	{
		_returnToSocketCoroutine ??= StartCoroutine(ReturnToSocket());
	}

	public void StopReturnToSocket()
	{
		if (_returnToSocketCoroutine != null)
		{
			StopCoroutine(_returnToSocketCoroutine);
			_returnToSocketCoroutine = null;
		}
	}

	private IEnumerator ReturnToSocket()
	{
		while (true)
		{
			float distance = Vector3.Distance(_socket.transform.position, _hammer.transform.position);
			if (distance > 5f)
			{
				_socket.StartManualInteraction(_hammer as IXRSelectInteractable);
				OnSocketReturn?.Invoke();
			}
			yield return new WaitForSeconds(_hammerDistanceCheckingInterval);
		}
	}

	public void ForceReturnToSocket()
	{
		_socket.StartManualInteraction(_hammer as IXRSelectInteractable);
		OnSocketReturn?.Invoke();
	}
}
