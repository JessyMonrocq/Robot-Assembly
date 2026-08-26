using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] musics;
    public Sound[] sounds;

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
    }

    private void OnValidate()
    {
        foreach (Sound m in musics)
        {
            m.name = m.clip.name;
        }

        foreach (Sound s in sounds)
        {
            s.name = s.clip.name;
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
}
