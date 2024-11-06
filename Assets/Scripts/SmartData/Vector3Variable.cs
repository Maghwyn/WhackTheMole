using UnityEngine;

[CreateAssetMenu(menuName = "Smart Data/Vector3 Variable", fileName = "Vector3 Variable")]
public class Vector3Variable : ScriptableObject 
{
	public Vector3 value;
	private Vector3 _default = new(0,0,0);

	public void SetValue(Vector3 _value)
	{
		value = _value;
	}

	public void ApplyChange(Vector3 _changeAmount)
	{
		value += _changeAmount;
	}

	public Vector3 GetDefault()
	{
		return _default;
	}
}