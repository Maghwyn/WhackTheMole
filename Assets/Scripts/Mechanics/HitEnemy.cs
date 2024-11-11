using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class HitEnemy : MonoBehaviour
{
	[SerializeField] private MiniGameDataManager _miniGameDataManager;
	[SerializeField] private ParticleSystem _bonkParticles;

	private Enemy _enemy;
	private bool _isHit = false;
	private Vector3 _headOffset;

	public void Start()
	{
		_enemy = gameObject.GetComponentInParent<Enemy>();
		_miniGameDataManager = FindObjectOfType<MiniGameDataManager>();
		CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();

    	_headOffset = capsuleCollider.center + (Vector3.up * (capsuleCollider.height * 100) * 0.5f);
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (_isHit) return;

		GameObject GO = collider.gameObject;
		if (!GO.CompareTag("Hammer")) return;
		_isHit = true;

		MoleType moleType = _enemy.type;
		_miniGameDataManager.HandleMoleHit(moleType, _enemy.scorePoint);

		Vector3 headPosition = transform.position;
		headPosition.y = headPosition.y + _headOffset.y;
		Instantiate(_bonkParticles, headPosition, Quaternion.identity);
		_enemy.DelayedKill();
	}
}
