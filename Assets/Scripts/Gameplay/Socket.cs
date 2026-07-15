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
    [SerializeField] private bool canReplace = true;

    public Func<GameObject, bool> CompatibilityCheck;

    public event Action<GameObject> OnSocketed;
    public event Action<GameObject> OnRemoved;

    public bool CanSocket
    {
        get { return canSocket; }
        set { canSocket = value; }
    }

    private RectTransform rectTransform;
    private GameObject socketedObject;
    private bool isHighlighted;
    private bool canSocket = true;
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

        if (currentDragged != null && canSocket && CompatibilityCheck(currentDragged))
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
        if (!canSocket)
        {
            return;
        }

        GameObject dropped = eventData.pointerDrag;

        if (dropped == null)
        {
            return;
        }

        if (!CompatibilityCheck(dropped))
        {
            return;
        }

        if (socketedObject != null && socketedObject != dropped && canReplace)
        {
            Destroy(socketedObject);
            socketedObject = null;
        }
        else if (socketedObject != null && socketedObject != dropped && !canReplace)
        {
            return;
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
            else
            {
                var drag = socketedObject.GetComponent<DragItem>();
                if (drag != null)
                {
                    drag.SetDraggable(true);
                }
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
            var drag = socketedObject.GetComponent<DragItem>();
            if (drag != null)
            {
                drag.SetDraggable(false);
                drag.SetCurrentParent(transform);

                var rt = socketedObject.GetComponent<RectTransform>();
                socketedObject.transform.SetParent(transform);
                socketedObject.transform.localScale = Vector3.one;

                var image = socketedObject.GetComponent<UnityEngine.UI.Image>();
                if (image != null) image.raycastTarget = true;

                yield return drag.Recenter().WaitForCompletion();

                drag.FinishSocketing(enableDragAfter: true);
            }
            else
            {
                var robotDraggable = socketedObject.GetComponent<RobotDraggable>();
                var rt = socketedObject.GetComponent<RectTransform>();
                socketedObject.transform.SetParent(transform);
                socketedObject.transform.localScale = Vector3.one;

                if (rt != null)
                {
                    rt.DOKill();
                    yield return rt.DOAnchorPos(Vector2.zero, 0.2f).SetEase(Ease.OutBack).WaitForCompletion();
                }
                else
                {
                    socketedObject.transform.localPosition = Vector3.zero;
                }
            }

            isHighlighted = false;
            yield return socketImage.rectTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).WaitForCompletion();
            OnSocketed?.Invoke(socketedObject);
        }
    }
    #endregion
}