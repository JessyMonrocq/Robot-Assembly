using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;

    private SatisfactionLevel satisfactionLevel;

    public void AssignSatisfactionLevel(SatisfactionLevel sl)
    {
        satisfactionLevel = sl;

        image.sprite = satisfactionLevel.satisfactionIcon;
        image.color = satisfactionLevel.iconColor;
    }

    public void OnButtonHover(bool hovering)
    {
        if (satisfactionLevel.satisfactionIcon == null)
        {
            return;
        }

        text.gameObject.SetActive(!hovering);
        image.gameObject.SetActive(hovering);
    }
}
