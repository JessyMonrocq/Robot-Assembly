using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSocket : MonoBehaviour, IDropHandler
{
    [SerializeField] private bool useSocketKey = true;
    [SerializeField] private Image socketImage;
    [SerializeField] private RobotPartSO.RobotPartType socketType;

    private RectTransform rectTransform;
    private DraggableItem socketedItem;
    private bool isHighlighted;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        socketedItem = null;
        isHighlighted = false;
    }

    private void Update()
    {
        if (DraggableItem.CurrentDraggedItem != null && DraggableItem.CurrentDraggedItem.RobotPartType == socketType)
        {
            if (RectOverlap.IsOverlapping(DraggableItem.CurrentDraggedItem.RectTransform, rectTransform) && !isHighlighted)
            {
                isHighlighted = true;
                socketImage.rectTransform.DOKill();
                socketImage.rectTransform.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutBack);
            }
            else if (!RectOverlap.IsOverlapping(DraggableItem.CurrentDraggedItem.RectTransform, rectTransform) && isHighlighted)
            {
                isHighlighted = false;
                socketImage.rectTransform.DOKill();
                socketImage.rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
            }
        }
        else if (isHighlighted)
        {
            isHighlighted = false;
            socketImage.rectTransform.DOKill();
            socketImage.rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        if (droppedItem.TryGetComponent<DraggableItem>(out DraggableItem item))
        {
            if (item.RobotPartType == socketType || !useSocketKey)
            {
                if (socketedItem != null && socketedItem != item)
                {
                    Destroy(socketedItem.gameObject);
                    socketedItem = null;
                }

                socketedItem = item;
                StartCoroutine(SocketItemCoroutine());
            }
        }
    }

    private IEnumerator SocketItemCoroutine()
    {
        socketedItem.SetDraggable(false);
        socketedItem.SetCurrentParent(transform);
        isHighlighted = false;
        yield return socketImage.rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).WaitForCompletion();
        socketedItem.SetDraggable(true);
    }
}
