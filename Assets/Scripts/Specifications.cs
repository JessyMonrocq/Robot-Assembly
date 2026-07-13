using UnityEngine;

public class Specifications : MonoBehaviour
{
    #region Inspector Fields
    [Header("Robot Specifications References")]
    [SerializeField] private StatisticsSliderDisplay armorStatDisplay;
    [SerializeField] private StatisticsSliderDisplay mobilityStatDisplay;
    [SerializeField] private StatisticsSliderDisplay strengthStatDisplay;
    [SerializeField] private StatisticsSliderDisplay computingStatDisplay;
    [SerializeField] private StatisticsSliderDisplay energyStatDisplay;
    [SerializeField] private StatisticsSliderDisplay weightStatDisplay;

    [Space(10)]
    private RobotStatistics requiredStatistics;
    private RobotStatistics currentStatistics;

    [Space(10)]
    [Header("Satisfaction Levels")]
    [SerializeField] private SatisfactionLevelsSO satisfactionLevelsSO;
    #endregion

    #region Public Methods
    public void InitializeSpecifications(RobotStatistics statistics)
    {
        requiredStatistics = statistics;
        UpdateSpecifications(new RobotStatistics());
    }

    public void UpdateSpecifications(RobotStatistics robotStatistics)
    {
        float armorSlider = requiredStatistics.Armor > 0f ? Mathf.Clamp01(robotStatistics.Armor / requiredStatistics.Armor) : 0f;
        float mobilitySlider = requiredStatistics.Mobility > 0f ? Mathf.Clamp01(robotStatistics.Mobility / requiredStatistics.Mobility) : 0f;
        float strengthSlider = requiredStatistics.Strength > 0f ? Mathf.Clamp01(robotStatistics.Strength / requiredStatistics.Strength) : 0f;
        float computingSlider = requiredStatistics.Computing > 0f ? Mathf.Clamp01(robotStatistics.Computing / requiredStatistics.Computing) : 0f;

        float energySlider = requiredStatistics.Energy > 0f ? Mathf.Clamp01(2f - (robotStatistics.Energy / requiredStatistics.Energy)) : 0f;
        float weightSlider = requiredStatistics.Weight > 0f ? Mathf.Clamp01(2f - (robotStatistics.Weight / requiredStatistics.Weight)) : 0f;

        armorStatDisplay.SetStatDisplayValues(armorSlider, robotStatistics.Armor, requiredStatistics.Armor, requiredStatistics.Armor > 0);
        mobilityStatDisplay.SetStatDisplayValues(mobilitySlider, robotStatistics.Mobility, requiredStatistics.Mobility, requiredStatistics.Mobility > 0);
        strengthStatDisplay.SetStatDisplayValues(strengthSlider, robotStatistics.Strength, requiredStatistics.Strength, requiredStatistics.Strength > 0);
        computingStatDisplay.SetStatDisplayValues(computingSlider, robotStatistics.Computing, requiredStatistics.Computing, requiredStatistics.Computing > 0);

        energyStatDisplay.SetStatDisplayValues(energySlider, robotStatistics.Energy, requiredStatistics.Energy, requiredStatistics.Energy > 0);
        weightStatDisplay.SetStatDisplayValues(weightSlider, robotStatistics.Weight, requiredStatistics.Weight, requiredStatistics.Weight > 0);

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
            if (current <= required)
            {
                ratio = 1f;
            }
            else
            {
                float overflowFactor = current / required;
                ratio = 2f - overflowFactor;
            }
        }

        ratio = Mathf.Clamp01(ratio);

        statResult.SatisfactionDegree = satisfactionLevelsSO.MapRatioToSatisfactionDegree(ratio);
        return statResult;
    }
    #endregion
}
