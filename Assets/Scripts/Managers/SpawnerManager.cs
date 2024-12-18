using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;

[System.Serializable]
public class MoleHole
{
	public int index;
	public Transform spawnTransform;
	public Transform upperTransform;
	public Transform lowerTransform;
}

public class SpawnerManager : MonoBehaviour
{
	[Header("General Settings")]
	[SerializeField] private List<MoleHole> _molesHoles = new();
	[SerializeField] private float _maxSpawnInterval = 2f;
	[SerializeField] private float _minSpawnInterval = 0.75f;
	[SerializeField] private int _maxSpawnEnemies = 5;
	[SerializeField] private float _speedGrowth = 0.05f;
	[SerializeField] private static float _speedMultiplier = 1f;

	[Header("Spawn Rates")]
	[SerializeField] private float _noHitMoleSpawnRate = 0.2f;
	[SerializeField] private float _goldenMoleSpawnRate = 0.15f;
	[SerializeField] private float _healthMoleSpawnRate = 0.05f;

	[Header("Enemies Prefab")]
	[SerializeField] private GameObject _regularMolePrefab;
	[SerializeField] private GameObject _noHitMolePrefab;
	[SerializeField] private GameObject _goldenMolePrefab;
	[SerializeField] private GameObject _healthMolePrefab;
	[SerializeField] private List<GameObject> _regularMolePrefabs;

	[Header("SO Float References")]
	[SerializeField] private FloatVariable _rate;

	private Quaternion _moleRotation = Quaternion.Euler(-90, 0, 180);
	private float _currentSpawnInterval;
	private readonly Dictionary<int, Enemy> _enemies = new();
	private Coroutine _spawnEnemiesCoroutine;
	private Coroutine _increaseSpeedOverTimeCoroutine;

	private void Start()
	{
		_speedMultiplier = 1f;
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
		_currentSpawnInterval = _maxSpawnInterval;
		_speedMultiplier = 1f;
		StartTask();
	}

	public void TerminateSpawAndInteraction()
	{
		EndTask();
		KillAllEnemies();
	}

	public void ResumeSpawAndInteraction()
	{
		StartTask();
		UnPauseAllEnemies();
	}

	public void PauseSpawnAndInteraction()
	{
		EndTask();
		PauseAllEnemies();
	}

	private void StartTask()
	{
		_spawnEnemiesCoroutine ??= StartCoroutine(SpawnEnemiesAtInterval());
		_increaseSpeedOverTimeCoroutine ??= StartCoroutine(IncreaseSpeedOvertime());
	}

	private void EndTask()
	{
		if (_spawnEnemiesCoroutine != null)
		{
			StopCoroutine(_spawnEnemiesCoroutine);
			StopCoroutine(_increaseSpeedOverTimeCoroutine);
			_spawnEnemiesCoroutine = null;
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
	
	IEnumerator IncreaseSpeedOvertime()
	{
		for (;;)
		{
			_speedMultiplier += _speedGrowth;
			yield return new WaitForSeconds(3f);
		}
	}

	private void SpawnEnemy()
	{
		MoleHole moleHole = GetRandomFreeMoleHole();
		GameObject prefab = DetermineMolePrefab();

		GameObject enemyGO = Instantiate(prefab, moleHole.spawnTransform.position, _moleRotation);
		Enemy enemy = enemyGO.GetComponent<Enemy>();
		enemy.GetComponent<EnemyMovementMechanic>().SetSpeedMultiplier(_speedMultiplier);
		enemy.movement.InitializeMaxMinPosition(moleHole.upperTransform.position, moleHole.lowerTransform.position);

		enemy.OnSelfDestroy += () => OnEnemyDestroy(moleHole.index);

		_enemies[moleHole.index] = enemy;
	}

	private GameObject DetermineMolePrefab()
	{
		float randomValue = Random.value;
		
		if (randomValue < _healthMoleSpawnRate)
			return _healthMolePrefab;
		
		randomValue -= _healthMoleSpawnRate;
		if (randomValue < _goldenMoleSpawnRate)
			return _goldenMolePrefab;
			
		randomValue -= _goldenMoleSpawnRate;
		if (randomValue < _noHitMoleSpawnRate)
			return _noHitMolePrefab;
		
		int randomIndex = Random.Range(0, _regularMolePrefabs.Count);
		return _regularMolePrefabs[randomIndex];
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

	private void PauseAllEnemies()
	{
		foreach (var enemy in _enemies.Values)
		{
			enemy.Freeze();
		}
	}

	private void UnPauseAllEnemies()
	{
		foreach (var enemy in _enemies.Values)
		{
			enemy.UnFreeze();
		}
	}

	private void OnEnemyDestroy(int keyIndex)
	{
		_enemies.Remove(keyIndex);
	}
}