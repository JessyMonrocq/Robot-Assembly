using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections;

public class DraggableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Inspector Fields
    [Header("Draggable Item Settings")]
    [SerializeField] private Image itemImage;
    [SerializeField] private float followSpeed = 0.1f;
    [SerializeField] private float destroySpeed = 0.05f;
    [SerializeField] private Ease destroyEase = Ease.Flash;
    [SerializeField] private bool destroyOnRelease;
    [SerializeField] private SocketKey socketKey;

    public SocketKey Socket => socketKey;

    public static RectTransform CurrentDraggedItem { get; private set; }

    private RectTransform rectTransform;
    private Canvas parentCanvas;
    private Camera mainCamera;
    private Transform startParent;

    private bool socketDetected;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();

        socketDetected = false;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }
    #endregion

    #region Event Methods
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemImage.DOKill();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        itemImage.raycastTarget = false;

        CurrentDraggedItem = rectTransform;
    }

    public void OnDrag(PointerEventData eventData)
    {
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
    public void SetStartParent(Transform parent)
    {
        startParent = parent;
        socketDetected = true;
    }
    #endregion

    #region Private Methods
    private void DestroyItem()
    {
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
