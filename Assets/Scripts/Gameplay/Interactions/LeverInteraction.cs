using UnityEngine;

public class LeverInteraction : MiniInteraction
{
    [SerializeField] private DraggableLever draggableLever;

    private bool leverComplete;

    private void OnEnable()
    {
        draggableLever.OnValueChanged.AddListener(UpdateLeverState);
    }

    private void OnDisable()
    {
        draggableLever.OnValueChanged.RemoveListener(UpdateLeverState);
    }

    public override void ResetInteraction()
    {
        leverComplete = false;

        draggableLever.SetCurrentValueImmediate(0f);
        draggableLever.CanInteract = false;
    }

    public override void InitializeInteraction()
    {
        draggableLever.CanInteract = true;
    }

    private void UpdateLeverState(float value)
    {
        if (value == 1f && !leverComplete)
        {
            leverComplete = true;
            draggableLever.CanInteract = false;

            InvokeOnInteractionCompleted();
        }
    }
}
