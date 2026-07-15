using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Coffee.UIEffects;

public class BatteryDraggable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<string> OnBatteryHover;

    [Header("Battery Visuals")]
    [SerializeField] private string batteryId;
    [SerializeField] private Image itemImage;
    [SerializeField] private UIEffect itemEffect;

    public string BatteryId => batteryId;

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

        OnBatteryHover?.Invoke(batteryId);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemEffect != null)
        {
            itemEffect.edgeMode = EdgeMode.None;
        }
    }

    public void SetAssignedBattery(Sprite sprite, Color tint, string id)
    {
        batteryId = id;

        if (itemImage != null && sprite != null)
        {
            itemImage.sprite = sprite;
            itemImage.color = tint;
        }
    }
}