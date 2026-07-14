using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AssemblerGameManager : MonoBehaviour
{
    #region Inspector Fields
    [Header("Assembler Game Manager References")]
    [SerializeField] private Assembler assembler;
    [SerializeField] private Specifications specifications;
    [SerializeField] private ConfirmBuildDisplay confirmBuildDisplay;
    [SerializeField] private ResultScreenManager resultScreenManager;
    [SerializeField] private ItemDataDisplay itemDataDisplay;
    [SerializeField] private Toggle itemListToggle;
    [SerializeField] private ChronoDisplay chronoDisplay;
    [SerializeField] private CanvasGroup backgroundBlurCG;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Ease animationEase = Ease.Linear;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        assembler.OnRobotStatisticsUpdated += specifications.UpdateSpecifications;
        confirmBuildDisplay.OnDisplayHidden += HideConfirmBuild;
        chronoDisplay.OnChronoEnded += InterruptGame;
    }

    private void OnDisable()
    {
        assembler.OnRobotStatisticsUpdated -= specifications.UpdateSpecifications;
        confirmBuildDisplay.OnDisplayHidden -= HideConfirmBuild;
        chronoDisplay.OnChronoEnded -= InterruptGame;
    }

    private void Awake()
    {
        backgroundBlurCG.alpha = 0f;
        backgroundBlurCG.gameObject.SetActive(false);
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
        chronoDisplay.ResetChrono();
        GameManager.Instance.DisplayResultScreen(specifications.CreateRobotResult(), true);
    }

    public void SetRequestedStatistics(RobotStatistics statistics)
    {
        specifications.InitializeSpecifications(statistics);
    }

    public void ResetAssemblerGame(ChronoFormat chrono, float chronoDelay)
    {
        backgroundBlurCG.alpha = 0f;
        backgroundBlurCG.gameObject.SetActive(false);

        itemDataDisplay.InitializeItemDataDisplay();
        itemListToggle.Select();
        confirmBuildDisplay.InitializeConfirmBuildDisplay();
        specifications.InitializeSpecifications(new RobotStatistics());
        assembler.ResetAssembler();

        chronoDisplay.InitializeChrono(chrono, chronoDelay);
    }

    public void InterruptGame()
    {
        GameManager.Instance.DisplayResultScreen(null, false);
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
}
