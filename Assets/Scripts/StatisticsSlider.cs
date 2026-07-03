using DG.Tweening;
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
    [SerializeField] private Color firstLevelColor;
    [SerializeField] private Color secondLevelColor;
    [SerializeField] private Color thirdLevelColor;

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
        if (value == 0)
        {
            firstSlider.value = 0;
            secondSlider.value = 0;
            thirdSlider.value = 0;
            return;
        }

        firstSlider.value = value / (1f / 3f);
        secondSlider.value = (value - (1f / 3f)) / (1f / 3f);
        thirdSlider.value = (value - (2f / 3f)) / (1f / 3f);
    }
}
