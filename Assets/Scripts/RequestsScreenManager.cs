using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestsScreenManager : MonoBehaviour
{
    [Header("Requests Screen Manager References")]
    [SerializeField] private CanvasGroup categoriesCG;
    [SerializeField] private CanvasGroup requestsListCG;
    [SerializeField] private CanvasGroup requestCG;
    [SerializeField] private CanvasGroup tutorialCG;
    [SerializeField] private Button requestButtonPrefab;
    [SerializeField] private Transform requestsListParent;
    [SerializeField] private RequestTicket requestTicket;

    [Header("Satisfaction Levels")]
    [SerializeField] private SatisfactionLevelsSO satisfactionLevelsSO;

    public void InitializeRequestsScreenManager()
    {
        GameManager.Instance.SetCanvasGroup(categoriesCG, true);
        GameManager.Instance.SetCanvasGroup(requestsListCG, false);
        GameManager.Instance.SetCanvasGroup(requestCG, false);
    }

    public void ReturnToRequestsList()
    {
        GameManager.Instance.TransitionBetweenScreens(requestCG, requestsListCG);
    }

    public void ReturnToCategories(CanvasGroup cg)
    {
        GameManager.Instance.TransitionBetweenScreens(cg, categoriesCG);
    }

    public void GoToTutorialScreen()
    {
        GameManager.Instance.TransitionBetweenScreens(categoriesCG, tutorialCG);
    }

    public void SetupRequestsList(RequestsListSO requestsList)
    {
        foreach(Transform child in requestsListParent)
        {
            child.GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(child.gameObject);
        }

        int i = 1;
        foreach(RequestSO requestSO in requestsList.RequestList)
        {
            Button button = Instantiate(requestButtonPrefab, requestsListParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            button.onClick.AddListener(delegate { DisplayRequestTicket(requestSO); });

            string saved = SaveData.GetRequestSatisfaction(requestSO.name);
            if (!string.IsNullOrEmpty(saved))
            {
                RequestButton rb = button.GetComponent<RequestButton>();
                if (rb != null && satisfactionLevelsSO != null)
                {
                    if (satisfactionLevelsSO.TryGetByName(saved, out SatisfactionLevel sl))
                    {
                        rb.AssignSatisfactionLevel(sl);
                    }
                }
            }

            i++;
        }

        GameManager.Instance.TransitionBetweenScreens(categoriesCG, requestsListCG);
    }

    private void DisplayRequestTicket(RequestSO requestSO)
    {
        requestTicket.InitializeRequestTicket(requestSO);

        GameManager.Instance.TransitionBetweenScreens(requestsListCG, requestCG);
    }
}
