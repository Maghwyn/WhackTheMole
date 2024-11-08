using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
	[Header("Managers")]
	[SerializeField] private MiniGameUIManager _miniGameUIManager;
	[SerializeField] private SpawnerManager _spawnerManager;

	[Header("Hammer Mechanics")]
	[SerializeField] private HammerEvent _hammerEvent;
	[SerializeField] private HammerReturn _hammerReturn;

	[Header("SO Float References")]
	[SerializeField] private FloatVariable _gameHP;

	private bool _isGamePaused = false;

	private void Awake()
	{
		_miniGameUIManager = gameObject.GetComponentInChildren<MiniGameUIManager>();
		_miniGameUIManager.HideAllUI();
		_spawnerManager.enabled = false;
	}

	private void OnEnable()
	{
		_hammerEvent.OnHammerGrab += InitMiniGame;
		_miniGameUIManager.ShowStartingMessage();
	}

	private void OnDisable()
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
			// TODO: Save to Leaderboard
		}
	}

	public void OnRestartMiniGame()
	{
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
}