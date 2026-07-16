using UnityEngine;

public class VolumeSlidersInteraction : MiniInteraction
{
    [SerializeField] private GoalSlider[] goalSliders;

    private int slidersCount;

    private const int randomMin = 1;
    private const int randomMax = 10;

    private void OnEnable()
    {
        foreach (GoalSlider gl in goalSliders)
        {
            gl.OnGoalReached += UpdateVolumeSlidersState;
        }
    }

    private void OnDisable()
    {
        foreach (GoalSlider gl in goalSliders)
        {
            gl.OnGoalReached -= UpdateVolumeSlidersState;
        }
    }

    public override void ResetInteraction()
    {
        slidersCount = 0;

        foreach (GoalSlider gl in goalSliders)
        {
            gl.ResetGoalSlider();
        }
    }

    public override void InitializeInteraction()
    {
        foreach (GoalSlider gl in goalSliders)
        {
            int random = Random.Range(randomMin, randomMax);

            gl.Initialize(random);
        }
    }

    private void UpdateVolumeSlidersState(bool goal)
    {
        slidersCount += goal ? 1 : -1;

        if (slidersCount == goalSliders.Length)
        {
            InvokeOnInteractionCompleted();
        }
    }
}
