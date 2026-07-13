using TMPro;
using UnityEngine;

public class RequestTicket : MonoBehaviour
{
    [SerializeField] private StatisticsDisplay statisticsDisplay;
    [SerializeField] private TextMeshProUGUI requestTitle;
    [SerializeField] private TextMeshProUGUI chronoText;

    private RequestSO requestSO;

    public void InitializeRequestTicket(RequestSO requestSO)
    {
        this.requestSO = requestSO;
        statisticsDisplay.SetStatistics(requestSO.RequestStats);
        requestTitle.text = requestSO.name.ToString();
        chronoText.text = requestSO.Chrono.ToString();
    }

    public void ConfirmRequest()
    {
        GameManager.Instance.StartAssembling(requestSO);
    }
}
