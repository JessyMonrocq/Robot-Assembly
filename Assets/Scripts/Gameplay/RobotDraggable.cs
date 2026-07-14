using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Coffee.UIEffects;

public class RobotDraggable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<RobotStatistics> OnItemHover;

    [Header("Robot Part Reference")]
    [SerializeField] private RobotPartsListSO robotPartsListSO;
    [SerializeField] private RobotPartSO assignedRobotPart;

    [Header("Robot Part Visuals")]
    [SerializeField] private Image itemImage;
    [SerializeField] private UIEffect itemEffect;

    public RobotPartSO RobotPartSO => assignedRobotPart;

    private void Awake()
    {
        if (itemEffect != null)
        {
            itemEffect.edgeMode = EdgeMode.None;
            itemEffect.shadowMode = ShadowMode.None;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DragItem.CurrentDraggedItem != null)
        {
            return;
        }

        if (itemEffect != null)
        {
            itemEffect.edgeMode = EdgeMode.Plain;
        }

        if (assignedRobotPart != null)
        {
            OnItemHover?.Invoke(assignedRobotPart.Stats);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemEffect != null)
        {
            itemEffect.edgeMode = EdgeMode.None;
        }
    }

    public void SetAssignedRobotPart(RobotPartSO robotPart)
    {
        assignedRobotPart = robotPart;
        if (robotPart == null)
        {
            return;
        }

        if (itemImage != null)
        {
            itemImage.sprite = robotPart.PartImage;
            itemImage.color = robotPart.ImageTint;
        }
    }
}