using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public enum AudioGroup
    {
        Master,
        Music,
        SFX
    }

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TMP_InputField masterVolumeInput;
    [SerializeField] private TMP_InputField musicVolumeInput;
    [SerializeField] private TMP_InputField sfxVolumeInput;

    private const float decibelMult = 20;
    private const float decibelMin = -80;

    private void OnEnable()
    {
        masterVolumeSlider.onValueChanged.AddListener(delegate { SetGameVolume(AudioGroup.Master, masterVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { SetGameVolume(AudioGroup.Music, musicVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { SetGameVolume(AudioGroup.SFX, sfxVolumeSlider.value); });
    }

    private void OnDisable()
    {
        masterVolumeSlider.onValueChanged.RemoveListener(delegate { SetGameVolume(AudioGroup.Master, masterVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.RemoveListener(delegate { SetGameVolume(AudioGroup.Music, musicVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.RemoveListener(delegate { SetGameVolume(AudioGroup.SFX, sfxVolumeSlider.value); });
    }

    private void SetGameVolume(AudioGroup audioGroup, float value)
    {
        if (value <= 0)
        {
            audioMixer.SetFloat(audioGroup.ToString(), decibelMin);
        }
        else
        {
            float dbValue = Mathf.Log10(value / 100f) * decibelMult;
            audioMixer.SetFloat(audioGroup.ToString(), dbValue);
        }

        switch (audioGroup)
        {
            case AudioGroup.Master:
                masterVolumeInput.text = value.ToString();
                break;
            case AudioGroup.Music:
                musicVolumeInput.text = value.ToString();
                break;
            case AudioGroup.SFX:
                sfxVolumeInput.text = value.ToString();
                break;
        }
    }
}
