using DG.Tweening;
using System.Collections;
using UnityEngine;

public class AssemblerGameManager : MonoBehaviour
{
    #region Inspector Fields
    [Header("Assembler Game Manager References")]
    [SerializeField] private Assembler assembler;
    [SerializeField] private Specifications specifications;
    [SerializeField] private ConfirmBuildDisplay confirmBuildDisplay;
    [SerializeField] private ResultScreenManager resultScreenManager;
    [SerializeField] private CanvasGroup backgroundBlurCG;
    [SerializeField] private CanvasGroup assemblerScreenCG;
    [SerializeField] private CanvasGroup resultsScreenCG;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Ease animationEase = Ease.Linear;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        assembler.OnRobotStatisticsUpdated += specifications.UpdateSpecifications;
        confirmBuildDisplay.OnDisplayHidden += HideConfirmBuild;
    }

    private void OnDisable()
    {
        assembler.OnRobotStatisticsUpdated -= specifications.UpdateSpecifications;
        confirmBuildDisplay.OnDisplayHidden -= HideConfirmBuild;
    }

    private void Awake()
    {
        backgroundBlurCG.alpha = 0f;
        backgroundBlurCG.gameObject.SetActive(false);

        assemblerScreenCG.alpha = 1f;
        assemblerScreenCG.interactable = true;
        assemblerScreenCG.blocksRaycasts = true;

        resultsScreenCG.alpha = 0f;
        resultsScreenCG.interactable = false;
        resultsScreenCG.blocksRaycasts = false;
    }

    private void Start()
    {
        RobotStatistics initialStatistics = new RobotStatistics();
        specifications.UpdateSpecifications(initialStatistics);
    }
    #endregion

    #region Public Methods
    public void DisplayConfirmBuild()
    {
        backgroundBlurCG.gameObject.SetActive(true);
        backgroundBlurCG.DOFade(1, animationDuration).SetEase(animationEase).OnComplete(() =>
            confirmBuildDisplay.UpdateConfirmBuildDisplay(true)
        );
    }

    public void DisplayRobotResults()
    {
        StartCoroutine(DisplayResultScreenCoroutine());
    }
    #endregion

    #region Private Methods
    private void HideConfirmBuild()
    {
        backgroundBlurCG.DOFade(0, animationDuration).SetEase(animationEase).OnComplete(() =>
            backgroundBlurCG.gameObject.SetActive(false)
        );
    }
    #endregion

    #region Coroutine Methods
    private IEnumerator DisplayResultScreenCoroutine()
    {
        assemblerScreenCG.interactable = false;
        assemblerScreenCG.blocksRaycasts = false;
        yield return assemblerScreenCG.DOFade(0f, animationDuration).SetEase(animationEase).WaitForCompletion();
        yield return new WaitForSeconds(1f);
        yield return resultsScreenCG.DOFade(1f, animationDuration).SetEase(animationEase).WaitForCompletion();
        resultsScreenCG.interactable = true;
        resultsScreenCG.blocksRaycasts = true;
        yield return new WaitForSeconds(1f);
        resultScreenManager.SetResultStatistics(specifications.CreateRobotResult());
    }
    #endregion
}
