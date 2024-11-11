using UnityEngine;

[CreateAssetMenu(menuName = "Audio SFX/SoundFXManager", fileName = "SoundFXManager")]
public class SoundFXManagerSO : ScriptableObject 
{
	private static SoundFXManagerSO instance;
	public static SoundFXManagerSO Instance {
		get {
			if (instance == null) {
				instance = Resources.Load<SoundFXManagerSO>("SoundFXManager");
			}
			return instance;
		}
	}

	[SerializeField] private AudioSource SoundFXObject;

	public static void PlaySoundFXClip(AudioClip clip, Vector3 soundPosition, float volume) {
		AudioSource audioSource = Instantiate(Instance.SoundFXObject, soundPosition, Quaternion.identity);
		audioSource.clip = clip;
		audioSource.volume = volume;
		audioSource.Play();
	}

	public static void PlaySoundFXClip(AudioClip[] clips, Vector3 soundPosition, float volume) {
		int randClip = Random.Range(0, clips.Length);
		AudioSource audioSource = Instantiate(Instance.SoundFXObject, soundPosition, Quaternion.identity);
		audioSource.clip = clips[randClip];
		audioSource.volume = volume;
		audioSource.Play();
	}
}