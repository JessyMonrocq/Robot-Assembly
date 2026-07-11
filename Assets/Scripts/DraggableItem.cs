using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System;
using Coffee.UIEffects;

public class DraggableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Inspector Fields
    public static event Action<RobotStatistics> OnItemHover;

    [Header("Draggable Item Settings")]
    [SerializeField] private Image itemImage;
    [SerializeField] private UIEffect itemEffect;
    [SerializeField] private float followSpeed = 0.1f;
    [SerializeField] private float destroySpeed = 0.05f;
    [SerializeField] private Ease destroyEase = Ease.Flash;
    [SerializeField] private bool destroyOnRelease;
    [SerializeField] private bool centerOnRelease = true;

    [Header("Robot Part Reference")]
    [SerializeField] private RobotPartsListSO robotPartsListSO;
    [SerializeField] private RobotPartSO assignedRobotPart;

    public static DraggableItem CurrentDraggedItem { get; private set; }

    public RobotPartSO RobotPartSO => assignedRobotPart;
    public RectTransform RectTransform => rectTransform;

    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Camera mainCamera;
    private Transform startParent;

    private bool socketDetected;
    private bool canDrag;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();

        itemEffect.edgeMode = EdgeMode.None;
        itemEffect.shadowMode = ShadowMode.None;

        socketDetected = false;
        canDrag = true;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }
    #endregion

    #region Event Methods
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DraggableItem.CurrentDraggedItem != null || !canDrag)
        {
            return;
        }
        itemEffect.edgeMode = EdgeMode.Plain;
        OnItemHover?.Invoke(assignedRobotPart.Stats);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemEffect.edgeMode = EdgeMode.None;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }

        startParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        itemImage.raycastTarget = false;

        itemEffect.shadowMode = ShadowMode.Shadow;

        CurrentDraggedItem = this;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag)
        {
            return;
        }

        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, eventData.position, mainCamera, out localPoint))
        {
            rectTransform.DOAnchorPos(localPoint, followSpeed);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (destroyOnRelease && !socketDetected)
        {
            ItemSocket socket = startParent?.GetComponent<ItemSocket>();
            if (socket != null)
            {
                socket.RemoveItem(destroy:false);
            }

            CurrentDraggedItem = null;
            DestroyItem();
            return;
        }

        itemEffect.shadowMode = ShadowMode.None;

        CurrentDraggedItem = null;

        socketDetected = false;
        transform.SetParent(startParent);
        itemImage.raycastTarget = true;
        rectTransform.DOKill();
        if (centerOnRelease)
        {
            rectTransform.DOAnchorPos(Vector2.zero, followSpeed);
        }
    }
    #endregion

    #region Public Methods
    public void SetCurrentParent(Transform parent)
    {
        startParent = parent;
        socketDetected = true;
    }

    public void SetAssignedRobotPart(RobotPartSO robotPart)
    {
        assignedRobotPart = robotPart;
        itemImage.sprite = robotPart.PartImage;
        itemImage.color = robotPart.ImageTint;
    }

    public void SetDraggable(bool value)
    {
        canDrag = value;
    }

    public void Recenter()
    {
        rectTransform.DOKill();
        rectTransform.DOAnchorPos(Vector2.zero, followSpeed);
    }
    #endregion

    #region Private Methods
    private void DestroyItem()
    {
        canDrag = false;
        StartCoroutine(DestroyItemCoroutine());
    }
    #endregion

    #region Coroutine Methods
    private IEnumerator DestroyItemCoroutine()
    {
        yield return rectTransform.DOScale(0f, destroySpeed).SetEase(destroyEase).WaitForCompletion();
        Destroy(gameObject);
    }
    #endregion
}
