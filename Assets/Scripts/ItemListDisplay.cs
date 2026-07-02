using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ItemCategoryToggle
{
    public Toggle toggle;
    public RobotPartSO.RobotPartType robotPartType;
}

public class ItemListDisplay : MonoBehaviour
{
    [SerializeField] private ItemSelector itemSelectorPrefab;
    [SerializeField] private Transform itemListContentParent;
    [SerializeField] private CanvasGroup itemListCanvasGroup;

    [SerializeField] private RobotPartsListSO robotPartsListSO;
    [SerializeField] private ItemCategoryToggle[] itemCategoryToggleGroup;

    private RobotPartSO.RobotPartType currentRobotPartType;

    private void Awake()
    {
        currentRobotPartType = 0;
        itemCategoryToggleGroup[0].toggle.isOn = true;
        SetItemDisplay(currentRobotPartType);
    }

    private void OnEnable()
    {
        foreach (ItemCategoryToggle itemCategoryToggle in itemCategoryToggleGroup)
        {
            itemCategoryToggle.toggle.onValueChanged.AddListener(OnItemCategoryToggleChanged);
        }
    }

    private void OnDisable()
    {
        foreach (ItemCategoryToggle itemCategoryToggle in itemCategoryToggleGroup)
        {
            itemCategoryToggle.toggle.onValueChanged.RemoveListener(OnItemCategoryToggleChanged);
        }
    }

    private void OnItemCategoryToggleChanged(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < itemCategoryToggleGroup.Length; i++)
            {
                if (itemCategoryToggleGroup[i].toggle.isOn)
                {
                    currentRobotPartType = itemCategoryToggleGroup[i].robotPartType;
                    SetItemDisplay(currentRobotPartType);
                    break;
                }
            }
        }
    }

    public void SetItemDisplay(RobotPartSO.RobotPartType robotPartType)
    {
        foreach (Transform item in itemListContentParent)
        {
            Destroy(item.gameObject);
        }

        foreach (RobotPartSO robotPart in robotPartsListSO.RobotPartsList)
        {
            if (robotPart.PartType == currentRobotPartType)
            {
                ItemSelector newItemSelector = Instantiate(itemSelectorPrefab, itemListContentParent);
                newItemSelector.SetAssignedRobotPart(robotPart);
            }
        }
    }
}
