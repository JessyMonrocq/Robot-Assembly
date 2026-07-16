using System;
using UnityEngine;
using UnityEngine.UI;

public class GoalSlider : MonoBehaviour
{
    public event Action<bool> OnGoalReached;

    [SerializeField] private Slider interactSlider;
    [SerializeField] private Slider goalSlider;

    private bool goalReached;

    private void OnEnable()
    {
        interactSlider.onValueChanged.AddListener(UpdateSliderState);
    }

    private void OnDisable()
    {
        interactSlider.onValueChanged.RemoveListener(UpdateSliderState);
    }

    public void ResetGoalSlider()
    {
        interactSlider.value = 0;
        interactSlider.interactable = false;
        goalSlider.value = 1;
        goalReached = false;
    }

    public void DisableSlider()
    {
        interactSlider.interactable = false;
    }

    public void Initialize(float value)
    {
        goalSlider.value = value;
        interactSlider.interactable = true;
    }

    private void UpdateSliderState(float value)
    {
        if (interactSlider.value == goalSlider.value)
        {
            goalReached = true;
            OnGoalReached?.Invoke(true);
        }
        else if (goalReached)
        {
            goalReached = false;
            OnGoalReached?.Invoke(false);
        }
    }
}
