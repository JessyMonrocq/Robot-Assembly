using DG.Tweening;
using UnityEngine;

public class AssemblerGameManager : MonoBehaviour
{
    [Header("Assembler Game Manager References")]
    [SerializeField] private Assembler assembler;
    [SerializeField] private Specifications specifications;
    [SerializeField] private ConfirmBuildDisplay confirmBuildDisplay;
    [SerializeField] private CanvasGroup backgroundBlurCG;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private Ease animationEase = Ease.Linear;

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
    }

    private void Start()
    {
        RobotStatistics initialStatistics = new RobotStatistics();
        specifications.UpdateSpecifications(initialStatistics);
    }

    public void DisplayConfirmBuild()
    {
        backgroundBlurCG.gameObject.SetActive(true);
        backgroundBlurCG.DOFade(1, animationDuration).SetEase(animationEase).OnComplete(() =>
            confirmBuildDisplay.UpdateConfirmBuildDisplay(true)
        );
    }

    private void HideConfirmBuild()
    {
        backgroundBlurCG.DOFade(0, animationDuration).SetEase(animationEase).OnComplete(() =>
            backgroundBlurCG.gameObject.SetActive(false)
        );
    }
}
