using System.Collections;
using UnityEngine;

public class FrequenciesInteraction : MiniInteraction
{
    [SerializeField] private FrequencySlider[] frequencySliders;

    private bool frequencyAligned;
    private bool interactable;
    private int frequencySlidersCount;

    private const float wrongInputDelay = 1f;

    private void OnEnable()
    {
        foreach (FrequencySlider slider in frequencySliders)
        {
            slider.OnFrequencyAligned += UpdateFrequencySliderState;
        }
    }

    private void OnDisable()
    {
        foreach (FrequencySlider slider in frequencySliders)
        {
            slider.OnFrequencyAligned -= UpdateFrequencySliderState;
        }
    }

    public override void ResetInteraction()
    {
        frequencyAligned = false;
        frequencySlidersCount = 0;
        interactable = false;

        foreach (FrequencySlider slider in frequencySliders)
        {
            slider.ResetFrequency();
        }
    }

    public override void InitializeInteraction()
    {
        foreach (FrequencySlider slider in frequencySliders)
        {
            float random = Random.Range(0f, 1f);
            slider.InitializeFrequency(random);
        }

        frequencySliders[frequencySlidersCount].StartFrequency();
        interactable = true;
    }

    public void Interact()
    {
        if (!interactable)
        {
            return;
        }

        if (frequencyAligned)
        {
            frequencySliders[frequencySlidersCount].StopFrequency();
            frequencySlidersCount++;
            if (frequencySlidersCount == frequencySliders.Length)
            {
                InvokeOnInteractionCompleted();
                interactable = false;
            }
            else
            {
                frequencySliders[frequencySlidersCount].StartFrequency();
            }
        }
        else
        {
            interactable = false;
            StartCoroutine(WrongInputCoroutine());
        }
    }

    private void UpdateFrequencySliderState(bool frequencyAligned)
    {
        this.frequencyAligned = frequencyAligned;
    }

    private IEnumerator WrongInputCoroutine()
    {
        // Set color red
        // Shake the screen

        yield return new WaitForSeconds(wrongInputDelay);

        // Set color back to normal
        interactable = true;
    }
}
