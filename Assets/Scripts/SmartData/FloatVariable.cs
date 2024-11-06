using UnityEngine;

[CreateAssetMenu(menuName = "Smart Data/Float Variable", fileName = "Float Variable")]
public class FloatVariable : ScriptableObject 
{
	public float value;

	public void SetValue(float _value)
	{
		value = _value;
	}

	public void ApplyChange(float _changeAmount)
	{
		value += _changeAmount;
	}
}