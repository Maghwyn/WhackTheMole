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


		if (enemy.CompareTag("Mole"))
		{

		}
		else if (enemy.CompareTag("SafeMole"))
		{

		}

		// TODO: Instantiate a VFX prefab to create a hit effect
		enemy.DelayedKill();
	}
}
