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
	[SerializeField] private float _defaultGameHP = 5f;
	[SerializeField] private int _defaultGameScore = 0;
	[SerializeField] private float _defaultGameMultiplier = 1;
	[SerializeField] private int _defaultGameCombo = 0;

	[Header("Game Data")]
	[SerializeField] private FloatVariable _gameHP;
	[SerializeField] private IntVariable _gameScore;
	[SerializeField] private FloatVariable _gameMultiplier;
	[SerializeField] private IntVariable _gameCombo;

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

	[SerializeField] private float _comboTimeWindow = 7.5f;

	[Header("Sounds")]
	[SerializeField] private AudioClip _bonkHitClip;
	[SerializeField] private AudioClip _loseHpClip;

	private float _lastHitTime;
	private int _currentTier = 0;
	private float _pausedTimeDifference = 0f;
	public bool isOutOfHealth => _gameHP.value <= 0f;
	public int score => _gameScore.value;
	public bool isPaused
	{
		get => _isPaused;
		set
		{
			if (value != _isPaused)
			{
				_isPaused = value;
				if (_isPaused)
				{
					_pausedTimeDifference = Time.time - _lastHitTime;
				}
				else
				{
					_lastHitTime = Time.time + _pausedTimeDifference;
				}
			}
		}
	}
	private bool _isPaused = false;

	private void OnEnable()
	{
		_gameHP.value = _defaultGameHP;
		_gameScore.value = _defaultGameScore;
		_gameMultiplier.value = _defaultGameMultiplier;
		_gameCombo.value = _defaultGameCombo;
	}

	private void Start()
	{
		ResetMultiplier();
	}

	private void Update()
	{
		if (!isPaused && Time.time - _lastHitTime > _comboTimeWindow && _gameCombo.value > 0)
		{
			ResetCombo();
		}
	}

	public void HandleMoleEscapedDoDamage()
	{
		SoundFXManagerSO.PlaySoundFXClip(_loseHpClip, transform.position, 0.5f);
		TakeDamage();
	}

	public void HandleMoleHit(MoleType moleType, int baseScore)
	{
		_lastHitTime = Time.time;

		switch (moleType)
		{
			case MoleType.Regular:
				SoundFXManagerSO.PlaySoundFXClip(_bonkHitClip, transform.position, 0.5f);
				HandleRegularMole(baseScore);
				break;
			case MoleType.Golden:
				SoundFXManagerSO.PlaySoundFXClip(_bonkHitClip, transform.position, 0.5f);
				HandleGoldenMole(baseScore);
				break;
			case MoleType.Health:
				SoundFXManagerSO.PlaySoundFXClip(_bonkHitClip, transform.position, 0.5f);
				HandleHealthMole(baseScore);
				break;
			case MoleType.NoHit:
				SoundFXManagerSO.PlaySoundFXClip(_loseHpClip, transform.position, 0.5f);
				HandleNotHitMole();
				break;
		}
	}

	private void HandleRegularMole(int baseScore)
	{
		_gameCombo.ApplyChange(1);
		UpdateMultiplierTier();
		AddScore(baseScore);
	}

	private void HandleGoldenMole(int baseScore)
	{
		_gameCombo.ApplyChange(5);
		UpdateMultiplierTier();
		AddScore(baseScore * 2);
	}

	private void HandleHealthMole(int baseScore)
	{
		if (_gameHP.value < 5f)
			_gameHP.ApplyChange(1f);

		_gameCombo.ApplyChange(1);
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

			if (_gameCombo.value >= next.hitsNeeded)
			{
				_currentTier++;
				_gameMultiplier.SetValue(next.multiplierValue);
			}
			else
			{
				// Calculate the proportion of progress within the current tier range
				float progress = (float)(_gameCombo.value - current.hitsNeeded) / (next.hitsNeeded - current.hitsNeeded);
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
		
		_gameHP.ApplyChange(-1f);
		_gameMultiplier.SetValue(Mathf.Max(1f, currentMultiplier * reduction));
		_gameCombo.SetValue(0);
	}

	private void AddScore(int baseScore)
	{
		int multipliedScore = Mathf.RoundToInt(baseScore * _gameMultiplier.value);
		_gameScore.ApplyChange(multipliedScore);
	}

	private void ResetCombo()
	{
		_gameCombo.SetValue(0);
		ResetMultiplier();
	}

	private void ResetMultiplier()
	{
		_currentTier = 0;
		_gameMultiplier.SetValue(_multiplierTiers[0].multiplierValue);
	}

	public void ResetMiniGameData()
	{
		_gameHP.SetValue(5f);
		_gameScore.SetValue(0);
		ResetCombo();
	}
}