using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class HitEnemy : MonoBehaviour
{
	private Enemy enemy;

	public void Start()
	{
		enemy = gameObject.GetComponentInParent<Enemy>();
	}

	private void OnTriggerEnter(Collider collider)
	{
		GameObject GO = collider.gameObject;
		if (!GO.CompareTag("Hammer")) return;

		//Player player = GO.GetComponentInParent<Player>();

		if (enemy.CompareTag("Mole"))
		{
			//player.IncreaseScrore();
		}
		else if (enemy.CompareTag("SafeMole"))
		{
			//player.TakeDamage(1);
		}

		// TODO: Instantiate a VFX prefab to create a hit effect
		enemy.Kill();
	}
}
