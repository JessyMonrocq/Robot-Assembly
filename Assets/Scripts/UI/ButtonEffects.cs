using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float clickScale = 0.85f;
    [SerializeField] private float hoverScale = 1.025f;
    [SerializeField] private float animDuration = 0.1f;
    [SerializeField] private Ease animEase = Ease.InOutQuad;

    private void Start()
    {
        transform.localScale = Vector3.one;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOScale(clickScale, animDuration).SetEase(animEase);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1f, animDuration).SetEase(animEase);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(hoverScale, animDuration).SetEase(animEase);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1f, animDuration).SetEase(animEase);
    }
}
