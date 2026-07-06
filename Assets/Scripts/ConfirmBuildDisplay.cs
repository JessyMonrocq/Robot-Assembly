using DG.Tweening;
using System;
using UnityEngine;

public class ConfirmBuildDisplay : MonoBehaviour
{
    public event Action OnDisplayHidden;

    [Header("Confirm Build Display References")]
    [SerializeField] private RectTransform confirmBuildPanel;
    [SerializeField] private CanvasGroup confirmBuildCG;
    [SerializeField] private float confirmBuildPosOffset;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private Ease animationEase = Ease.Linear;

    private void Awake()
    {
        InitializeConfirmBuildDisplay();
    }

    public void UpdateConfirmBuildDisplay(bool display)
    {
        if (display)
        {
            confirmBuildPanel.DOLocalMoveX(0, animationDuration).SetEase(animationEase).OnComplete(() =>
                confirmBuildCG.DOFade(1, animationDuration).SetEase(animationEase).OnComplete(() =>
                    confirmBuildCG.interactable = true
                )
            );
        }
        else
        {
            confirmBuildCG.interactable = false;
            confirmBuildCG.DOFade(0, animationDuration).SetEase(animationEase).OnComplete(() =>
                confirmBuildPanel.DOLocalMoveX(confirmBuildPosOffset, animationDuration).SetEase(animationEase).OnComplete(() =>
                    OnDisplayHidden?.Invoke()
                )
            );
        }

    }

    private void InitializeConfirmBuildDisplay()
    {
        confirmBuildPanel.transform.localPosition = new Vector3(confirmBuildPosOffset, 0, 0);
        confirmBuildCG.alpha = 0f;
        confirmBuildCG.interactable = false;
    }
}
