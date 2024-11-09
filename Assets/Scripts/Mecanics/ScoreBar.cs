using TMPro;
using UnityEngine;
using System.Collections;

public class ScoreBar : MonoBehaviour
{
	[Header("Score UI")]
	[SerializeField] private TextMeshProUGUI _scoreText;

	[Header("Score Data")]
	[SerializeField] private FloatVariable _gameScore;

	[Header("Score Animation")]
	[SerializeField] private float _scoreIncrementDuration = 1f;
	[SerializeField] private AnimationCurve _scoreIncrementCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[Header("Text Scale Animation")]
	[SerializeField] private float _maxScaleMultiplier = 1.5f;
	[SerializeField] private float _scaleDuration = 0.3f;
	[SerializeField] private AnimationCurve _scaleAnimationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	private float _displayedScore;
	private float _targetScore;
	private Vector3 _originalScale;
	private Coroutine _scoreCoroutine;
	private Coroutine _scaleCoroutine;

	private void Start()
	{
		_originalScale = _scoreText.transform.localScale;
		_displayedScore = _gameScore.value;
		_targetScore = _gameScore.value;
		UpdateScoreText();
	}

	private void Update()
	{
		if (_gameScore.value != _targetScore)
		{
			StartScoreAnimation(_gameScore.value);
		}
	}

	private void StartScoreAnimation(float newTarget)
	{
		_targetScore = newTarget;

		if (_scoreCoroutine != null)
			StopCoroutine(_scoreCoroutine);
		
		_scoreCoroutine = StartCoroutine(AnimateScore());

		if (_scaleCoroutine != null)
			StopCoroutine(_scaleCoroutine);
			
		_scaleCoroutine = StartCoroutine(AnimateScale());
	}

	private IEnumerator AnimateScore()
	{
		float startScore = _displayedScore;
		float elapsedTime = 0f;

		while (elapsedTime < _scoreIncrementDuration)
		{
			elapsedTime += Time.deltaTime;
			float progress = _scoreIncrementCurve.Evaluate(elapsedTime / _scoreIncrementDuration);
			
			_displayedScore = Mathf.Lerp(startScore, _targetScore, progress);
			UpdateScoreText();
			
			yield return null;
		}

		_displayedScore = _targetScore;
		UpdateScoreText();
	}

	private IEnumerator AnimateScale()
	{
		float elapsedTime = 0f;
		Vector3 maxScale = _originalScale * _maxScaleMultiplier;

		while (elapsedTime < _scaleDuration)
		{
			elapsedTime += Time.deltaTime;
			float progress = _scaleAnimationCurve.Evaluate(elapsedTime / _scaleDuration);
			_scoreText.transform.localScale = Vector3.Lerp(_originalScale, maxScale, progress);
			yield return null;
		}

		elapsedTime = 0f;
		while (elapsedTime < _scaleDuration)
		{
			elapsedTime += Time.deltaTime;
			float progress = _scaleAnimationCurve.Evaluate(elapsedTime / _scaleDuration);
			_scoreText.transform.localScale = Vector3.Lerp(maxScale, _originalScale, progress);
			yield return null;
		}

		_scoreText.transform.localScale = _originalScale;
	}

	private void UpdateScoreText()
	{
		_scoreText.text = Mathf.RoundToInt(_displayedScore).ToString();
	}
}