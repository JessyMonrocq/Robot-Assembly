using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    [Header("Stat Display References")]
    [SerializeField] private StatisticsSlider statisticsSlider;
    [SerializeField] private TextMeshProUGUI statisticsText;

    public void SetStatDisplayValues(float sliderValue, float statValue, float reqStatValue)
    {
        statisticsSlider.SetSliderValue(sliderValue);
        statisticsText.text = statValue.ToString() + "/" + reqStatValue.ToString();
    }
}
