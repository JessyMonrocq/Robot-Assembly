using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FrequencySlider : MonoBehaviour
{
    public event Action<bool> OnFrequencyAligned;

    public enum FrequencyState
    {
        Default,
        Error,
        Correct
    }

    [SerializeField] private Slider gaugeSlider;
    [SerializeField] private Slider goalSlider;
    [SerializeField] private RectTransform gaugeHandle;
    [SerializeField] private RectTransform goalHandle;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color defaultBackgroundColor;
    [SerializeField] private Color errorBackgroundColor;
    [SerializeField] private Color correctBackgroundColor;
    [SerializeField] private float frequencySpeed = 1;
    [SerializeField] private Ease frequencyEase = Ease.Linear;

    private bool frequencyOn;
    private bool frequencyAligned;

    public void ResetFrequency()
    {
        frequencyOn = false;
        frequencyAligned = false;

        SetBackgroundColor(FrequencyState.Default);

        gaugeSlider.DOKill();
        gaugeSlider.value = 0;
        goalSlider.value = 1;
    }

    public void InitializeFrequency(float value)
    {
        goalSlider.value = value;
    }

    public void StopFrequency()
    {
        frequencyOn = false;
        gaugeSlider.DOKill();
    }

    public void StartFrequency()
    {
        frequencyOn = true;
        gaugeSlider.DOValue(1, frequencySpeed).SetLoops(-1, LoopType.Yoyo).SetEase(frequencyEase);
    }

    public void SetBackgroundColor(FrequencyState state)
    {
        switch (state)
        {
            case FrequencyState.Default:
                backgroundImage.color = defaultBackgroundColor;
                break;
            case FrequencyState.Error:
                backgroundImage.color = errorBackgroundColor;
                break;
            case FrequencyState.Correct:
                backgroundImage.color = correctBackgroundColor;
                break;
        }
    }

    private void Update()
    {
        if (frequencyOn)
        {
            if (RectOverlap.IsOverlapping(gaugeHandle, goalHandle) && !frequencyAligned)
            {
                frequencyAligned = true;
                OnFrequencyAligned?.Invoke(true);
            }
            else if (!RectOverlap.IsOverlapping(gaugeHandle, goalHandle) && frequencyAligned)
            {
                frequencyAligned = false;
                OnFrequencyAligned?.Invoke(false);
            }
        }
    }
}
