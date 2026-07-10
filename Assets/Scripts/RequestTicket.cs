using TMPro;
using UnityEngine;

public class RequestTicket : MonoBehaviour
{
    [SerializeField] private RequestSO requestSO;
    [SerializeField] private StatisticsDisplay statisticsDisplay;
    [SerializeField] private TextMeshProUGUI chronoText;

    private void Awake()
    {
        statisticsDisplay.SetStatistics(requestSO.RequestStats);
        chronoText.text = requestSO.Chrono.ToString();
    }

    public void ConfirmRequest()
    {
        GameManager.Instance.StartAssembling(requestSO);
    }
}
