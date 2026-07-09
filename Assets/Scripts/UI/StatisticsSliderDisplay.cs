using TMPro;
using UnityEngine;

public class StatisticsSliderDisplay : MonoBehaviour
{
    [Header("Stat Display References")]
    [SerializeField] private StatisticsSlider statisticsSlider;
    [SerializeField] private TextMeshProUGUI statisticsLabel;
    [SerializeField] private TextMeshProUGUI statisticsValueText;

    public void SetStatDisplayValues(float sliderValue, float statValue, float reqStatValue, bool isRequired)
    {
        statisticsSlider.SetSliderValue(sliderValue);
        statisticsValueText.text = statValue.ToString() + "/" + reqStatValue.ToString();
        statisticsLabel.fontStyle = isRequired ? FontStyles.Bold : FontStyles.Normal;
        statisticsLabel.fontStyle = isRequired ? FontStyles.Underline : FontStyles.Normal;
    }
}
