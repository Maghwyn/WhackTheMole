using TMPro;
using UnityEngine;
using System.Collections;

public class MultiplierBar : MonoBehaviour
{
	[Header("Multiplier UI")]
	[SerializeField] private TextMeshProUGUI _multiplierText;
	[SerializeField] private TextMeshProUGUI _comboText;

	[Header("Multiplier Data")]
	[SerializeField] private FloatVariable _gameMultiplier;
	[SerializeField] private IntVariable _gameCombo;

	[Header("Text Scale Animation")]
	[SerializeField] private float _maxScaleMultiplier = 1.5f;
	[SerializeField] private float _scaleDuration = 0.3f;
	[SerializeField] private AnimationCurve _scaleAnimationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	private Vector3 _originalScale;
	private Coroutine _scaleCoroutine;
	private float _lastMultiplier;
	private int _lastCombo;

	private void Start()
	{
		_originalScale = _multiplierText.transform.localScale;
		_lastMultiplier = _gameMultiplier.value;
		_lastCombo = _gameCombo.value;
		UpdateMultiplierText();
	}

	private void Update()
	{
		if (_gameMultiplier.value != _lastMultiplier)
		{
			_lastMultiplier = _gameMultiplier.value;
			UpdateMultiplierText();
			StartScaleAnimation();
		}

		if (_gameCombo.value != _lastCombo)
		{
			_lastCombo = _gameCombo.value;
			UpdateComboText();
		}
	}

	private void StartScaleAnimation()
	{
		if (_scaleCoroutine != null)
			StopCoroutine(_scaleCoroutine);
			
		_scaleCoroutine = StartCoroutine(AnimateScale());
	}

	private IEnumerator AnimateScale()
	{
		float elapsedTime = 0f;
		Vector3 maxScale = _originalScale * _maxScaleMultiplier;

		while (elapsedTime < _scaleDuration)
		{
			elapsedTime += Time.deltaTime;
			float progress = _scaleAnimationCurve.Evaluate(elapsedTime / _scaleDuration);
			_multiplierText.transform.localScale = Vector3.Lerp(_originalScale, maxScale, progress);
			yield return null;
		}

		elapsedTime = 0f;
		while (elapsedTime < _scaleDuration)
		{
			elapsedTime += Time.deltaTime;
			float progress = _scaleAnimationCurve.Evaluate(elapsedTime / _scaleDuration);
			_multiplierText.transform.localScale = Vector3.Lerp(maxScale, _originalScale, progress);
			yield return null;
		}

		_multiplierText.transform.localScale = _originalScale;
	}

	private void UpdateMultiplierText()
	{
		_multiplierText.text = $"x{_gameMultiplier.value}";
	}

	private void UpdateComboText()
	{
		_comboText.text = $"{_gameCombo.value}";
	}
}