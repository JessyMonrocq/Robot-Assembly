using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Socket : MonoBehaviour, IDropHandler
{
    #region Inspector Fields
    [SerializeField] private Image socketImage;

    public Func<GameObject, bool> CompatibilityCheck;

    public event Action<GameObject> OnSocketed;
    public event Action<GameObject> OnRemoved;

    private RectTransform rectTransform;
    private GameObject socketedObject;
    private bool isHighlighted;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        socketedObject = null;
        isHighlighted = false;

        if (CompatibilityCheck == null)
        {
            CompatibilityCheck = _ => true;
        }
    }

    private void Update()
    {
        GameObject currentDragged = null;

        if (typeof(DragItem) != null && DragItem.CurrentDraggedItem != null)
        {
            currentDragged = DragItem.CurrentDraggedItem.gameObject;
        }
        else if (typeof(DragItem) != null && DragItem.CurrentDraggedItem != null)
        {
            currentDragged = DragItem.CurrentDraggedItem.gameObject;
        }

        if (currentDragged != null && CompatibilityCheck(currentDragged))
        {
            var draggedRect = currentDragged.GetComponent<RectTransform>();
            if (draggedRect != null && RectOverlap.IsOverlapping(draggedRect, rectTransform) && !isHighlighted)
            {
                isHighlighted = true;
                socketImage.rectTransform.DOKill();
                socketImage.rectTransform.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutBack);
            }
            else if ((draggedRect == null || !RectOverlap.IsOverlapping(draggedRect, rectTransform)) && isHighlighted)
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
        GameObject dropped = eventData.pointerDrag;

        if (dropped == null)
        {
            return;
        }

        if (!CompatibilityCheck(dropped))
        {
            return;
        }

        if (socketedObject != null && socketedObject != dropped)
        {
            Destroy(socketedObject);
            socketedObject = null;
        }

        socketedObject = dropped;
        StartCoroutine(SocketItemCoroutine());
    }
    #endregion

    #region Public Methods
    public void InitializeSocket()
    {
        if (socketedObject != null)
        {
            Destroy(socketedObject);
        }

        socketedObject = null;
        isHighlighted = false;

        socketImage.rectTransform.DOKill();
        socketImage.rectTransform.localScale = Vector3.one;
    }

    public void RemoveItem(bool destroy)
    {
        if (socketedObject != null)
        {
            OnRemoved?.Invoke(socketedObject);
            if (destroy)
            {
                Destroy(socketedObject);
            }
            socketedObject = null;
        }
    }
    #endregion

    #region Coroutine Methods
    private IEnumerator SocketItemCoroutine()
    {
        if (socketedObject != null)
        {
            var dragg = socketedObject.GetComponent<DragItem>();
            if (dragg != null)
            {
                dragg.SetDraggable(false);
                dragg.SetCurrentParent(transform);
            }
            else
            {
                var draggable = socketedObject.GetComponent<DragItem>();
                if (draggable != null)
                {
                    draggable.SetDraggable(false);
                    draggable.SetCurrentParent(transform);
                }
            }

            isHighlighted = false;
            yield return socketImage.rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).WaitForCompletion();
            OnSocketed?.Invoke(socketedObject);

            if (dragg != null)
            {
                dragg.SetDraggable(true);
            }
            else if (socketedObject.TryGetComponent<DragItem>(out var d))
            {
                d.SetDraggable(true);
            }
        }
    }
    #endregion
}