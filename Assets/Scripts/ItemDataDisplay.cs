using DG.Tweening;
using UnityEngine;

public class ItemDataDisplay : MonoBehaviour
{
    [Header("Item Data References")]
    [SerializeField] private Transform itemDataPanel;
    [SerializeField] private CanvasGroup itemDataPanelCG;
    [SerializeField] private StatisticsDisplay statisticsDisplay;
    [SerializeField] private float itemDataPanelPosOffset;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Ease animationEase = Ease.Linear;

    private bool isDisplayed;

    private void Awake()
    {
        InitializeItemDataDisplay();
    }

    private void OnEnable()
    {
        RobotDraggable.OnItemHover += UpdateItemDataDisplay;
    }

    private void OnDisable()
    {
        RobotDraggable.OnItemHover -= UpdateItemDataDisplay;
    }

    public void UpdateItemDataDisplay(RobotStatistics stats)
    {
        if (!isDisplayed)
        {
            itemDataPanel.DOLocalMoveX(0, animationDuration).SetEase(animationEase).OnComplete(() =>
                itemDataPanelCG.DOFade(1, animationDuration).SetEase(animationEase)
            );
            isDisplayed = true;
        }

        statisticsDisplay.SetStatistics(stats);
    }

    public void InitializeItemDataDisplay()
    {
        itemDataPanel.DOKill();
        itemDataPanelCG.DOKill();

        isDisplayed = false;
        itemDataPanelCG.alpha = 0f;
        itemDataPanel.localPosition = new Vector3(itemDataPanelPosOffset, 0, 0);

        statisticsDisplay.Clear();
    }
}
