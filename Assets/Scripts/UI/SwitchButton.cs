using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private RectTransform switchImage;

    private bool isOn;

    private void Awake()
    {
        isOn = false;
        switchImage.localRotation = new Quaternion(0, 0, 180, 0);
    }

    private void OnEnable()
    {
        button.onClick.AddListener(ButtonSwitch);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(ButtonSwitch);
    }

    private void ButtonSwitch()
    {
        isOn = !isOn;
        switchImage.localRotation = isOn ? new Quaternion(0, 0, 180, 0) : Quaternion.identity;
    }
}
