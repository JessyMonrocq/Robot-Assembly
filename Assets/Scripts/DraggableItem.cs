using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;

public class DraggableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Inspector Fields
    [Header("Draggable Item Settings")]
    [SerializeField] private Image itemImage;
    [SerializeField] private Outline itemOutline;
    [SerializeField] private float followSpeed = 0.1f;
    [SerializeField] private float destroySpeed = 0.05f;
    [SerializeField] private Ease destroyEase = Ease.Flash;
    [SerializeField] private bool destroyOnRelease;

    [Header("Robot Part Reference")]
    [SerializeField] private RobotPartsListSO robotPartsListSO;
    [SerializeField] private RobotPartSO assignedRobotPart;

    public static DraggableItem CurrentDraggedItem { get; private set; }

    public RobotPartSO.RobotPartType RobotPartType => assignedRobotPart.PartType;
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

        itemOutline.enabled = false;

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
        itemOutline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemOutline.enabled = false;
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
            CurrentDraggedItem = null;
            DestroyItem();
            return;
        }

        CurrentDraggedItem = null;

        socketDetected = false;
        transform.SetParent(startParent);
        itemImage.raycastTarget = true;
        rectTransform.DOKill();
        rectTransform.DOAnchorPos(Vector2.zero, followSpeed);
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
    }

    public void SetDraggable(bool value)
    {
        canDrag = value;
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
