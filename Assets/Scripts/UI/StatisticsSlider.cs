using DG.Tweening;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.UI;

public class StatisticsSlider : MonoBehaviour
{
    [Header("Statistics Slider Settings")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Ease animationEase = Ease.InOutSine;

    [Header("Statistics Slider References")]
    [SerializeField] private Slider mainSlider;
    [SerializeField] private Slider firstSlider;
    [SerializeField] private Image firstSliderImage;
    [SerializeField] private Slider secondSlider;
    [SerializeField] private Image secondSliderImage;
    [SerializeField] private Slider thirdSlider;
    [SerializeField] private Image thirdSliderImage;
    [SerializeField] private Image sliderHandleImage;
    [SerializeField] private Color firstLevelColor;
    [SerializeField] private Color secondLevelColor;
    [SerializeField] private Color thirdLevelColor;
    [SerializeField] private Color perfectLevelColor;
    [SerializeField] private Color zeroLevelColor;

    [Header("Satisfaction Levels")]
    [SerializeField] private SatisfactionLevelsSO satisfactionLevelsSO;

    private void Awake()
    {
        InitializeStatisticsSlider();
    }

    private void OnEnable()
    {
        mainSlider.onValueChanged.AddListener(HandleSliderValue);
    }

    private void OnDisable()
    {
        mainSlider.onValueChanged.RemoveListener(HandleSliderValue);
    }

    public void SetSliderValue(float value)
    {
        mainSlider.DOKill();
        mainSlider.DOValue(value, animationDuration).SetEase(animationEase);
    }

    private void InitializeStatisticsSlider()
    {
        mainSlider.value = 0;
        firstSlider.value = 0;
        secondSlider.value = 0;
        thirdSlider.value = 0;
        firstSliderImage.color = firstLevelColor;
        secondSliderImage.color = secondLevelColor;
        thirdSliderImage.color = thirdLevelColor;
    }

    private void HandleSliderValue(float value)
    {
        float partition = satisfactionLevelsSO.Partition;

        if (Mathf.Approximately(value, 0f))
        {
            firstSlider.value = secondSlider.value = thirdSlider.value = 0f;
            sliderHandleImage.color = zeroLevelColor;
            return;
        }

        firstSlider.value = Mathf.Clamp01(value / partition);
        secondSlider.value = Mathf.Clamp01((value - partition) / partition);
        thirdSlider.value = Mathf.Clamp01((value - 2f * partition) / partition);

        Color handleColor = value < partition ? firstLevelColor :
                           value < 2f * partition ? secondLevelColor :
                           value < 1f ? thirdLevelColor : perfectLevelColor;

        if (sliderHandleImage.color != handleColor)
        {
            sliderHandleImage.color = handleColor;
        }
    }
}
