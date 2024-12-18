using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameUIManager : MonoBehaviour
{
	[Header("Event UI")]
	[SerializeField] private GameObject _startingMessageUI;
	[SerializeField] private GameObject _restartUI;
	[SerializeField] private GameObject _respawnHammerUI;

	[Header("Message UI")]
	[SerializeField] private GameObject _gameMessageUI;
	[SerializeField] private Image _gameMessageBackground;
	[SerializeField] private TextMeshProUGUI _gameMessageText;
	[SerializeField] private TextMeshProUGUI _scorePointText;

	[Header("Stats UI")]
	[SerializeField] private GameObject _lifeUI;
	[SerializeField] private GameObject _scoreUI;
	[SerializeField] private GameObject _multiplierUI;

	public event Action OnStartNewGameComplete;
	public event Action OnResumeGameComplete;
	private Coroutine _newGameCountdownCoroutine;
	private Coroutine _resumeCountdownCoroutine;
	private Color _colorGreen = new(118f/255, 214f/255, 126f/255, 235f/255);
	private Color _colorRed = new(214f/255, 118f/255, 124f/255, 235f/255);
	private Color _colorOrange = new(214f/255, 178f/255, 118f/255, 235f/255);

	public void HideUI()
	{
		ToggleStartingMessage(false);
		ToggleGameMessage(false);
		ToggleRestartUI(false);
		ToggleRespawnHammerUI(false);
		ShowGameUI();
	}

	public void ShowGameUI()
	{
		ToggleMiniGameLife(true);
		ToggleMiniGameScore(true);
		ToggleMiniGameMultiplier(true);
	}

	public void ShowStartingMessage()
	{
		ToggleStartingMessage(true);
	}

	public void HideStartingMessage()
	{
		ToggleStartingMessage(false);
	}

	public void ShowPauseMessage()
	{
		ToggleGameMessage(true);
		_gameMessageText.text = "Paused";
		_gameMessageBackground.color = _colorRed;

		ToggleRespawnHammerUI(true);
	}

	public void ShowRestartUI()
	{
		ToggleRestartUI(true);
	}

	public void HideRestartUI()
	{
		ToggleRestartUI(false);
	}

	public void HideRespawnHammerUI()
	{
		ToggleRespawnHammerUI(false);
	}

	public void UpdateFinalScoreText(int score)
	{
		string point = score > 0 ? "points" : "point";
		_scorePointText.text = $"{score} {point}!";
	}

	public void StartNewGameCountdown()
	{
		ToggleGameMessage(true);
		HideRespawnHammerUI();
		_newGameCountdownCoroutine = StartCoroutine(StartingNewGameCoroutine());
	}

	public void StartResumeCountdown()
	{
		ToggleGameMessage(true);
		HideRespawnHammerUI();
		_resumeCountdownCoroutine = StartCoroutine(ResumingGameCoroutine());
	}

	public void ForceStopNewGameCountdownIfRunning()
	{
		if (_newGameCountdownCoroutine != null)
		{
			StopCoroutine(_newGameCountdownCoroutine);
			_newGameCountdownCoroutine = null;
		}
	}

	public void ForceStopResumeCountdownIfRunning()
	{
		if (_resumeCountdownCoroutine != null)
		{
			StopCoroutine(_resumeCountdownCoroutine);
			_resumeCountdownCoroutine = null;
		}
	}

	private IEnumerator StartingNewGameCoroutine()
	{
		_gameMessageBackground.color = _colorGreen;

		for (int i = 3; i > 0; i--)
		{
			_gameMessageText.text = $"Starting in... {i}";
			yield return new WaitForSeconds(1f);
		}

		_gameMessageText.text = "GO!";
		yield return new WaitForSeconds(1f);

		ToggleGameMessage(false);

		OnStartNewGameComplete.Invoke();
		_newGameCountdownCoroutine = null;
	}

	private IEnumerator ResumingGameCoroutine()
	{
		_gameMessageBackground.color = _colorOrange;

		for (int i = 3; i > 0; i--)
		{
			_gameMessageText.text = $"Resuming in... {i}";
			yield return new WaitForSeconds(1f);
		}

		_gameMessageText.text = "GO!";
		yield return new WaitForSeconds(1f);

		ToggleGameMessage(false);

		OnResumeGameComplete.Invoke();
		_resumeCountdownCoroutine = null;
	}

	private void ToggleStartingMessage(bool value)
	{
		_startingMessageUI.SetActive(value);
	}

	private void ToggleGameMessage(bool value)
	{
		_gameMessageUI.SetActive(value);
	}

	private void ToggleMiniGameLife(bool value)
	{
		_lifeUI.SetActive(value);
	}

	private void ToggleMiniGameScore(bool value)
	{
		_scoreUI.SetActive(value);
	}

	private void ToggleMiniGameMultiplier(bool value)
	{
		_multiplierUI.SetActive(value);
	}

	private void ToggleRestartUI(bool value)
	{
		_restartUI.SetActive(value);
	}

	private void ToggleRespawnHammerUI(bool value)
	{
		_respawnHammerUI.SetActive(value);
	}
}