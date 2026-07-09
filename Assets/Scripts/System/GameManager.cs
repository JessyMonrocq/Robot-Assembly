using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Inspector Fields
    public static GameManager Instance;

    [Header("Game Manager References")]
    [SerializeField] private CanvasGroup requestScreen;
    [SerializeField] private CanvasGroup assemblerScreen;
    [SerializeField] private CanvasGroup assemblerGameScreen;
    [SerializeField] private CanvasGroup resultsScreen;
    [SerializeField] private AssemblerGameManager assemblerGameManager;
    [SerializeField] private ResultScreenManager resultScreenManager;

    [SerializeField] private float transitionSpeed = 0.5f;
    [SerializeField] private float transitionDelay = 0.5f;
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

        SetCanvasGroup(requestScreen, true);
        SetCanvasGroup(assemblerScreen, false);
        SetCanvasGroup(assemblerGameScreen, false);
        SetCanvasGroup(resultsScreen, false);
    }
    #endregion

    #region Public Methods
    public void StartAssembling(RobotStatistics requestedRobotStatistics)
    {
        assemblerGameManager.ResetAssemblerGame();
        assemblerGameManager.SetRequestedStatistics(requestedRobotStatistics);
        TransitionBetweenScreens(requestScreen, assemblerScreen);
    }

    public void DisplayResultScreen(RobotResult results)
    {
        TransitionBetweenScreens(assemblerScreen, resultsScreen);
        resultScreenManager.SetResultStatistics(results);
    }

    public void ReturnToRequestScreen()
    {
        TransitionBetweenScreens(resultsScreen, requestScreen);
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