using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Random = UnityEngine.Random;

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
	[SerializeField] private FloatVariable _rate;

	private Quaternion _moleRotation = Quaternion.Euler(-90, 0, 180);
	private float _currentSpawnInterval;
	private readonly Dictionary<int, Enemy> _enemies = new();
	private Coroutine _spawnEnemiesCoroutine;

	[Serializable]
	public class MoleHole
	{
		public int index;
		public Transform spawnTransform;
		public Transform upperTransform;
		public Transform lowerTransform;
	}

	private void Awake()
	{
		_currentSpawnInterval = _maxSpawnInterval;
	}

	private void Update()
	{
		_currentSpawnInterval = Mathf.Max(_minSpawnInterval, _currentSpawnInterval - (_rate.value * Time.deltaTime));
	}

	private void OnEnable()
	{
		_spawnEnemiesCoroutine ??= StartCoroutine(SpawnEnemiesAtInterval());
	}

	private void OnDisable()
	{
		if (_spawnEnemiesCoroutine != null)
		{
			StopCoroutine(_spawnEnemiesCoroutine);
			_spawnEnemiesCoroutine = null;
		}
	}

	public void EndTask()
	{
		if (_spawnEnemiesCoroutine != null)
		{
			StopCoroutine(_spawnEnemiesCoroutine);
			_spawnEnemiesCoroutine = null;
		}

		KillAllEnemies();
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
		GameObject prefab = Random.value < _safeMoleSpawnRate ? _safeMolePrefab : _molePrefab;

		GameObject enemyGO = Instantiate(prefab, moleHole.spawnTransform.position, _moleRotation);
		Enemy enemy = enemyGO.GetComponent<Enemy>();
		enemy.movement.InitializeMaxMinPosition(moleHole.upperTransform.position, moleHole.lowerTransform.position);

		enemy.OnSelfDestroy += () => OnEnemyDestroy(moleHole.index);

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
			enemy.InstantKill();
		}
		_enemies.Clear();
	}

	private void OnEnemyDestroy(int keyIndex)
	{
		_enemies.Remove(keyIndex);
	}
}