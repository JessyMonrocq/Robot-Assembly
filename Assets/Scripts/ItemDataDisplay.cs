using DG.Tweening;
using TMPro;
using UnityEngine;

public class ItemDataDisplay : MonoBehaviour
{
    [Header("Item Data References")]
    [SerializeField] private Transform itemDataPanel;
    [SerializeField] private CanvasGroup itemDataPanelCG;
    [SerializeField] private float itemDataPanelPosOffset;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Ease animationEase = Ease.Linear;
    [SerializeField] private TextMeshProUGUI armorDataText;
    [SerializeField] private TextMeshProUGUI mobilityDataText;
    [SerializeField] private TextMeshProUGUI strengthDataText;
    [SerializeField] private TextMeshProUGUI computingDataText;
    [SerializeField] private TextMeshProUGUI energyDataText;
    [SerializeField] private TextMeshProUGUI weightDataText;

    private bool isDisplayed;

    private void Awake()
    {
        InitializeItemDataDisplay();
    }

    private void OnEnable()
    {
        DraggableItem.OnItemHover += UpdateItemDataDisplay;
    }

    private void OnDisable()
    {
        DraggableItem.OnItemHover -= UpdateItemDataDisplay;
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

        armorDataText.text = stats.Armor.ToString();
        mobilityDataText.text = stats.Mobility.ToString();
        strengthDataText.text = stats.Strength.ToString();
        computingDataText.text = stats.Computing.ToString();
        energyDataText.text = stats.Energy.ToString();
        weightDataText.text = stats.Weight.ToString();
    }

    private void InitializeItemDataDisplay()
    {
        isDisplayed = false;
        itemDataPanelCG.alpha = 0f;
        itemDataPanel.localPosition = new Vector3(itemDataPanelPosOffset, 0, 0);

        armorDataText.text = "0";
        mobilityDataText.text = "0";
        strengthDataText.text = "0";
        computingDataText.text = "0";
        energyDataText.text = "0";
        weightDataText.text = "0";
    }
}
