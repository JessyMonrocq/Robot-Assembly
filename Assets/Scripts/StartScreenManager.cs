using Coffee.UIEffects;
using DG.Tweening;
using EasyTextEffects;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    [Header("Start Screen Settings")]
    [SerializeField] private CanvasGroup screenCG;
    [SerializeField] private TextEffect gameTitleEffects;
    [SerializeField] private Button startButton;
    [SerializeField] private float startDelay;
    [SerializeField] private float buttonDelay;
    [SerializeField] private float buttonAnimationDuration;
    [SerializeField] private Ease buttonAnimationEase;
    [SerializeField] private float screenAnimationDelay;
    [SerializeField] private float screenAnimationDuration;
    [SerializeField] private float screenAnimationOffset;
    [SerializeField] private Ease screenAnimationEase;

    [Header("Screen Layout Settings")]
    [SerializeField] private RectTransform screenLayout;
    [SerializeField] private UIEffect screenLayoutEffect;
    [SerializeField] private float screenLayoutDelay;
    [SerializeField] private float screenLayoutDuration;
    [SerializeField] private float screenLayoutOffset;
    [SerializeField] private float transitionAlpha;
    [SerializeField] private Ease screenLayoutEase;

    public void ResetStartScreen()
    {
        screenCG.transform.DOLocalMoveY(0f, 0f);
        screenCG.alpha = 1f;
        screenCG.interactable = false;
        screenCG.blocksRaycasts = false;
        gameTitleEffects.Refresh();
        gameTitleEffects.transform.DOScale(0f, 0f);
        startButton.transform.DOScale(0f, 0f);

        screenLayout.DOScale(0.1f, 0f);
        screenLayout.DOLocalMoveY(screenLayoutOffset, 0f);
        screenLayoutEffect.transitionColorAlpha = 0f;
    }

    public void InitializeStartScreen()
    {
        StartCoroutine(StartMenuCoroutine());
    }

    public void HideStartScreen()
    {
        screenCG.interactable = false;
        screenCG.blocksRaycasts = false;
        StartCoroutine(HideStartScreenCoroutine());
    }

    private IEnumerator StartMenuCoroutine()
    {
        yield return new WaitForSeconds(startDelay);
        gameTitleEffects.StartManualEffects();
        gameTitleEffects.transform.DOScale(1f, 0f);
        yield return new WaitForSeconds(buttonDelay);
        screenCG.interactable = true;
        screenCG.blocksRaycasts = true;
        startButton.transform.DOScale(1f, buttonAnimationDuration).SetEase(buttonAnimationEase);
    }

    private IEnumerator HideStartScreenCoroutine()
    {
        yield return new WaitForSeconds(screenAnimationDelay);
        yield return screenCG.transform.DOLocalMoveY(screenAnimationOffset, screenAnimationDuration).SetEase(screenAnimationEase).WaitForCompletion();

        yield return new WaitForSeconds(screenLayoutDelay);
        yield return screenLayout.DOLocalMoveY(0f, screenLayoutDuration).SetEase(screenLayoutEase).WaitForCompletion();
        yield return new WaitForSeconds(screenLayoutDelay);
        yield return screenLayout.DOScale(1f, screenLayoutDuration).SetEase(screenLayoutEase).WaitForCompletion();
        yield return DOTween.To(() => screenLayoutEffect.transitionColorAlpha,
            x => screenLayoutEffect.transitionColorAlpha = x,
            transitionAlpha,
            screenLayoutDuration)
        .SetEase(screenLayoutEase).WaitForCompletion();
        yield return new WaitForSeconds(screenLayoutDelay);
        GameManager.Instance.ReturnToRequestScreen(screenCG);
    }
}
