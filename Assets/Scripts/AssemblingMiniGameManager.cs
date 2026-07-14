using UnityEngine;

public class AssemblingMiniGameManager : MonoBehaviour
{
    [Header("Assebling Mini Game Manager References")]
    [SerializeField] private ScrewButton[] batteryPanelScrews;
    [SerializeField] private SwitchButton[] switchButtons;
    [SerializeField] private DraggableLever batteryPanelSlider;
    [SerializeField] private DraggableLever finalLever;
    [SerializeField] private KeyPadController keypadController;

    private int screwCount;
    private int switchButtonCount;

    public void InitializeMiniGame()
    {
        screwCount = 0;
        switchButtonCount = 0;

        foreach (ScrewButton screw in  batteryPanelScrews)
        {
            screw.ResetScrewButton();
            screwCount++;
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
