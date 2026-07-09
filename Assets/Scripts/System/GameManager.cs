using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Manager References")]
    [SerializeField] private CanvasGroup requestScreen;
    [SerializeField] private CanvasGroup assemblerScreen;
    [SerializeField] private CanvasGroup assemblerGameScree;
    [SerializeField] private CanvasGroup resultsScreen;
    [SerializeField] private AssemblerGameManager assemblerGameManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        SetCanvasGroup(requestScreen, true, false);
        SetCanvasGroup(assemblerScreen, false, false);
        SetCanvasGroup(assemblerGameScree, false, false);
        SetCanvasGroup(resultsScreen, false, false);
    }

    public void StartAssembling(RobotStatistics requestedRobotStatistics)
    {
        SetCanvasGroup(requestScreen, false, false);
        assemblerGameManager.ResetAssemblerGame();
        assemblerGameManager.SetRequestedStatistics(requestedRobotStatistics);
        SetCanvasGroup(assemblerScreen, true, false);
    }
    
    public void ReturnToRequestScreen()
    {
        SetCanvasGroup(resultsScreen, false, false);
        SetCanvasGroup(requestScreen, true, false);
    }

    public void DEBUG_TEST()
    {
        Debug.Log("Test Message");
    }

    private void SetCanvasGroup(CanvasGroup cg, bool enabled, bool useTransition)
    {
        if (useTransition)
        {

        }
        else
        {
            cg.alpha = enabled ? 1f : 0f;
            cg.interactable = enabled ? true : false;
            cg.blocksRaycasts = enabled ? true : false;
        }
    }
}
