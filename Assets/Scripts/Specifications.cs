using UnityEngine;

public class Specifications : MonoBehaviour
{
    #region Inspector Fields
    [Header("Robot Specifications References")]
    [SerializeField] private StatDisplay armorStatDisplay;
    [SerializeField] private StatDisplay mobilityStatDisplay;
    [SerializeField] private StatDisplay strengthStatDisplay;
    [SerializeField] private StatDisplay computingStatDisplay;
    [SerializeField] private StatDisplay energyStatDisplay;
    [SerializeField] private StatDisplay weightStatDisplay;

    [Space(10)]
    [SerializeField] private RobotStatistics requiredStatistics;
    private RobotStatistics currentStatistics;

    public RobotStatistics RequiredStatistics
    {
        get { return requiredStatistics; }
        set { requiredStatistics = value; }
    }

    [Space(10)]
    [Header("Satisfaction Levels")]
    [SerializeField] private SatisfactionLevelsSO satisfactionLevelsSO;
    #endregion

    #region Public Methods
    public void UpdateSpecifications(RobotStatistics robotStatistics)
    {
        armorStatDisplay.SetStatDisplayValues(robotStatistics.Armor / requiredStatistics.Armor, robotStatistics.Armor, requiredStatistics.Armor, requiredStatistics.Armor > 0);
        mobilityStatDisplay.SetStatDisplayValues(robotStatistics.Mobility / requiredStatistics.Mobility, robotStatistics.Mobility, requiredStatistics.Mobility, requiredStatistics.Mobility > 0);
        strengthStatDisplay.SetStatDisplayValues(robotStatistics.Strength / requiredStatistics.Strength, robotStatistics.Strength, requiredStatistics.Strength, requiredStatistics.Strength > 0);
        computingStatDisplay.SetStatDisplayValues(robotStatistics.Computing / requiredStatistics.Computing, robotStatistics.Computing, requiredStatistics.Computing, requiredStatistics.Computing > 0);
        
        energyStatDisplay.SetStatDisplayValues(1 - (robotStatistics.Energy / requiredStatistics.Energy), robotStatistics.Energy, requiredStatistics.Energy, requiredStatistics.Energy > 0);
        weightStatDisplay.SetStatDisplayValues(1 - (robotStatistics.Weight / requiredStatistics.Weight), robotStatistics.Weight, requiredStatistics.Weight, requiredStatistics.Weight > 0);

        currentStatistics = robotStatistics;
    }

    public RobotResult CreateRobotResult()
    {
        RobotResult result = new RobotResult();

        result.Armor = CreateStatResult(requiredStatistics.Armor, currentStatistics.Armor, higherIsBetter: true);
        result.Mobility = CreateStatResult(requiredStatistics.Mobility, currentStatistics.Mobility, higherIsBetter: true);
        result.Strength = CreateStatResult(requiredStatistics.Strength, currentStatistics.Strength, higherIsBetter: true);
        result.Computing = CreateStatResult(requiredStatistics.Computing, currentStatistics.Computing, higherIsBetter: true);

        result.Energy = CreateStatResult(requiredStatistics.Energy, currentStatistics.Energy, higherIsBetter: false);
        result.Weight = CreateStatResult(requiredStatistics.Weight, currentStatistics.Weight, higherIsBetter: false);

        return result;
    }
    #endregion

    #region Private Methods
    private RobotStatResult CreateStatResult(float required, float current, bool higherIsBetter)
    {
        RobotStatResult statResult = new RobotStatResult
        {
            Required = required,
            Result = current,
            SatisfactionDegree = SatisfactionLevel.SatisfactionDegree.Unsatisfied
        };

        if (required <= 0f)
        {
            return statResult;
        }

        float ratio;
        if (higherIsBetter)
        {
            ratio = current / required;
        }
        else
        {
            ratio = 1f - (current / required);
        }

        ratio = Mathf.Clamp01(ratio);

        statResult.SatisfactionDegree = satisfactionLevelsSO.MapRatioToSatisfactionDegree(ratio);
        return statResult;
    }
    #endregion
}
