using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class HitEnemy : MonoBehaviour
{
	[SerializeField] private MiniGameDataManager _miniGameDataManager;

	private Enemy _enemy;

	public void Start()
	{
		_enemy = gameObject.GetComponentInParent<Enemy>();
		_miniGameDataManager = FindObjectOfType<MiniGameDataManager>();
	}

	private void OnTriggerEnter(Collider collider)
	{
		GameObject GO = collider.gameObject;
		if (!GO.CompareTag("Hammer")) return;


		MoleType moleType = _enemy.type;
		_miniGameDataManager.HandleMoleHit(moleType, _enemy.scorePoint);

		// TODO: Instantiate a VFX prefab to create a hit effect
		_enemy.DelayedKill();
	}
}
