using System;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    public event Action<bool> OnSwitch;

    [SerializeField] private Button button;
    [SerializeField] private RectTransform switchImage;

    private bool isOn;

    private void Awake()
    {
        ResetSwitchButton();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ButtonSwitch);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ButtonSwitch);
    }

    public void ResetSwitchButton()
    {
        isOn = false;
        switchImage.localScale = new Vector3(1, -1, 1);
    }

    public void CanInteract(bool state)
    {
        button.interactable = state;
    }

    private void ButtonSwitch()
    {
        isOn = !isOn;
        switchImage.localScale = isOn ? Vector3.zero : new Vector3(1, -1, 1);

        OnSwitch?.Invoke(isOn);
    }
}
