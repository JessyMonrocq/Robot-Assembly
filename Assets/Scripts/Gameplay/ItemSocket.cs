using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSocket : MonoBehaviour, IDropHandler
{
    #region Inspector Fields
    public event Action<RobotPartSO> OnItemSocketed;
    public event Action<RobotPartSO> OnItemRemoved;

    [SerializeField] private bool useSocketKey = true;
    [SerializeField] private Image socketImage;
    [SerializeField] private RobotPartSO.RobotPartType socketType;

    private RectTransform rectTransform;
    private DraggableItem socketedItem;
    private bool isHighlighted;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        socketedItem = null;
        isHighlighted = false;
    }

    private void Update()
    {
        if (DraggableItem.CurrentDraggedItem != null && DraggableItem.CurrentDraggedItem.RobotPartSO.PartType == socketType)
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
            if (item.RobotPartSO.PartType == socketType || !useSocketKey)
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
    #endregion

    #region Public Methods
    public void InitializeSocket()
    {
        if (socketedItem != null)
        {
            Destroy(socketedItem.gameObject);
        }

        socketedItem = null;
        isHighlighted = false;

        socketImage.rectTransform.DOKill();
        socketImage.rectTransform.localScale = Vector3.one;
    }

    public void RemoveItem(bool destroy)
    {
        if (socketedItem != null)
        {
            OnItemRemoved?.Invoke(socketedItem.RobotPartSO);
            if (destroy)
            {
                Destroy(socketedItem.gameObject);
            }
            socketedItem = null;
        }
    }
    #endregion

    #region Coroutine Methods
    private IEnumerator SocketItemCoroutine()
    {
        socketedItem.SetDraggable(false);
        socketedItem.SetCurrentParent(transform);
        isHighlighted = false;
        yield return socketImage.rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).WaitForCompletion();
        OnItemSocketed?.Invoke(socketedItem.RobotPartSO);
        socketedItem.SetDraggable(true);
    }
    #endregion
}
