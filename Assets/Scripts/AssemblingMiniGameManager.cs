using UnityEngine;

public class AssemblingMiniGameManager : MonoBehaviour
{
    [Header("Assebling Mini Game Manager References")]
    [SerializeField] private ScrewButton[] batteryPanelScrews;
    [SerializeField] private Socket[] batterySockets;
    [SerializeField] private Transform[] batteryParents;
    [SerializeField] private DragItem batteryItemPrefab;
    [SerializeField] private SwitchButton[] switchButtons;
    [SerializeField] private DraggableLever batteryPanelSlider;
    [SerializeField] private DraggableLever finalLever;
    [SerializeField] private KeyPadController keypadController;

    private string randomPassword;
    private const int passwordLength = 4;

    private int screwCount;
    private int batteryCount;
    private int switchButtonCount;

    private void OnEnable()
    {
        foreach (ScrewButton screw in batteryPanelScrews)
        {
            screw.OnUnscrewed += UpdateScrewCount;
        }

        foreach (Socket socket in batterySockets)
        {
            socket.OnSocketed += UpdateBatteryCount;
        }

        foreach (SwitchButton switchButton in switchButtons)
        {
            switchButton.OnSwitch += UpdateSwitchButtonCount;
        }

        batteryPanelSlider.OnValueChanged.AddListener(UpdatePanelSliderState);
        finalLever.OnValueChanged.AddListener(UpdateFinalSliderState);
        keypadController.onCorrectpassword.AddListener(UpdateKeypadState);
    }

    private void UpdateScrewCount()
    {
        screwCount++;

        if (screwCount == batteryPanelScrews.Length)
        {
            batteryPanelSlider.CanInteract = true;
        }
    }

    private void UpdateBatteryCount(GameObject battery)
    {
        battery.GetComponent<DragItem>().SetDraggable(false);
        batteryCount++;

        if (batteryCount == batterySockets.Length)
        {
            randomPassword = Random.Range(0, 10000).ToString("D4");
            keypadController.InitializeKeypad(randomPassword);
        }
    }

    private void UpdateSwitchButtonCount(bool isOn)
    {
        switchButtonCount += isOn ? 1 : -1;

        if (switchButtonCount == switchButtons.Length)
        {
            foreach (SwitchButton switchButton in switchButtons)
            {
                switchButton.CanInteract(false);
            }

            finalLever.CanInteract = true;
        }
    }

    private void UpdateKeypadState()
    {
        foreach (SwitchButton switchButton in switchButtons)
        {
            switchButton.CanInteract(true);
        }
    }

    private void UpdatePanelSliderState(float value)
    {
        if (value == 1f)
        {
            foreach (Socket socket in batterySockets)
            {
                socket.CanSocket = true;
            }
        }
        else
        {
            foreach (Socket socket in batterySockets)
            {
                socket.CanSocket = false;
            }
        }
    }

    private void UpdateFinalSliderState(float value)
    {
        if (value == 1f)
        {
            finalLever.CanInteract = false;

            Debug.Log("Final Step complete");
            // FINAL STEP COMPLETE
        }
    }

    public void InitializeMiniGame()
    {
        screwCount = 0;
        batteryCount = 0;
        switchButtonCount = 0;

        foreach (ScrewButton screw in batteryPanelScrews)
        {
            screw.ResetScrewButton();
            screwCount++;
        }

        foreach (Socket socket in batterySockets)
        {
            socket.RemoveItem(true);
            socket.CanSocket = false;
            batteryCount++;
        }

        foreach (Transform parent in batteryParents)
        {
            foreach (Transform child in parent)
            {
                Destroy(child);
            }

            DragItem item = Instantiate(batteryItemPrefab, parent);
            item.transform.localPosition = Vector3.zero;
        }

        foreach (SwitchButton switchButton in switchButtons)
        {
            switchButton.ResetSwitchButton();
            switchButton.CanInteract(false);
            switchButtonCount++;
        }

        batteryPanelSlider.SetCurrentValueImmediate(0f);
        batteryPanelSlider.CanInteract = false;
        finalLever.SetCurrentValueImmediate(0f);
        finalLever.CanInteract = false;

        keypadController.ResetKeypad();
    }
}
