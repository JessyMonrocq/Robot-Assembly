using System.Collections;
using UnityEngine;

public class PlaySoundEvent : MonoBehaviour
{
    private AudioSource audioSource;

    [Tooltip("Select the name of the sound to play (popup in inspector).")]
    public string soundName = "EventSound";

    private Sound sound;
    private bool randomizePitch;
    private void Awake()
    {
        AudioManager audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }

        if (audioManager != null)
        {
            if (!string.IsNullOrEmpty(soundName))
            {
                sound = audioManager.GetSound(soundName);
            }
            else
            {
                Debug.LogWarning("PlaySoundEvent: soundName is empty");
            }
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found!");
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        ResetAudioSource();
    }

    public void PlaySound()
    {
        if (sound != null && audioSource != null)
        {
            if (randomizePitch)
            {
                audioSource.pitch = Random.Range(sound.pitchMin, sound.pitchMax);
            }
            else
            {
                audioSource.pitch = sound.pitch;
            }
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("PlaySoundEvent: Sound or AudioSource is null, cannot play sound.");
        }
    }
    public void StopSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void FadeOut()
    {
        if (audioSource != null)
        {
            StartCoroutine(FadeOutCoroutine(1.0f));
        }
    }

    private void ResetAudioSource()
    {
        audioSource.clip = sound?.clip;
        audioSource.volume = sound != null ? sound.volume : 1.0f;
        audioSource.pitch = sound != null ? sound.pitch : 1.0f;
        audioSource.spatialBlend = sound != null ? sound.spatialBlend : 0.0f;
        audioSource.loop = sound != null ? sound.loop : false;
        audioSource.playOnAwake = sound != null ? sound.playOnAwake : false;
        audioSource.outputAudioMixerGroup = sound != null ? sound.outputMixerGroup : null;
        randomizePitch = sound != null && sound.RandomizePitch;
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
