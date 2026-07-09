using System;
using UnityEngine;

public class ResultScreenManager : MonoBehaviour
{
    #region Inspector Fields
    [Header("Result Screen Manager References")]
    [SerializeField] private SatisfactionLevelDisplay satisfactionLevelDisplayPrefab;
    [SerializeField] private Transform satisfactionLevelsParent;

    [Header("Satisfaction Levels")]
    [SerializeField] private SatisfactionLevel unsatisfiedLevel;
    [SerializeField] private SatisfactionLevel poorLevel;
    [SerializeField] private SatisfactionLevel averageLevel;
    [SerializeField] private SatisfactionLevel goodLevel;
    [SerializeField] private SatisfactionLevel perfectLevel;

    private RobotResult robotResult;
    #endregion

    #region Public Methods
    public void SetResultStatistics(RobotResult result)
    {
        robotResult = result;
        HandleResultScreen();
    }

    public void BackToMenu()
    {
        GameManager.Instance.ReturnToRequestScreen();
    }
    #endregion

    #region Private Methods
    private void HandleResultScreen()
    {
        if (robotResult == null)
        {
            return;
        }

        foreach (Transform child in satisfactionLevelsParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var (name, stat) in robotResult.EnumerateStats())
        {
            CreateDisplay(name, stat);
        }
    }

    private void CreateDisplay(string statName, RobotStatResult stat)
    {
        if (stat.Required <= 0)
        {
            return;
        }

        SatisfactionLevelDisplay sld = Instantiate(satisfactionLevelDisplayPrefab, satisfactionLevelsParent);
        SatisfactionLevel sl = GetSatisfactionLevel(stat.SatisfactionDegree);
        sld.SetSatisfactionLevelDisplayValues(statName, stat.Result, stat.Required, sl);
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
    #endregion
}
