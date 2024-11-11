using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[Header("Health UI")]
	[SerializeField] private Image _healthBarFill;

	[Header("Health Data")]
	[SerializeField] private FloatVariable _gameHP;

	private void Update()
	{
		float value = _gameHP.value / 5;
		if (_healthBarFill.fillAmount == value) return;
		Debug.Log(value);
		Debug.Log(_gameHP.value);

		_healthBarFill.fillAmount = value;
	}
}