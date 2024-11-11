using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class HitEnemy : MonoBehaviour
{
	[SerializeField] private MiniGameDataManager _miniGameDataManager;
	[SerializeField] private ParticleSystem _bonkParticles;

	private Enemy _enemy;
	private bool _isHit = false;

	public void Start()
	{
		_enemy = gameObject.GetComponentInParent<Enemy>();
		_miniGameDataManager = FindObjectOfType<MiniGameDataManager>();
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (_isHit) return;

		GameObject GO = collider.gameObject;
		if (!GO.CompareTag("Hammer")) return;
		_isHit = true;

		MoleType moleType = _enemy.type;
		_miniGameDataManager.HandleMoleHit(moleType, _enemy.scorePoint);

		Instantiate(_bonkParticles, transform.position, Quaternion.identity);
		_enemy.DelayedKill();
	}
}
