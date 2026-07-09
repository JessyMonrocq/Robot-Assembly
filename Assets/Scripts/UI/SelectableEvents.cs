using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableEvents : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event Action<bool> Pointer;
    public event Action<bool> Select;
    public event Action Click;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Pointer?.Invoke(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Pointer?.Invoke(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select?.Invoke(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Select?.Invoke(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click?.Invoke();
    }
}
