using UnityEngine;

public class RequestTicket : MonoBehaviour
{
    [SerializeField] private RobotStatistics requestRobotStatistics;
    [SerializeField] private StatisticsDisplay statisticsDisplay;

    private void Awake()
    {
        statisticsDisplay.SetStatistics(requestRobotStatistics);
    }

    public void ConfirmRequest()
    {
        GameManager.Instance.StartAssembling(requestRobotStatistics);
    }
}
