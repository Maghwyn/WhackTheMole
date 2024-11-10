using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[Header("Health UI")]
	[SerializeField] private Image _healthBarFill;

	[Header("Health Data")]
	[SerializeField] private IntVariable _gameHP;

	private void Update()
	{
		float value = _gameHP.value / 5;
		if (_healthBarFill.fillAmount == value) return;

		_healthBarFill.fillAmount = value;
	}
}