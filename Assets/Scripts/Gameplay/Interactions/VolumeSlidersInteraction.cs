using UnityEngine;

public class VolumeSlidersInteraction : MiniInteraction
{
    [SerializeField] private GoalSlider[] goalSliders;

    private int slidersCount;

    private const int randomMin = 1;
    private const int randomMax = 11;

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
        foreach (GoalSlider gl in goalSliders)
        {
            gl.ResetGoalSlider();
        }

        slidersCount = 0;
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
            foreach (GoalSlider gl in goalSliders)
            {
                gl.DisableSlider();
            }

            InvokeOnInteractionCompleted();
        }
    }
}
