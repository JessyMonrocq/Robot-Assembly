using UnityEngine;

public class BatteryInteraction : MiniInteraction
{
    [SerializeField] private ScrewButton[] batteryPanelScrews;
    [SerializeField] private BatterySocket[] batterySockets;
    [SerializeField] private Transform[] batteryParents;
    [SerializeField] private DraggableBattery batteryItemPrefab;
    [SerializeField] private DraggableLever batteryPanelSlider;

    private int screwCount;
    private int batteryCount;

    private void OnEnable()
    {
        foreach (ScrewButton screw in batteryPanelScrews)
        {
            screw.OnUnscrewed += UpdateScrewCount;
        }

        batteryPanelSlider.OnValueChanged.AddListener(UpdatePanelSliderState);
    }

    private void OnDisable()
    {
        foreach (ScrewButton screw in batteryPanelScrews)
        {
            screw.OnUnscrewed -= UpdateScrewCount;
        }

        batteryPanelSlider.OnValueChanged.RemoveListener(UpdatePanelSliderState);
    }

    public override void ResetInteraction()
    {
        screwCount = 0;
        batteryCount = 0;

        foreach (ScrewButton screw in batteryPanelScrews)
        {
            screw.ResetScrewButton();
        }

        foreach (BatterySocket socket in batterySockets)
        {
            socket.ResetSocket();
            socket.OnBatteryInserted -= UpdateBatteryAdded;
            socket.enabled = false;
        }

        foreach (Transform parent in batteryParents)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }

            DraggableBattery battery = Instantiate(batteryItemPrefab, parent);
            battery.transform.localPosition = Vector3.zero;
        }

        batteryPanelSlider.SetCurrentValueImmediate(0f);
        batteryPanelSlider.CanInteract = false;
    }

    private void UpdateScrewCount()
    {
        screwCount++;

        if (screwCount == batteryPanelScrews.Length)
        {
            batteryPanelSlider.CanInteract = true;
        }
    }

    private void UpdatePanelSliderState(float value)
    {
        if (value == 1f && batteryPanelSlider.CanInteract)
        {
            batteryPanelSlider.CanInteract = false;

            foreach (BatterySocket socket in batterySockets)
            {
                socket.enabled = true;
                socket.OnBatteryInserted += UpdateBatteryAdded;
            }
        }
    }

    private void UpdateBatteryAdded()
    {
        batteryCount++;

        if (batteryCount == batterySockets.Length)
        {
            Debug.Log("Battery Interaction Completed");
            InvokeOnInteractionCompleted();
        }
    }
}
