using UnityEngine;
using UnityEngine.UI;

public class OutlineHandler : MonoBehaviour
{
    [SerializeField] private Outline outline;
    [SerializeField] private bool useOutline;
    [SerializeField] private bool keepOnSelected;

    private bool isSelected;

    private void Awake()
    {
        outline.enabled = false;
        isSelected = false;
    }

    public void SetOutline(bool state)
    {
        if (outline == null || !useOutline)
        {
            return;
        }

        if (isSelected && keepOnSelected)
        {
            return;
        }

        outline.enabled = state;
    }

    public void SetSelected(bool state)
    {
        if (keepOnSelected)
        {
            isSelected = state;
            outline.enabled = state;
        }
    }
}
