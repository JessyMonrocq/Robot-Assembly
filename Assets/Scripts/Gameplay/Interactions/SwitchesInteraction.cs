using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SwitchesInteraction : MiniInteraction
{
    [SerializeField] private SwitchButton[] switchButtons;

    private const int minButtons = 3;
    private int buttonsCount;

    private HashSet<SwitchButton> chosenButtons = new();
    private HashSet<SwitchButton> notChosenButtons = new();

    private void Start()
    {
        InitializeInteraction();
    }

    private void OnEnable()
    {
        foreach (SwitchButton sb in switchButtons)
        {
            sb.OnSwitch += UpdateSwitchButtonCount;
        }
    }

    private void OnDisable()
    {
        foreach (SwitchButton sb in switchButtons)
        {
            sb.OnSwitch -= UpdateSwitchButtonCount;
        }
    }

    public override void ResetInteraction()
    {
        foreach (SwitchButton sb in switchButtons)
        {
            sb.GetComponent<Image>().color = Color.white;
            sb.ResetSwitchButton();
            sb.CanInteract(false);
        }
    }

    public override void InitializeInteraction()
    {
        buttonsCount = Random.Range(minButtons, switchButtons.Length);

        chosenButtons.Clear();
        notChosenButtons.Clear();

        var shuffled = switchButtons.OrderBy(x => Random.value).ToArray();

        for (int i = 0; i < shuffled.Length; i++)
        {
            if (i < buttonsCount)
            {
                chosenButtons.Add(shuffled[i]);
            }
            else
            {
                notChosenButtons.Add(shuffled[i]);
            }
        }

        foreach (SwitchButton sb in switchButtons)
        {
            bool shouldBeOn = chosenButtons.Contains(sb);
            sb.GetComponent<Image>().color = shouldBeOn ? Color.red : Color.green;
            sb.CanInteract(true);
        }
    }

    private void UpdateSwitchButtonCount(SwitchButton button, bool isOn)
    {
        bool shouldBeOn = chosenButtons.Contains(button);
        button.GetComponent<Image>().color = (button.IsOn != shouldBeOn) ? Color.red : Color.green;

        bool allChosenOn = chosenButtons.Count == 0 || chosenButtons.All(b => b.IsOn);
        bool allNotChosenOff = notChosenButtons.Count == 0 || notChosenButtons.All(b => !b.IsOn);

        if (allChosenOn && allNotChosenOff)
        {
            foreach (SwitchButton sb in switchButtons)
            {
                sb.CanInteract(false);
            }

            InvokeOnInteractionCompleted();
        }
    }
}
