using UnityEngine;
using UnityEngine.UI;

public class CaptchaToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private Image image;
    [SerializeField] private float selectedSize = 0.95f;

    public Toggle Toggle { get { return toggle; } }

    public void SetToggle(Sprite sprite, bool isOn)
    {
        image.sprite = sprite;
        toggle.isOn = isOn;
    }

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(ToggleSize);
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveListener(ToggleSize);
    }

    private void ToggleSize(bool isOn)
    {
        transform.localScale = isOn ? new Vector3(selectedSize, selectedSize, selectedSize) : Vector3.one;
    }
}
