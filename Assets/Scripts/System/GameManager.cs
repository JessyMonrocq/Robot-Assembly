using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Inspector Fields
    public static GameManager Instance;

    [Header("Game Manager References")]
    [SerializeField] private CanvasGroup startScreen;
    [SerializeField] private CanvasGroup menuScreen;
    [SerializeField] private CanvasGroup requestScreen;
    [SerializeField] private CanvasGroup assemblerScreen;
    [SerializeField] private CanvasGroup assemblerMiniGameScreen;
    [SerializeField] private CanvasGroup resultsScreen;
    [SerializeField] private StartScreenManager startScreenManager;
    [SerializeField] private RequestsScreenManager requestsScreenManager;
    [SerializeField] private AssemblerGameManager assemblerGameManager;
    [SerializeField] private InteractionsScreenManager interactionsScreenManager;
    [SerializeField] private ResultScreenManager resultScreenManager;

    [SerializeField] private float transitionSpeed = 0.5f;
    [SerializeField] private float transitionDelay = 0.5f;
    [SerializeField] private float chronoDelay = 1f;
    [SerializeField] private Ease transitionEase;

    private RobotResult currentRobotResult;
    #endregion

    #region Unity Methods
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

        Debug.Log(Application.persistentDataPath.ToString());

        currentRobotResult = null;

        SetCanvasGroup(startScreen, false);
        SetCanvasGroup(menuScreen, false);
        SetCanvasGroup(requestScreen, false);
        SetCanvasGroup(assemblerScreen, false);
        SetCanvasGroup(assemblerMiniGameScreen, false);
        SetCanvasGroup(resultsScreen, false);

        startScreenManager.ResetStartScreen();
    }

    private void Start()
    {
        startScreenManager.InitializeStartScreen();
    }
    #endregion

    #region Public Methods
    public void StartAssembling(RequestSO request)
    {
        assemblerGameManager.ResetAssemblerGame(request.Chrono, chronoDelay);
        assemblerGameManager.SetRequestedStatistics(request.RequestStats);
        TransitionBetweenScreens(requestScreen, assemblerScreen);

        // PLAYTEST
        if (PlaytestLogger.Instance != null)
        {
            PlaytestLogger.Instance.OnRequestStarted(request);
        }
    }

    public void HandleRequestResults(RobotResult results, bool success)
    {
        if (success)
        {
            TransitionBetweenScreens(assemblerScreen, assemblerMiniGameScreen);
            interactionsScreenManager.InitializeMiniGame();
            currentRobotResult = results;
        }
        else
        {
            TransitionBetweenScreens(assemblerScreen, resultsScreen);
            resultScreenManager.DisplayFailureScreen();
        }

        // PLAYTEST 
        if (PlaytestLogger.Instance != null)
        {
            PlaytestLogger.Instance.OnRequestFinished(results, success);
        }
    }

    public void GoToResultScreen(CanvasGroup cg)
    {
        TransitionBetweenScreens(cg, resultsScreen);
        resultScreenManager.SetResultStatistics(currentRobotResult);

        // PLAYTEST
        string finalSatisfactionLevel = resultScreenManager.GetFinalSatisfactionLevel();
        if (PlaytestLogger.Instance != null)
        {
            PlaytestLogger.Instance.OnMiniGameFinished(finalSatisfactionLevel);
        }
    }

    public void GoToRequestScreen(CanvasGroup cg)
    {
        requestsScreenManager.InitializeRequestsScreenManager();
        TransitionBetweenScreens(cg, requestScreen);
    }

    public void GoToMenu(CanvasGroup cg)
    {
        TransitionBetweenScreens(cg, menuScreen);
    }

    public void GoToMinigame(CanvasGroup cg)
    {
        interactionsScreenManager.InitializeMiniGame();
        TransitionBetweenScreens(cg, assemblerMiniGameScreen);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetCanvasGroup(CanvasGroup cg, bool enabled)
    {
        cg.alpha = enabled ? 1f : 0f;
        cg.interactable = enabled ? true : false;
        cg.blocksRaycasts = enabled ? true : false;
    }

    public void TransitionBetweenScreens(CanvasGroup firstScreen, CanvasGroup newScreen)
    {
        StartCoroutine(TransitionCoroutine(firstScreen, newScreen));
    }
    #endregion

    #region Coroutine Methods
    private IEnumerator TransitionCoroutine(CanvasGroup firstScene, CanvasGroup newScene)
    {
        firstScene.interactable = false;
        firstScene.blocksRaycasts = false;
        yield return firstScene.DOFade(0f, transitionSpeed).SetEase(transitionEase).WaitForCompletion();
        yield return new WaitForSeconds(transitionDelay);
        yield return newScene.DOFade(1f, transitionSpeed).SetEase(transitionEase).WaitForCompletion();
        newScene.interactable = true;
        newScene.blocksRaycasts = true;
    }
    #endregion
}