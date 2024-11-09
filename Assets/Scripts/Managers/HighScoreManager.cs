using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class HighScoreList
{
	public List<float> scores = new();
}

public class HighScoreManager : MonoBehaviour
{
	[SerializeField] private string _saveKey = "MoleMiniGameScores";
	[SerializeField] private GameObject _scorePrefab;
	[SerializeField] private RectTransform _topScoreBox;
	[SerializeField] private RectTransform _otherScoreBox;
	[SerializeField] private Color[] _medalColors = new Color[3];

	private HighScoreList _scoreList = new();
	private const int _MaxScores = 15;

	private void Start()
	{
		LoadScores();
		ClearScoreBoards();
		PopulateScoreBoard();
	}

	public void LoadScores()
	{
		string jsonData = PlayerPrefs.GetString(_saveKey, "");
		if (!string.IsNullOrEmpty(jsonData))
		{
			_scoreList = JsonUtility.FromJson<HighScoreList>(jsonData);
		}
	}

	public void SaveScores()
	{
		string jsonData = JsonUtility.ToJson(_scoreList);
		PlayerPrefs.SetString(_saveKey, jsonData);
		PlayerPrefs.Save();
	}

	public void AddNewScore(float score)
	{
		// Should allow duplicated score cuz why not
		_scoreList.scores.Add(score);

		_scoreList.scores = _scoreList.scores
			.OrderByDescending(s => s)
			.Take(_MaxScores)
			.ToList();

		SaveScores();
		ClearScoreBoards();
		PopulateScoreBoard();
	}

	private void ClearScoreBoards()
	{

		while (_topScoreBox.childCount > 0)
		{
			#if UNITY_EDITOR
				DestroyImmediate(_topScoreBox.GetChild(0).gameObject);
			#else
				Destroy(_topScoreBox.GetChild(0).gameObject);
			#endif
		}

		while (_otherScoreBox.childCount > 0)
		{
			#if UNITY_EDITOR
				DestroyImmediate(_otherScoreBox.GetChild(0).gameObject);
			#else
				Destroy(_otherScoreBox.GetChild(0).gameObject);
			#endif
		}
	}

	private void PopulateScoreBoard()
	{
		for (int i = 0; i < _scoreList.scores.Count; i++)
		{
			Transform targetParent = i < 3 ? _topScoreBox : _otherScoreBox;
			GameObject scoreEntry = Instantiate(_scorePrefab, targetParent);
			
			TextMeshProUGUI scoreText = scoreEntry.GetComponentInChildren<TextMeshProUGUI>();
			scoreText.text = $"#{i + 1}: {_scoreList.scores[i]:F2}";

			Image background = scoreEntry.GetComponentInChildren<Image>();
			if (background != null)
			{
				background.color = i < 3 ? _medalColors[i] : Color.white;
			}
		}
	}

	#if UNITY_EDITOR
	public void PopulateTestScores()
	{
		ClearAllScores();

		HashSet<float> uniqueScores = new();
		while (uniqueScores.Count < _MaxScores)
		{
			float randomScore = Mathf.Round(Random.Range(100f, 1000f));
			if (uniqueScores.Add(randomScore))
			{
				AddNewScore(randomScore);
			}
		}
	}

	public void AddRandomScore()
	{
		float randomScore = Mathf.Round(Random.Range(100f, 1000f));
		AddNewScore(randomScore);
	}

	public void ClearAllScores()
	{
		_scoreList.scores.Clear();
		PlayerPrefs.DeleteKey(_saveKey);
		ClearScoreBoards();
	}

	[UnityEditor.MenuItem("Tools/Highscore/Populate Test Scores")]
	private static void EditorPopulateTestScores()
	{
		HighScoreManager manager = FindObjectOfType<HighScoreManager>();
		if (manager != null)
			manager.PopulateTestScores();
	}

	[UnityEditor.MenuItem("Tools/Highscore/Add Random Score")]
	private static void EditorAddRandomScore()
	{
		HighScoreManager manager = FindObjectOfType<HighScoreManager>();
		if (manager != null)
			manager.AddRandomScore();
	}

	[UnityEditor.MenuItem("Tools/Highscore/Clear All Scores")]
	private static void EditorClearAllScores()
	{
		HighScoreManager manager = FindObjectOfType<HighScoreManager>();
		if (manager != null)
			manager.ClearAllScores();
	}
	#endif
}