using UnityEngine;

public class SimulatedBreathing : MonoBehaviour
{
	[Header("Breathing Settings")]
	[SerializeField] private float _scaleAmplitude = 10f;
	[SerializeField] private float _rotationAmplitude = 5f;
	[SerializeField] private float _frequency = 1f;

	private Vector3 _initialScale;
	private Vector3 _initialLocalEuler;

	private void Start()
	{
		_initialScale = transform.localScale;
		_initialLocalEuler = transform.localEulerAngles;
	}

	private void Update()
	{
		float time = Time.time * _frequency;
		float scaleOffset = Mathf.Sin(time) * _scaleAmplitude;
		float rotationOffset = Mathf.Cos(time) * _rotationAmplitude;

		transform.localScale = new Vector3(
			_initialScale.x + scaleOffset,
			_initialScale.y,
			_initialScale.z + scaleOffset
		);

		// Inversed Z and Y, not sure why it's switching in the first place
		transform.localEulerAngles = new Vector3(
			_initialLocalEuler.x + rotationOffset,
			_initialLocalEuler.z,
			_initialLocalEuler.y
		);
	}
}