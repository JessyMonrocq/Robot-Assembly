using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SatisfactionLevelDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statLabel;
    [SerializeField] private TextMeshProUGUI statRatio;
    [SerializeField] private Image satisfactionImage;

    public void SetSatisfactionLevelDisplayValues(string statName, float statValue, float reqStatValue, SatisfactionLevel satisfactionLevel)
    {
        statLabel.text = statName;
        statRatio.text = statValue.ToString() + "/" + reqStatValue.ToString();

        satisfactionImage.sprite = satisfactionLevel.satisfactionIcon;
        satisfactionImage.color = satisfactionLevel.iconColor;
    }
}
