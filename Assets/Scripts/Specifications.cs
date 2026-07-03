using UnityEngine;

public class Specifications : MonoBehaviour
{
    [Header("Robot Specifications References")]
    [SerializeField] private StatDisplay armorStatDisplay;
    [SerializeField] private StatDisplay mobilityStatDisplay;
    [SerializeField] private StatDisplay strengthStatDisplay;
    [SerializeField] private StatDisplay computationStatDisplay;
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
        armorStatDisplay.SetStatDisplayValues(robotStatistics.Armor / requiredStatistics.Armor, robotStatistics.Armor, requiredStatistics.Armor);
        mobilityStatDisplay.SetStatDisplayValues(robotStatistics.Mobility / requiredStatistics.Mobility, robotStatistics.Mobility, requiredStatistics.Mobility);
        strengthStatDisplay.SetStatDisplayValues(robotStatistics.Strength / requiredStatistics.Strength, robotStatistics.Strength, requiredStatistics.Strength);
        computationStatDisplay.SetStatDisplayValues(robotStatistics.Computation / requiredStatistics.Computation, robotStatistics.Computation, requiredStatistics.Computation);
        
        energyStatDisplay.SetStatDisplayValues(1 - (robotStatistics.EnergyConsumption / requiredStatistics.EnergyConsumption), robotStatistics.EnergyConsumption, requiredStatistics.EnergyConsumption);
        weightStatDisplay.SetStatDisplayValues(1 - (robotStatistics.Weight / requiredStatistics.Weight), robotStatistics.Weight, requiredStatistics.Weight);
    }
}
