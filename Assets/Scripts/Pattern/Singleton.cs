using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
	private static T _instance;
	private static bool _isQuitting;
	
	public static T Instance
	{
		get
		{
			if (_instance == null && !_isQuitting)
			{
				_instance = FindObjectOfType<T>();

				if (_instance == null)
				{
					GameObject go = new() { name = typeof(T).Name };
					_instance = go.AddComponent<T>();
				}
			}
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if (_instance == null)
		{
			_instance = this as T;
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}

	protected virtual void OnApplicationQuit()
	{
		_isQuitting = true;
	}
}