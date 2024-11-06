using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnerManager : MonoBehaviour
{
	public GameObject enemyPrefab;
	public int numEnemies = 1;
	public float spawnInterval = 10f;
	private List<GameObject> enemies = new List<GameObject>();

	void Start()
	{
		StartCoroutine(SpawnEnemiesAtInterval());
	}

	IEnumerator SpawnEnemiesAtInterval()
	{
		while (true)
		{
			SpawnEnemies();
			spawnInterval = Mathf.Max(2f, spawnInterval * 0.95f);
			yield return new WaitForSeconds(spawnInterval);
		}
	}

	void SpawnEnemies()
	{
		for (int i = 0; i < numEnemies; i++)
		{
			// TODO
			Vector3 spawnPosition = Vector3.zero;
			GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
			enemies.Add(enemy);
		}
	}
}