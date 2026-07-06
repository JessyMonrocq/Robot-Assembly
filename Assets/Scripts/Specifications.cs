using UnityEngine;

public class Specifications : MonoBehaviour
{
    [Header("Robot Specifications References")]
    [SerializeField] private StatDisplay armorStatDisplay;
    [SerializeField] private StatDisplay mobilityStatDisplay;
    [SerializeField] private StatDisplay strengthStatDisplay;
    [SerializeField] private StatDisplay computingStatDisplay;
    [SerializeField] private StatDisplay energyStatDisplay;
    [SerializeField] private StatDisplay weightStatDisplay;

    [Space(10)]
    [SerializeField] private RobotStatistics requiredStatistics;

    public RobotStatistics RequiredStatistics
    {
        get { return requiredStatistics; }
        set { requiredStatistics = value; }
    }

    public void UpdateSpecifications(RobotStatistics robotStatistics)
    {
        armorStatDisplay.SetStatDisplayValues(robotStatistics.Armor / requiredStatistics.Armor, robotStatistics.Armor, requiredStatistics.Armor, requiredStatistics.Armor > 0);
        mobilityStatDisplay.SetStatDisplayValues(robotStatistics.Mobility / requiredStatistics.Mobility, robotStatistics.Mobility, requiredStatistics.Mobility, requiredStatistics.Mobility > 0);
        strengthStatDisplay.SetStatDisplayValues(robotStatistics.Strength / requiredStatistics.Strength, robotStatistics.Strength, requiredStatistics.Strength, requiredStatistics.Strength > 0);
        computingStatDisplay.SetStatDisplayValues(robotStatistics.Computing / requiredStatistics.Computing, robotStatistics.Computing, requiredStatistics.Computing, requiredStatistics.Computing > 0);
        
        energyStatDisplay.SetStatDisplayValues(1 - (robotStatistics.EnergyConsumption / requiredStatistics.EnergyConsumption), robotStatistics.EnergyConsumption, requiredStatistics.EnergyConsumption, requiredStatistics.EnergyConsumption > 0);
        weightStatDisplay.SetStatDisplayValues(1 - (robotStatistics.Weight / requiredStatistics.Weight), robotStatistics.Weight, requiredStatistics.Weight, requiredStatistics.Weight > 0);
    }
}
