using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSocket : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool useSocketKey = true;
    [SerializeField] private SocketKey socketKey;
    [SerializeField] private Image socketImage;

    private RectTransform rectTransform;
    private bool isHighlighted;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        isHighlighted = false;
    }

    private void Update()
    {
        if (DraggableItem.CurrentDraggedItem != null)
        {
            if (RectOverlap.IsOverlapping(DraggableItem.CurrentDraggedItem, rectTransform) && !isHighlighted)
            {
                isHighlighted = true;
                socketImage.rectTransform.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutBack);
            }
            else if (!RectOverlap.IsOverlapping(DraggableItem.CurrentDraggedItem, rectTransform) && isHighlighted)
            {
                isHighlighted = false;
                socketImage.rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        if (droppedItem.TryGetComponent<DraggableItem>(out DraggableItem item))
        {
            if (item.Socket.socketType == socketKey.socketType || !useSocketKey)
            {
                item.SetStartParent(transform);
                if (isHighlighted)
                {
                    isHighlighted = false;
                    socketImage.rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
                }
            }
        }
    }
}
