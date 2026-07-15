using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIEffects;
using Unity.VisualScripting;

public class DraggableBattery : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Inspector Fields
    [Header("Draggable Battery Settings")]
    [SerializeField] private Image itemImage;
    [SerializeField] private UIEffect itemEffect;
    [SerializeField] private float followSpeed = 0.1f;

    public static DraggableBattery CurrentDraggedBattery { get; private set; }

    public RectTransform RectTransform => rectTransform;
    public bool CanDrag => canDrag;


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
        if (DraggableBattery.CurrentDraggedBattery != null || !canDrag)
        {
            return;
        }
        itemEffect.edgeMode = EdgeMode.Plain;
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

        CurrentDraggedBattery = this;
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
        if (!canDrag)
        {
            return;
        }

        canDrag = false;

        itemEffect.shadowMode = ShadowMode.None;

        CurrentDraggedBattery = null;

        transform.SetParent(startParent);
        itemImage.raycastTarget = true;
        rectTransform.DOKill();
        rectTransform.DOAnchorPos(Vector2.zero, followSpeed).OnComplete(() =>
            canDrag = !socketDetected
        );
    }
    #endregion

    #region Public Methods
    public void SetCurrentParent(Transform parent)
    {
        startParent = parent;
        socketDetected = true;
    }

    public void SetDraggable(bool value)
    {
        canDrag = value;
    }
    #endregion
}
