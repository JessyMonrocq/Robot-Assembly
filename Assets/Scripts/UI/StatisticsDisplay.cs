using TMPro;
using UnityEngine;

public class StatisticsDisplay : MonoBehaviour
{
    [Header("Name Texts")]
    [SerializeField] private TextMeshProUGUI armorName;
    [SerializeField] private TextMeshProUGUI mobilityName;
    [SerializeField] private TextMeshProUGUI strengthName;
    [SerializeField] private TextMeshProUGUI computingName;
    [SerializeField] private TextMeshProUGUI energyName;
    [SerializeField] private TextMeshProUGUI weightName;

    [Header("Value Texts")]
    [SerializeField] private TextMeshProUGUI armorValue;
    [SerializeField] private TextMeshProUGUI mobilityValue;
    [SerializeField] private TextMeshProUGUI strengthValue;
    [SerializeField] private TextMeshProUGUI computingValue;
    [SerializeField] private TextMeshProUGUI energyValue;
    [SerializeField] private TextMeshProUGUI weightValue;

    private void OnValidate()
    {
        armorName.text = string.IsNullOrEmpty(armorName.text) ? "Armor" : armorName.text;
        mobilityName.text = string.IsNullOrEmpty(mobilityName.text) ? "Mobility" : mobilityName.text;
        strengthName.text = string.IsNullOrEmpty(strengthName.text) ? "Strength" : strengthName.text;
        computingName.text = string.IsNullOrEmpty(computingName.text) ? "Computing" : computingName.text;
        energyName.text = string.IsNullOrEmpty(energyName.text) ? "Energy" : energyName.text;
        weightName.text = string.IsNullOrEmpty(weightName.text) ? "Weight" : weightName.text;
    }

    public void SetStatistics(RobotStatistics stats)
    {
        armorValue.text = stats.Armor.ToString();
        mobilityValue.text = stats.Mobility.ToString();
        strengthValue.text = stats.Strength.ToString();
        computingValue.text = stats.Computing.ToString();
        energyValue.text = stats.Energy.ToString();
        weightValue.text = stats.Weight.ToString();
    }

    public void Clear()
    {
        armorValue.text = "-";
        mobilityValue.text = "-";
        strengthValue.text = "-";
        computingValue.text = "-";
        energyValue.text = "-";
        weightValue.text = "-";
    }
}
