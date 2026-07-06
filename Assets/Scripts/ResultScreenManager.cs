using System;
using UnityEngine;

[Serializable]
public struct SatisfactionLevel
{
    public enum SatisfactionDegree
    {
        Unsatisfied,
        Poor,
        Average,
        Good,
        Perfect
    }

    public SatisfactionDegree satisfactionDegree;
    public Sprite satisfactionIcon;
    public Color iconColor;
}

public class ResultScreenManager : MonoBehaviour
{
    [Header("Result Screen Manager References")]
    [SerializeField] private SatisfactionLevelDisplay satisfactionLevelDisplayPrefab;
    [SerializeField] private Transform satisfactionLevelsParent;

    [Header("Satisfaction Levels")]
    [SerializeField] private SatisfactionLevel unsatisfiedLevel;
    [SerializeField] private SatisfactionLevel poorLevel;
    [SerializeField] private SatisfactionLevel averageLevel;
    [SerializeField] private SatisfactionLevel goodLevel;
    [SerializeField] private SatisfactionLevel perfectLevel;

    [SerializeField] private RobotResult robotResult;

    private void Awake()
    {
        HandleResultScreen();
    }

    public void SetResultStatistics(RobotResult result)
    {
        robotResult = result;
        HandleResultScreen();
    }

    private void HandleResultScreen()
    {
        if (robotResult == null)
        {
            return;
        }

        if (robotResult.Armor.Required > 0)
        {
            SatisfactionLevelDisplay sld = Instantiate(satisfactionLevelDisplayPrefab, satisfactionLevelsParent);
            SatisfactionLevel sl = GetSatisfactionLevel(robotResult.Armor.SatisfactionDegree);
            sld.SetSatisfactionLevelDisplayValues("Armor", robotResult.Armor.Result, robotResult.Armor.Required, sl);
        }

        if (robotResult.Mobility.Required > 0)
        {
            SatisfactionLevelDisplay sld = Instantiate(satisfactionLevelDisplayPrefab, satisfactionLevelsParent);
            SatisfactionLevel sl = GetSatisfactionLevel(robotResult.Mobility.SatisfactionDegree);
            sld.SetSatisfactionLevelDisplayValues("Mobility", robotResult.Mobility.Result, robotResult.Mobility.Required, sl);
        }

        if (robotResult.Strength.Required > 0)
        {
            SatisfactionLevelDisplay sld = Instantiate(satisfactionLevelDisplayPrefab, satisfactionLevelsParent);
            SatisfactionLevel sl = GetSatisfactionLevel(robotResult.Strength.SatisfactionDegree);
            sld.SetSatisfactionLevelDisplayValues("Strength", robotResult.Strength.Result, robotResult.Strength.Required, sl);
        }

        if (robotResult.Computing.Required > 0)
        {
            SatisfactionLevelDisplay sld = Instantiate(satisfactionLevelDisplayPrefab, satisfactionLevelsParent);
            SatisfactionLevel sl = GetSatisfactionLevel(robotResult.Computing.SatisfactionDegree);
            sld.SetSatisfactionLevelDisplayValues("Computing", robotResult.Computing.Result, robotResult.Computing.Required, sl);
        }

        if (robotResult.Energy.Required > 0)
        {
            SatisfactionLevelDisplay sld = Instantiate(satisfactionLevelDisplayPrefab, satisfactionLevelsParent);
            SatisfactionLevel sl = GetSatisfactionLevel(robotResult.Energy.SatisfactionDegree);
            sld.SetSatisfactionLevelDisplayValues("Energy", robotResult.Energy.Result, robotResult.Energy.Required, sl);
        }

        if (robotResult.Weight.Required > 0)
        {
            SatisfactionLevelDisplay sld = Instantiate(satisfactionLevelDisplayPrefab, satisfactionLevelsParent);
            SatisfactionLevel sl = GetSatisfactionLevel(robotResult.Weight.SatisfactionDegree);
            sld.SetSatisfactionLevelDisplayValues("Weight", robotResult.Weight.Result, robotResult.Weight.Required, sl);
        }
    }

    private SatisfactionLevel GetSatisfactionLevel(SatisfactionLevel.SatisfactionDegree degree)
    {
        return degree switch
        {
            SatisfactionLevel.SatisfactionDegree.Unsatisfied => unsatisfiedLevel,
            SatisfactionLevel.SatisfactionDegree.Poor => poorLevel,
            SatisfactionLevel.SatisfactionDegree.Average => averageLevel,
            SatisfactionLevel.SatisfactionDegree.Good => goodLevel,
            SatisfactionLevel.SatisfactionDegree.Perfect => perfectLevel,
            _ => throw new ArgumentOutOfRangeException(nameof(degree), degree, null)
        };
    }
}
