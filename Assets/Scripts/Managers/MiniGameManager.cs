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

	private MiniGameDataManager _miniGameDataManager;
	private bool _isGamePaused = false;

	private void Awake()
	{
		_miniGameUIManager = gameObject.GetComponentInChildren<MiniGameUIManager>();
		_miniGameUIManager.HideAllUI();
		_spawnerManager.enabled = false;

		_teleportEvent.OnAnchorEnter += PreInitGame;
		_teleportEvent.OnAnchorExit += PostEndGame;
	}

	private void Start()
	{
		_miniGameDataManager = FindObjectOfType<MiniGameDataManager>();
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
		_miniGameDataManager.ResetMiniGameData();
	}

	private void Update()
	{
		if (_miniGameDataManager.isOutOfHealth)
		{
			_hammerEvent.OnHammerGrab -= ResumeMiniGame;
			_hammerEvent.OnHammerDrop -= PauseMiniGame;

			_spawnerManager.EndTask();
			_spawnerManager.enabled = false;

			_miniGameUIManager.ShowRestartUI();
			_highScoreManager.AddNewScore(_miniGameDataManager.score);
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

	public void OnRestartMiniGame()
	{
		_miniGameDataManager.ResetMiniGameData();
		_miniGameUIManager.HideRestartUI();

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
}