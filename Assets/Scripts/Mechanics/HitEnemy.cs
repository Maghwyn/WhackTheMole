using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class HitEnemy : MonoBehaviour
{
	[SerializeField] private ParticleSystem _bonkParticles;
	
	private MiniGameDataManager _miniGameDataManager;

	private Enemy _enemy;
	private bool _isHit = false;
	private Vector3 _headOffset;

	public void Start()
	{
		_enemy = gameObject.GetComponentInParent<Enemy>();
		_miniGameDataManager = FindObjectOfType<MiniGameDataManager>();
		CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
		
		_headOffset = capsuleCollider.center + (capsuleCollider.height * 100 * 0.5f * Vector3.up);
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (_isHit) return;

		GameObject GO = collider.gameObject;
		if (!GO.CompareTag("Hammer")) return;
		_isHit = true;

		XRController device = InputSystem.GetDevice<XRController>(CommonUsages.RightHand);
		var command = UnityEngine.InputSystem.XR.Haptics.SendHapticImpulseCommand.Create(0, 0.5f, 0.2f);
		device.ExecuteCommand(ref command);

		MoleType moleType = _enemy.type;
		_miniGameDataManager.HandleMoleHit(moleType, _enemy.scorePoint);

		Vector3 headPosition = transform.position;
		headPosition.y += _headOffset.y;
		Instantiate(_bonkParticles, headPosition, Quaternion.identity);
		_enemy.DelayedKill();
	}
}
