using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnerManager : MonoBehaviour
{
	[Header("General Settings")]
	[SerializeField] private List<MoleHole> _molesHoles = new();
	[SerializeField] private float _maxSpawnInterval = 2f;
	[SerializeField] private float _minSpawnInterval = 0.5f;
	[SerializeField] private int _maxSpawnEnemies = 5;
	[SerializeField] private float _safeMoleSpawnRate = 0.2f;

	[Header("Enemies Prefab")]
	[SerializeField] private GameObject _molePrefab;
	[SerializeField] private GameObject _safeMolePrefab;

	[Header("SO Float References")]
	[SerializeField] private FloatVariable _gameHP;
	[SerializeField] private FloatVariable _rate;

	private float _currentSpawnInterval;
	private readonly Dictionary<int, GameObject> _enemies = new();

	public class MoleHole
	{
		public int index;
		public Transform transform;
	}

	private void Start()
	{
		_currentSpawnInterval = _maxSpawnInterval;
		StartCoroutine(SpawnEnemiesAtInterval());
	}

	private void Update()
	{
		if (_gameHP.value <= 0)
		{
			StopCoroutine(SpawnEnemiesAtInterval());
			KillAllEnemies();
			gameObject.SetActive(false);
		}
		else
		{
			_currentSpawnInterval = Mathf.Max(_minSpawnInterval, _currentSpawnInterval - (_rate.value * Time.deltaTime));
		}
	}

	IEnumerator SpawnEnemiesAtInterval()
	{
		while (true)
		{
			if (_enemies.Count < _maxSpawnEnemies) SpawnEnemy();
			yield return new WaitForSeconds(_currentSpawnInterval);
		}
	}

	private void SpawnEnemy()
	{
		MoleHole moleHole = GetRandomFreeMoleHole();
		GameObject enemy;
		if (Random.value < _safeMoleSpawnRate)
		{
			enemy = Instantiate(_safeMolePrefab, moleHole.transform.position, Quaternion.identity);
		}
		else
		{
			enemy = Instantiate(_molePrefab, moleHole.transform.position, Quaternion.identity);
		}
		_enemies[moleHole.index] = enemy;
	}

	private MoleHole GetRandomFreeMoleHole()
	{
		int randomIndex;
		do
		{
			randomIndex = Random.Range(0, _molesHoles.Count);
		} while (_enemies.ContainsKey(_molesHoles[randomIndex].index));
		return _molesHoles[randomIndex];
	}

	private void KillAllEnemies()
	{
		foreach (var enemy in _enemies.Values)
		{
			enemy.GetComponent<Enemy>().ForceKill();
		}
		_enemies.Clear();
	}
}