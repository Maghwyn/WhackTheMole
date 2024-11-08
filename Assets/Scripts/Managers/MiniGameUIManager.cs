using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MiniGameUIManager : MonoBehaviour
{
	[SerializeField] private GameObject _startingMessageUI;
	[SerializeField] private GameObject _gameMessageUI;
	[SerializeField] private GameObject _lifeUI;
	[SerializeField] private GameObject _scoreUI;
	[SerializeField] private GameObject _multiplierUI;
	[SerializeField] private GameObject _restartUI;
	[SerializeField] private TextMeshProUGUI _floatingText;

	public event Action OnStartNewGameComplete;
	public event Action OnResumeGameComplete;
	private Coroutine _newGameCountdownCoroutine;
	private Coroutine _resumeCountdownCoroutine;

	public void HideAllUI()
	{
		ToggleStartingMessage(false);
		ToggleGameMessage(false);
		ToggleMiniGameLife(false);
		ToggleMiniGameScore(false);
		ToggleMiniGameMultiplier(false);
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
		_floatingText.text = "Paused";
		// TODO: Set background to RED
	}

	public void ShowRestartUI()
	{
		ToggleRestartUI(true);
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
		yield return new WaitForSeconds(0.5f);

		// TODO: Set background to GREEN

		for (int i = 3; i > 0; i--)
		{
			_floatingText.text = $"Starting in... {i}";
			yield return new WaitForSeconds(1f);
		}

		_floatingText.text = "GO!";
		yield return new WaitForSeconds(1f);

		ToggleGameMessage(false);
		ShowGameUI();

		OnStartNewGameComplete.Invoke();
		_newGameCountdownCoroutine = null;
	}

	private IEnumerator ResumingGameCoroutine()
	{
		yield return new WaitForSeconds(0.5f);

		// TODO: Set background to ORANGE

		for (int i = 3; i > 0; i--)
		{
			_floatingText.text = $"Resuming in... {i}";
			yield return new WaitForSeconds(1f);
		}

		_floatingText.text = "GO!";
		yield return new WaitForSeconds(1f);

		ToggleGameMessage(false);
		ShowGameUI();

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