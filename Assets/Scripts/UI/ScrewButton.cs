using DG.Tweening;
using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrewButton : MonoBehaviour
{
    public event Action OnUnscrewed;

    [SerializeField] private Button button;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] [Range(1, 10)] private int rotationAmount;
    [SerializeField] private float rotatingSpeed;
    [SerializeField] private float rotatingDelay;
    [SerializeField] private Ease rotatingEase;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private float detachForce;

    private Vector3 originPos;

    private void Awake()
    {
        rb.simulated = false;
        button.image.SetAlpha(1f);

        originPos = transform.localPosition;
    }

    private void OnEnable()
    {
        button.onClick.AddListener(Unscrew);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Unscrew);
    }

    public void ResetScrewButton()
    {
        rb.simulated = false;
        button.interactable = true;
        button.image.SetAlpha(1f);
        transform.localPosition = originPos;
        transform.localRotation = Quaternion.identity;
    }

    private void Unscrew()
    {
        button.interactable = false;
        Vector3 rot = new Vector3(0f, 0f, rotationAmount * 360f);
        rectTransform.DORotate(rot, rotatingSpeed, RotateMode.FastBeyond360).SetEase(rotatingEase).OnComplete(() =>
            StartCoroutine(FallAndDieCoroutine())
        );
    }

    private IEnumerator FallAndDieCoroutine()
    {
        yield return new WaitForSeconds(rotatingDelay);
        rb.simulated = true;
        rb.AddForce(new Vector2(detachForce, 0), ForceMode2D.Impulse);
        rb.AddTorque(-detachForce * 10f, ForceMode2D.Impulse);

        OnUnscrewed?.Invoke();

        yield return new WaitForSeconds(timeToDestroy);
        rb.simulated = false;
        button.image.SetAlpha(0f);
    }
}
