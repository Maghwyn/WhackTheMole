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
	private Color _colorGreen = new(118/255, 214/255, 126/255, 235/255);
	private Color _colorRed = new(214/255, 118/255, 124/255, 235/255);
	private Color _colorOrange = new(214/255, 178/255, 118/255, 235/255);

	public void HideUI()
	{
		ToggleStartingMessage(false);
		ToggleGameMessage(false);
		ToggleRestartUI(false);
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
	}

	public void ShowRestartUI()
	{
		ToggleRestartUI(true);
	}

	public void HideRestartUI()
	{
		ToggleRestartUI(false);
	}

	public void UpdateFinalScoreText(int score)
	{
		string point = score > 0 ? "points" : "point";
		_scorePointText.text = $"{score} {point}!";
	}

	public void StartNewGameCountdown()
	{
		ToggleGameMessage(true);
		_newGameCountdownCoroutine = StartCoroutine(StartingNewGameCoroutine());
	}

	public void StartResumeCountdown()
	{
		ToggleGameMessage(true);
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
}