using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FrequencySlider : MonoBehaviour
{
    public event Action<bool> OnFrequencyAligned;

    [SerializeField] private Slider gaugeSlider;
    [SerializeField] private Slider goalSlider;
    [SerializeField] private RectTransform gaugeHandle;
    [SerializeField] private RectTransform goalHandle;

    [SerializeField] private float frequencySpeed = 1;
    [SerializeField] private Ease frequencyEase = Ease.Linear;

    private bool frequencyOn;
    private bool frequencyAligned;

    public void ResetFrequency()
    {
        frequencyOn = false;
        frequencyAligned = false;

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
