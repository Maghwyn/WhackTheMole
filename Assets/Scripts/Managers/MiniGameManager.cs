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
		_miniGameUIManager.HideUI();
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
		_hammerEvent.OnHammerHandGrab += InitMiniGame;
		_miniGameUIManager.ShowStartingMessage();
	}

	private void PostEndGame()
	{
		// Make sure everything is reset even if it's unnecessary as it's "safe" anyway.

		_hammerEvent.OnHammerHandGrab -= InitMiniGame;
		_hammerEvent.OnHammerHandGrab -= ResumeMiniGame;
		_hammerEvent.OnHammerHandDrop -= PauseMiniGame;
		_miniGameUIManager.OnStartNewGameComplete -= RunMiniGame;
		_miniGameUIManager.OnResumeGameComplete -= RunMiniGame;

		_spawnerManager.EndTask();
		_spawnerManager.enabled = false;

		_miniGameUIManager.ForceStopResumeCountdownIfRunning();
		_miniGameUIManager.ForceStopNewGameCountdownIfRunning();

		_miniGameUIManager.HideUI();
		_hammerReturn.ForceReturnToSocket();

		_isGamePaused = false;
		_miniGameDataManager.ResetMiniGameData();
	}

	private void Update()
	{
		if (_miniGameDataManager.isOutOfHealth)
		{
			_hammerEvent.OnHammerHandGrab -= ResumeMiniGame;
			_hammerEvent.OnHammerHandDrop -= PauseMiniGame;

			_spawnerManager.EndTask();
			_spawnerManager.enabled = false;

			_miniGameUIManager.UpdateFinalScoreText(_miniGameDataManager.score);
			_miniGameUIManager.ShowRestartUI();
			_highScoreManager.AddNewScore(_miniGameDataManager.score);
		}
	}

	private void InitMiniGame()
	{
		_hammerEvent.OnHammerHandGrab -= InitMiniGame;
		_hammerEvent.OnHammerHandGrab += ResumeMiniGame;
		_hammerEvent.OnHammerHandDrop += PauseMiniGame;

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

		if (_hammerEvent.isGrabbedByHand)
		{
			InitMiniGame();
		}
		else
		{
			_hammerEvent.OnHammerHandGrab += InitMiniGame;
			_miniGameUIManager.ShowStartingMessage();
		}
	}
}