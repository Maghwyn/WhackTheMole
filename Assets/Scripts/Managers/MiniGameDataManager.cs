using UnityEngine;

[System.Serializable]
public class MultiplierTier
{
	public float multiplierValue;
	public int hitsNeeded;
	public float penaltyReduction;
}

public class MiniGameDataManager : MonoBehaviour
{
	[Header("Game Base Data")]
	[SerializeField] private int _defaultGameHP = 5;
	[SerializeField] private int _defaultGameScore = 0;
	[SerializeField] private float _defaultGameMultiplier = 1;

	[Header("Game Data")]
	[SerializeField] private IntVariable _gameHP;
	[SerializeField] private IntVariable _gameScore;
	[SerializeField] private FloatVariable _gameMultiplier;

	[Header("Multiplier Settings")]
	[SerializeField] private MultiplierTier[] _multiplierTiers = new MultiplierTier[]
	{
		new() { multiplierValue = 1f, hitsNeeded = 0, penaltyReduction = 0f },
		new() { multiplierValue = 2f, hitsNeeded = 5, penaltyReduction = 0.20f },
		new() { multiplierValue = 4f, hitsNeeded = 15, penaltyReduction = 0.40f },
		new() { multiplierValue = 8f, hitsNeeded = 45, penaltyReduction = 0.60f },
		new() { multiplierValue = 16f, hitsNeeded = 90, penaltyReduction = 0.65f },
		new() { multiplierValue = 32f, hitsNeeded = 150, penaltyReduction = 0.70f },
	};

	[SerializeField] private float _comboTimeWindow = 2.5f;
	
	private int _currentCombo = 0;
	private float _lastHitTime;
	private int _currentTier = 0;
	public bool isOutOfHealth => _gameHP.value <= 0;
	public int score => _gameScore.value;

	private void OnEnable()
	{
		_gameHP.value = _defaultGameHP;
		_gameScore.value = _defaultGameScore;
		_gameMultiplier.value = _defaultGameMultiplier;
	}

	private void Start()
	{
		ResetMultiplier();
	}

	private void Update()
	{
		if (Time.time - _lastHitTime > _comboTimeWindow && _currentCombo > 0)
		{
			ResetCombo();
		}
	}

	public void HandleMoleEscapedDoDamage()
	{
		TakeDamage();
	}

	public void HandleMoleHit(MoleType moleType, int baseScore)
	{
		_lastHitTime = Time.time;

		switch (moleType)
		{
			case MoleType.Regular:
				HandleRegularMole(baseScore);
				break;
			case MoleType.Golden:
				HandleGoldenMole(baseScore);
				break;
			case MoleType.Health:
				HandleHealthMole(baseScore);
				break;
			case MoleType.NoHit:
				HandleNotHitMole();
				break;
		}
	}

	private void HandleRegularMole(int baseScore)
	{
		_currentCombo++;
		UpdateMultiplierTier();
		AddScore(baseScore);
	}

	private void HandleGoldenMole(int baseScore)
	{
		_currentCombo += 5;
		UpdateMultiplierTier();
		AddScore(baseScore * 2);
	}

	private void HandleHealthMole(int baseScore)
	{
		if (_gameHP.value < 5)
			_gameHP.ApplyChange(1);

		_currentCombo += 1;
		UpdateMultiplierTier();
		AddScore(baseScore);
	}

	private void HandleNotHitMole()
	{
		TakeDamage();
		
		for (int i = _multiplierTiers.Length - 1; i >= 0; i--)
		{
			if (_gameMultiplier.value >= _multiplierTiers[i].multiplierValue)
			{
				_currentTier = i;
				break;
			}
		}
	}

	private void UpdateMultiplierTier()
	{
		if (_currentTier < _multiplierTiers.Length - 1)
		{
			MultiplierTier current = _multiplierTiers[_currentTier];
			MultiplierTier next = _multiplierTiers[_currentTier + 1];

			if (_currentCombo >= next.hitsNeeded)
			{
				_currentTier++;
				_gameMultiplier.SetValue(next.multiplierValue);
			}
			else
			{
				// Calculate the proportion of progress within the current tier range
				float progress = (float)(_currentCombo - current.hitsNeeded) / (next.hitsNeeded - current.hitsNeeded);
				float interpolatedMultiplier = Mathf.Lerp(current.multiplierValue, next.multiplierValue, progress);

				interpolatedMultiplier = Mathf.Round(interpolatedMultiplier * 100f) / 100f;
				_gameMultiplier.SetValue(interpolatedMultiplier);
			}
		}
		else
		{
			_gameMultiplier.SetValue(_multiplierTiers[_currentTier].multiplierValue);
		}
	}

	private void TakeDamage()
	{
		float currentMultiplier = _gameMultiplier.value;
		float reduction = _multiplierTiers[_currentTier].penaltyReduction;
		
		_gameHP.ApplyChange(-1);
		_gameMultiplier.SetValue(Mathf.Max(1f, currentMultiplier * reduction));
		_currentCombo = 0;
	}

	private void AddScore(int baseScore)
	{
		int multipliedScore = Mathf.RoundToInt(baseScore * _gameMultiplier.value);
		_gameScore.ApplyChange(multipliedScore);
	}

	private void ResetCombo()
	{
		_currentCombo = 0;
		ResetMultiplier();
	}

	private void ResetMultiplier()
	{
		_currentTier = 0;
		_gameMultiplier.SetValue(_multiplierTiers[0].multiplierValue);
	}

	public void ResetMiniGameData()
	{
		_gameHP.SetValue(5);
		_gameScore.SetValue(0);
		ResetCombo();
	}
}