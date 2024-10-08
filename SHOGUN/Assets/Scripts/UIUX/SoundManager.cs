using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;

    // Volume and pitch settings
    [Range(0f, 10f)]  // You can increase this further, allowing up to 10x louder sound
    public float soundVolume = 1.0f;
    [Range(0.5f, 3f)]
    public float soundPitch = 1.0f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            // Create a temporary audio source to play the sound with custom pitch and volume
            GameObject tempAudioSource = new GameObject("TempAudioSource");
            tempAudioSource.transform.position = pos;
            AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();

            audioSource.clip = clip;
            audioSource.volume = Mathf.Clamp(soundVolume, 0f, 10f);  // Boost volume up to 10x
            audioSource.pitch = soundPitch;
            audioSource.spatialBlend = 1f;  // Make it 3D sound
            audioSource.Play();

            Destroy(tempAudioSource, clip.length / audioSource.pitch);  // Destroy after playing
        }
    }

    public void PlaySound3D(string soundName, Vector3 pos)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }
}
