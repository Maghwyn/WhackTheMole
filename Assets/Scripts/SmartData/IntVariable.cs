using UnityEngine;

[CreateAssetMenu(menuName = "Smart Data/Int Variable", fileName = "Int Variable")]
public class IntVariable : ScriptableObject 
{
	public int value;

	public void SetValue(int _value)
	{
		value = _value;
	}

	public void ApplyChange(int _changeAmount)
	{
		value += _changeAmount;
	}
}