using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] musics;
    public Sound[] sounds;

    public AudioSource musicAudioSource;
    private Coroutine musicTransitionCoroutine;
    private Sound currentMusic;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (musicAudioSource == null)
        {
            musicAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource.playOnAwake = false;
        }
    }

    public Sound GetMusic(string name)
    {
        Sound m = System.Array.Find(musics, music => music.name == name);
        if (m != null)
        {
            return m;
        }
        else
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return null;
        }
    }

    public Sound GetSound(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null)
        {
            return s;
        }
        else
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return null;
        }
    }

    public void PlayMusic(Sound music, float fadeOutDuration = 1f, float fadeInDuration = 1f)
    {
        if (music == null)
        {
            Debug.LogWarning("AudioManager: PlayMusic called with null music.");
            return;
        }

        if (musicTransitionCoroutine != null)
        {
            StopCoroutine(musicTransitionCoroutine);
        }

        musicTransitionCoroutine = StartCoroutine(SwitchMusicCoroutine(music, fadeOutDuration, fadeInDuration));
    }

    public void StopMusic(float fadeOutDuration = 1f)
    {
        if (musicTransitionCoroutine != null)
        {
            StopCoroutine(musicTransitionCoroutine);
        }

        musicTransitionCoroutine = StartCoroutine(StopMusicCoroutine(fadeOutDuration));
    }


    private IEnumerator StopMusicCoroutine(float fadeOutDuration)
    {
        if (musicAudioSource == null || !musicAudioSource.isPlaying)
        {
            musicTransitionCoroutine = null;
            yield break;
        }

        float startVolume = musicAudioSource.volume;
        for (float t = 0f; t < fadeOutDuration; t += Time.deltaTime)
        {
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeOutDuration);
            yield return null;
        }

        musicAudioSource.volume = 0f;
        musicAudioSource.Stop();
        musicAudioSource.volume = startVolume;
        currentMusic = null;
        musicTransitionCoroutine = null;
    }

    private IEnumerator SwitchMusicCoroutine(Sound newMusic, float fadeOutDuration, float fadeInDuration)
    {
        if (musicAudioSource == null)
        {
            musicAudioSource = gameObject.AddComponent<AudioSource>();
            musicAudioSource.playOnAwake = false;
        }

        float previousVolume = musicAudioSource.volume;

        if (musicAudioSource.isPlaying)
        {
            for (float t = 0f; t < fadeOutDuration; t += Time.deltaTime)
            {
                musicAudioSource.volume = Mathf.Lerp(previousVolume, 0f, t / fadeOutDuration);
                yield return null;
            }
            musicAudioSource.volume = 0f;
            musicAudioSource.Stop();
        }

        musicAudioSource.clip = newMusic.clip;
        musicAudioSource.loop = newMusic.loop;
        musicAudioSource.outputAudioMixerGroup = newMusic.outputMixerGroup;
        musicAudioSource.pitch = newMusic.pitch;
        musicAudioSource.spatialBlend = newMusic.spatialBlend;
        float targetVolume = Mathf.Clamp01(newMusic.volume);

        musicAudioSource.volume = 0f;
        musicAudioSource.Play();

        for (float t = 0f; t < fadeInDuration; t += Time.deltaTime)
        {
            musicAudioSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeInDuration);
            yield return null;
        }

        musicAudioSource.volume = targetVolume;
        currentMusic = newMusic;
        musicTransitionCoroutine = null;
    }
}
