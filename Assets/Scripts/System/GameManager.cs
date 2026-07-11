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
    [SerializeField] private CanvasGroup assemblerGameScreen;
    [SerializeField] private CanvasGroup resultsScreen;
    [SerializeField] private CanvasGroup failureScreen;
    [SerializeField] private StartScreenManager startScreenManager;
    [SerializeField] private AssemblerGameManager assemblerGameManager;
    [SerializeField] private ResultScreenManager resultScreenManager;

    [SerializeField] private float transitionSpeed = 0.5f;
    [SerializeField] private float transitionDelay = 0.5f;
    [SerializeField] private float chronoDelay = 1f;
    [SerializeField] private Ease transitionEase;
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

        SetCanvasGroup(startScreen, false);
        SetCanvasGroup(menuScreen, false);
        SetCanvasGroup(requestScreen, false);
        SetCanvasGroup(assemblerScreen, false);
        SetCanvasGroup(assemblerGameScreen, false);
        SetCanvasGroup(resultsScreen, false);
        SetCanvasGroup(failureScreen, false);

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
    }

    public void DisplayResultScreen(RobotResult results)
    {
        TransitionBetweenScreens(assemblerScreen, resultsScreen);
        resultScreenManager.SetResultStatistics(results);
    }

    public void DisplayFailureScreen()
    {
        TransitionBetweenScreens(assemblerScreen, failureScreen);
    }

    public void ReturnToRequestScreen(CanvasGroup cg)
    {
        TransitionBetweenScreens(cg, requestScreen);
    }

    public void GoToMenu(CanvasGroup cg)
    {
        TransitionBetweenScreens(cg, menuScreen);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Private Methods
    private void SetCanvasGroup(CanvasGroup cg, bool enabled)
    {
        cg.alpha = enabled ? 1f : 0f;
        cg.interactable = enabled ? true : false;
        cg.blocksRaycasts = enabled ? true : false;
    }

    private void TransitionBetweenScreens(CanvasGroup firstScreen, CanvasGroup newScreen)
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