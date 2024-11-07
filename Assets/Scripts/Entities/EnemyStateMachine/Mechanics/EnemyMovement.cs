using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public sealed class EnemyMovementMechanic : MonoBehaviour
{
	[Header("General Settings")]
	[SerializeField] private bool _useDOTween = true;
	[SerializeField] private float _movementDuration = 1f;
	[SerializeField] private Ease _movementEase = Ease.InOutSine;
	[SerializeField] private float _speed = 50f;

	private Rigidbody _rb;

	private Vector3 _upperTargetPos;
	private Vector3 _lowerTargetPos;

	public enum MovementDirection { Up, Down }
	public event Action OnPositionReached;

	public void InitializeMaxMinPosition(Vector3 max, Vector3 min)
	{
		_upperTargetPos = max;
		_lowerTargetPos = min;
	}

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}

	internal void PerformMovement(MovementDirection direction)
	{
		if (_useDOTween)
		{
			PerformMovementDOTween(direction);
		}
		else
		{
			PerformMovementVelocity(direction);
		}
	}

	private void PerformMovementDOTween(MovementDirection direction)
	{
		Vector3 targetPosition = direction == MovementDirection.Up ? _upperTargetPos : _lowerTargetPos;

		transform.DOMove(targetPosition, _movementDuration)
			.SetEase(_movementEase)
			.OnComplete(() =>
			{
				OnPositionReached?.Invoke();
			});
	}

	private void PerformMovementVelocity(MovementDirection direction)
	{
		Vector3 targetPosition = direction == MovementDirection.Up ? _upperTargetPos : _lowerTargetPos;

		Vector3 directionVector = (targetPosition - transform.position).normalized;
		float distance = Vector3.Distance(transform.position, targetPosition);

		float _speedFactor = Mathf.SmoothStep(0, _speed, distance / 10f);
		Vector3 velocity = directionVector * _speedFactor;
		_rb.MovePosition(_rb.position + velocity);

		// Snap if close to target
		if (distance < 0.1f)
		{
			_rb.MovePosition(_rb.position + Vector3.zero);
			OnPositionReached?.Invoke();
		}
	}
}
