using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public sealed class EnemyMovementMechanic : MonoBehaviour
{
	[Header("General Settings")]
	[SerializeField] private bool _useDOTween = true;
	[SerializeField] private float _movementDuration = 2f;
	[SerializeField] private Ease _movementEase = Ease.InOutSine;

	[Header("Targets")]
	[SerializeField] private Transform _upperTarget;
	[SerializeField] private Transform _lowerTarget;
	[SerializeField] private float _speed = 5f;

	private Rigidbody _rb;

	public enum MovementDirection { Up, Down }

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
		Transform target = direction == MovementDirection.Up ? _upperTarget : _lowerTarget;

		transform.DOMove(target.position, _movementDuration)
			.SetEase(_movementEase);
	}

	private void PerformMovementVelocity(MovementDirection direction)
	{
		Transform target = direction == MovementDirection.Up ? _upperTarget : _lowerTarget;
		Vector3 targetPosition = target.position;

		Vector3 directionVector = (targetPosition - transform.position).normalized;
		float distance = Vector3.Distance(transform.position, targetPosition);

		float _speedFactor = Mathf.SmoothStep(0, _speed, distance / 10f);
		_rb.velocity = directionVector * _speedFactor;

		// Snap if close to target
		if (distance < 0.1f)
		{
			_rb.velocity = Vector3.zero;
		}
	}
}
