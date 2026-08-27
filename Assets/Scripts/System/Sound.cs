using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1.0f;
    [Range(0.1f, 3f)]
    public float pitch = 1.0f;
    [Range(0f, 1f)]
    public float spatialBlend = 0.0f;

    public bool loop = false;
    public bool playOnAwake = false;
    public AudioMixerGroup outputMixerGroup;

    public bool RandomizePitch = false;
    [Range(0.1f, 3f)]
    public float pitchMin = 0.9f;
    [Range(0.1f, 3f)]
    public float pitchMax = 1.1f;

    [HideInInspector]
    public AudioSource source;
}
