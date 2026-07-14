using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class DraggableLever : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UnityEvent<float> OnValueChanged;

    [Header("Draggable Lever References")]
    [SerializeField] private RectTransform track;
    [SerializeField] private RectTransform handle;

    [Header("Draggable Lever Settings")]
    [SerializeField] private bool canInteract = true;
    [SerializeField] private bool topToBottom = false;
    [SerializeField] private float followSpeed = 0.5f;
    [SerializeField] private Ease followEase = Ease.OutQuad;
    [SerializeField] private bool snapToPointerOnDown = true;
    [SerializeField] private bool resetToZero = false;
    [SerializeField] private float releaseSpeed = 0.2f;
    [SerializeField] private Ease releaseEase = Ease.OutQuad;
    [SerializeField, Range(0.5f, 1f)] private float snapToOneThreshold = 0.98f;

    [Header("Slider Value")]
    [SerializeField, Range(0f, 1f)] private float initialValue = 0f;

    public bool CanInteract
    {
        get { return canInteract; }
        set { canInteract = value; }
    }

    private Canvas parentCanvas;
    private Camera eventCamera;
    private RectTransform rectTransform;

    private float currentValue;
    private float desiredValue;
    private Tween valueTween;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        currentValue = Mathf.Clamp01(initialValue);
        desiredValue = currentValue;
    }

    private void Start()
    {
        if (parentCanvas != null && parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            eventCamera = parentCanvas.worldCamera ?? Camera.main;
        }
        else
        {
            eventCamera = null;
        }

        ApplyValueToHandleImmediate();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (snapToPointerOnDown)
        {
            UpdateDesiredFromPointer(eventData);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canInteract)
        {
            return;
        }

        handle.SetAsLastSibling();
        UpdateDesiredFromPointer(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canInteract)
        {
            return;
        }

        UpdateDesiredFromPointer(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canInteract)
        {
            return;
        }

        if (resetToZero && currentValue < snapToOneThreshold)
        {
            SetDesiredValue(0f, true);
        }
        else if (currentValue >= snapToOneThreshold)
        {
            SetDesiredValue(1, true);
        }
    }

    private void UpdateDesiredFromPointer(PointerEventData eventData)
    {
        if (track == null || handle == null)
        {
            return;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(track, eventData.position, eventCamera, out Vector2 localPoint))
        {
            Vector2 halfSize = track.rect.size * 0.5f;
            bool isHorizontal = track.rect.width >= track.rect.height;

            if (isHorizontal)
            {
                float clamped = Mathf.Clamp(localPoint.x, -halfSize.x, halfSize.x);
                float normalized = (clamped + halfSize.x) / track.rect.width;
                SetDesiredValue(normalized, false);
            }
            else
            {
                float clamped = Mathf.Clamp(localPoint.y, -halfSize.y, halfSize.y);
                float normalized = (clamped + halfSize.y) / track.rect.height;

                if (topToBottom) normalized = 1f - normalized;
                SetDesiredValue(normalized, false);
            }
        }
    }

    public void SetDesiredValue(float normalizedDesired, bool released)
    {
        desiredValue = Mathf.Clamp01(normalizedDesired);
        AnimateCurrentTowardDesired(released);
    }

    private void AnimateCurrentTowardDesired(bool released)
    {
        valueTween?.Kill();

        float duration = released ? releaseSpeed : followSpeed;
        Ease ease = released ? releaseEase : followEase;

        valueTween = DOTween.To(() => currentValue, v =>
        {
            currentValue = v;
            UpdateHandlePositionFromValue();
            OnValueChanged?.Invoke(currentValue);
        }, desiredValue, duration).SetEase(ease).OnComplete(() =>
        {
            currentValue = desiredValue;
            UpdateHandlePositionFromValue();
            OnValueChanged?.Invoke(currentValue);
            valueTween = null;
        });
    }

    private void ApplyValueToHandleImmediate()
    {
        UpdateHandlePositionFromValue();
        OnValueChanged?.Invoke(currentValue);
    }

    private void UpdateHandlePositionFromValue()
    {
        if (track == null || handle == null) return;

        Vector2 half = track.rect.size * 0.5f;
        bool isHorizontal = track.rect.width >= track.rect.height;

        if (isHorizontal)
        {
            float x = Mathf.Lerp(-half.x, half.x, currentValue);
            Vector3 local = new Vector3(x, handle.localPosition.y, handle.localPosition.z);
            if (handle.parent == track)
            {
                handle.localPosition = local;
            }
            else
            {
                handle.position = track.TransformPoint(local);
            }
        }
        else
        {
            float mappedValue = topToBottom ? 1f - currentValue : currentValue;
            float y = Mathf.Lerp(-half.y, half.y, mappedValue);
            Vector3 local = new Vector3(handle.localPosition.x, y, handle.localPosition.z);
            if (handle.parent == track)
            {
                handle.localPosition = local;
            }
            else
            {
                handle.position = track.TransformPoint(local);
            }
        }
    }

    public void SetCurrentValueImmediate(float normalized)
    {
        currentValue = Mathf.Clamp01(normalized);
        desiredValue = currentValue;
        valueTween?.Kill();
        UpdateHandlePositionFromValue();
        OnValueChanged?.Invoke(currentValue);
    }
}