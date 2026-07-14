using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreenManager : MonoBehaviour
{
    #region Inspector Fields
    [Header("Result Screen Manager References")]
    [SerializeField] private SatisfactionLevelDisplay satisfactionLevelDisplayPrefab;
    [SerializeField] private Transform satisfactionLevelsParent;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject failurePanel;
    [SerializeField] private CanvasGroup finalScoreCG;
    [SerializeField] private Image finalScoreImage;
    [SerializeField] private TextMeshProUGUI finalScoreComment;

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
        failurePanel.SetActive(false);
        resultPanel.SetActive(true);

        robotResult = result;
        HandleResultScreen();
    }

    public void DisplayFailureScreen()
    {
        failurePanel.SetActive(true);
        resultPanel.SetActive(false);
    }

    public void BackToMenu()
    {
        GameManager.Instance.GoToRequestScreen(GetComponent<CanvasGroup>());
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

        CalculateFinalScore();
    }

    private void CalculateFinalScore()
    {
        if (robotResult == null)
        {
            return;
        }

        int count = 0;
        int sumDegrees = 0;

        foreach (var (_, stat) in robotResult.EnumerateStats())
        {
            if (stat.Required <= 0f)
            {
                continue;
            }

            count++;

            if (stat.SatisfactionDegree == SatisfactionLevel.SatisfactionDegree.Unsatisfied)
            {
                var finalSL = GetSatisfactionLevel(SatisfactionLevel.SatisfactionDegree.Unsatisfied);
                if (finalScoreImage != null)
                {
                    finalScoreImage.sprite = finalSL.satisfactionIcon;
                    finalScoreImage.color = finalSL.iconColor;
                }

                if (finalScoreComment != null)
                {
                    finalScoreComment.text = SatisfactionLevel.SatisfactionDegree.Unsatisfied.ToString();
                }

                return;
            }

            sumDegrees += (int)stat.SatisfactionDegree;
        }

        if (count == 0)
        {
            return;
        }

        float avg = (float)sumDegrees / count;
        int nearest = Mathf.Clamp(Mathf.RoundToInt(avg), (int)SatisfactionLevel.SatisfactionDegree.Unsatisfied, (int)SatisfactionLevel.SatisfactionDegree.Perfect);
        var averagedDegree = (SatisfactionLevel.SatisfactionDegree)nearest;
        var averagedSL = GetSatisfactionLevel(averagedDegree);

        if (finalScoreImage != null)
        {
            finalScoreImage.sprite = averagedSL.satisfactionIcon;
            finalScoreImage.color = averagedSL.iconColor;
        }

        if (finalScoreComment != null)
        {
            finalScoreComment.text = averagedDegree.ToString();
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
