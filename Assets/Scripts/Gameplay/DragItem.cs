using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using Coffee.UIEffects;

public class DragItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Inspector Fields
    [Header("Draggable Item Settings")]
    [SerializeField] private Image itemImage;
    [SerializeField] private UIEffect itemEffect;
    [SerializeField] private float followSpeed = 0.1f;
    [SerializeField] private float destroySpeed = 0.05f;
    [SerializeField] private Ease destroyEase = Ease.Flash;
    [SerializeField] private bool destroyOnRelease;
    [SerializeField] private bool centerOnRelease = true;

    public static DragItem CurrentDraggedItem { get; private set; }

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
        if (DragItem.CurrentDraggedItem != null || !canDrag)
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
        if (!canDrag)
        {
            return;
        }

        if (destroyOnRelease && !socketDetected)
        {
            Socket socket = startParent?.GetComponent<Socket>();
            if (socket != null)
            {
                socket.RemoveItem(destroy: false);
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

    public void SetDraggable(bool value)
    {
        canDrag = value;
    }

    public Tween Recenter()
    {
        rectTransform.DOKill();
        return rectTransform.DOAnchorPos(Vector2.zero, followSpeed).SetEase(Ease.OutBack);
    }

    public void FinishSocketing(bool enableDragAfter = true)
    {
        CurrentDraggedItem = null;

        if (itemEffect != null) itemEffect.shadowMode = ShadowMode.None;

        if (itemImage != null)
        {
            itemImage.raycastTarget = true;
            itemImage.canvasRenderer.SetAlpha(1f);

            itemImage.enabled = false;
            itemImage.enabled = true;
        }

        Canvas.ForceUpdateCanvases();
        if (parentCanvas != null)
        {
            var rt = parentCanvas.transform as RectTransform;
            if (rt != null) UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
        }

        canDrag = enableDragAfter;
        socketDetected = true;
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
