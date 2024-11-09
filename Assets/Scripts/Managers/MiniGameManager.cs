using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
	[Header("Managers")]
	[SerializeField] private MiniGameUIManager _miniGameUIManager;
	[SerializeField] private SpawnerManager _spawnerManager;
	[SerializeField] private HighScoreManager _highScoreManager;

	[Header("Mechanic")]
	[SerializeField] private HammerReturn _hammerReturn;

	[Header("Events")]
	[SerializeField] private HammerEvent _hammerEvent;
	[SerializeField] private TeleportEvent _teleportEvent;

	[Header("Game Data")]
	[SerializeField] private FloatVariable _gameHP;
	[SerializeField] private FloatVariable _gameScore;
	[SerializeField] private FloatVariable _gameMultiplier;

	private bool _isGamePaused = false;

	private void Awake()
	{
		_miniGameUIManager = gameObject.GetComponentInChildren<MiniGameUIManager>();
		_miniGameUIManager.HideAllUI();
		_spawnerManager.enabled = false;

		_teleportEvent.OnAnchorEnter += PreInitGame;
		_teleportEvent.OnAnchorExit += PostEndGame;
	}

	private void PreInitGame()
	{
		_hammerEvent.OnHammerGrab += InitMiniGame;
		_miniGameUIManager.ShowStartingMessage();
	}

	private void PostEndGame()
	{
		// Make sure everything is reset even if it's unnecessary as it's "safe" anyway.

		_hammerEvent.OnHammerGrab -= InitMiniGame;
		_hammerEvent.OnHammerGrab -= ResumeMiniGame;
		_hammerEvent.OnHammerGrab -= PauseMiniGame;
		_miniGameUIManager.OnStartNewGameComplete -= RunMiniGame;
		_miniGameUIManager.OnResumeGameComplete -= RunMiniGame;

		_spawnerManager.EndTask();
		_spawnerManager.enabled = false;

		_miniGameUIManager.ForceStopResumeCountdownIfRunning();
		_miniGameUIManager.ForceStopNewGameCountdownIfRunning();

		_miniGameUIManager.HideAllUI();
		_hammerReturn.ForceReturnToSocket();

		_isGamePaused = false;
		ResetMiniGameData();
	}

	private void Update()
	{
		if (_gameHP.value <= 0)
		{
			_hammerEvent.OnHammerGrab -= ResumeMiniGame;
			_hammerEvent.OnHammerDrop -= PauseMiniGame;

			_spawnerManager.EndTask();
			_spawnerManager.enabled = false;

			_miniGameUIManager.ShowRestartUI();
			_highScoreManager.AddNewScore(_gameScore.value);
		}
	}

	public void OnRestartMiniGame()
	{
		ResetMiniGameData();

		if (_hammerEvent.isGrabbed)
		{
			InitMiniGame();
		}
		else
		{
			_hammerEvent.OnHammerGrab += InitMiniGame;
			_miniGameUIManager.ShowStartingMessage();
		}
	}

	private void InitMiniGame()
	{
		_hammerEvent.OnHammerGrab -= InitMiniGame;
		_hammerEvent.OnHammerGrab += ResumeMiniGame;
		_hammerEvent.OnHammerDrop += PauseMiniGame;

		_miniGameUIManager.OnStartNewGameComplete += RunMiniGame;
		_miniGameUIManager.HideStartingMessage();
		_miniGameUIManager.StartNewGameCountdown();
	}

	private void RunMiniGame()
	{
		if (_isGamePaused)
		{
			_miniGameUIManager.OnResumeGameComplete -= RunMiniGame;
		}

		_isGamePaused = false;
		_spawnerManager.enabled = true;

		_miniGameUIManager.HideStartingMessage();
	}

	private void ResumeMiniGame()
	{
		_miniGameUIManager.OnResumeGameComplete += RunMiniGame;
		_miniGameUIManager.StartResumeCountdown();
	}

	private void PauseMiniGame()
	{
		if (_isGamePaused)
		{
			_miniGameUIManager.ForceStopResumeCountdownIfRunning();
			_miniGameUIManager.ShowPauseMessage();
			return;
		}

		_isGamePaused = true;
		_spawnerManager.enabled = false;

		_miniGameUIManager.ForceStopNewGameCountdownIfRunning();
		_miniGameUIManager.ShowPauseMessage();
	}

	private void ResetMiniGameData()
	{
		_gameHP.value = 5;
		_gameMultiplier.value = 1;
		_gameScore.value = 0;
	}
}