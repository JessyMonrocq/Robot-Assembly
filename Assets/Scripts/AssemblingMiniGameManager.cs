using UnityEngine;

public class AssemblingMiniGameManager : MonoBehaviour
{
    [Header("Assebling Mini Game Manager References")]
    [SerializeField] private ScrewButton[] batteryPanelScrews;
    [SerializeField] private BatterySocket[] batterySockets;
    [SerializeField] private Transform[] batteryParents;
    [SerializeField] private DraggableBattery batteryItemPrefab;
    [SerializeField] private SwitchButton[] switchButtons;
    [SerializeField] private DraggableLever batteryPanelSlider;
    [SerializeField] private DraggableLever finalLever;
    [SerializeField] private KeyPadController keypadController;

    private string randomPassword;
    private const int passwordLength = 4;
    private bool minigameComplete;

    private int screwCount;
    private int batteryCount;
    private int switchButtonCount;

    private void OnEnable()
    {
        foreach (ScrewButton screw in batteryPanelScrews)
        {
            screw.OnUnscrewed += UpdateScrewCount;
        }

        foreach (SwitchButton switchButton in switchButtons)
        {
            switchButton.OnSwitch += UpdateSwitchButtonCount;
        }

        batteryPanelSlider.OnValueChanged.AddListener(UpdatePanelSliderState);
        finalLever.OnValueChanged.AddListener(UpdateFinalSliderState);
        keypadController.onCorrectpassword.AddListener(UpdateKeypadState);
    }

    private void OnDisable()
    {
        foreach (ScrewButton screw in batteryPanelScrews)
        {
            screw.OnUnscrewed -= UpdateScrewCount;
        }

        foreach (SwitchButton switchButton in switchButtons)
        {
            switchButton.OnSwitch -= UpdateSwitchButtonCount;
        }

        batteryPanelSlider.OnValueChanged.RemoveAllListeners();
        finalLever.OnValueChanged.RemoveAllListeners();
        keypadController.onCorrectpassword.RemoveAllListeners();
    }

    private void UpdateScrewCount()
    {
        screwCount++;

        if (screwCount == batteryPanelScrews.Length)
        {
            batteryPanelSlider.CanInteract = true;
        }
    }

    private void UpdateBatteryAdded()
    {
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

    private void UpdateFinalSliderState(float value)
    {
        if (value == 1f && !minigameComplete)
        {
            minigameComplete = true;
            finalLever.CanInteract = false;

            GameManager.Instance.GoToResultScreen(this.gameObject.GetComponent<CanvasGroup>());
        }
    }

    public void InitializeMiniGame()
    {
        minigameComplete = false;

        screwCount = 0;
        batteryCount = 0;
        switchButtonCount = 0;

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

        foreach (SwitchButton switchButton in switchButtons)
        {
            switchButton.ResetSwitchButton();
            switchButton.CanInteract(false);
        }

        batteryPanelSlider.SetCurrentValueImmediate(0f);
        batteryPanelSlider.CanInteract = false;
        finalLever.SetCurrentValueImmediate(0f);
        finalLever.CanInteract = false;

        keypadController.ResetKeypad();
    }
}
