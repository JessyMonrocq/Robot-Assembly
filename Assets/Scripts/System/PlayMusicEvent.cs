using UnityEngine;

public class PlayMusicEvent : MonoBehaviour
{
    [Tooltip("Select the name of the music to play (popup in inspector).")]
    public string musicName = "MusicTrack";

    private Sound music;

    private void Awake()
    {
        AudioManager audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }

        if (audioManager != null)
        {
            if (!string.IsNullOrEmpty(musicName))
            {
                music = audioManager.GetMusic(musicName);
            }
            else
            {
                Debug.LogWarning("PlayMusicEvent: musicName is empty");
            }
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found!");
        }
    }

    public void PlayMusic(float fadeOutDuration = 0.5f, float fadeInDuration = 0.5f)
    {
        if (AudioManager.instance == null)
        {
            Debug.LogWarning("PlayMusicEvent: AudioManager.instance is null");
            return;
        }

        if (music == null)
        {
            music = AudioManager.instance.GetMusic(musicName);
        }

        AudioManager.instance.PlayMusic(music, fadeOutDuration, fadeInDuration);
    }
}
