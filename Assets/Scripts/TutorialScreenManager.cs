using UnityEngine;
using UnityEngine.UI;

public class TutorialScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialPanels;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    private int currentIndex;

    public void InitializeTutorialScreen()
    {
        foreach (GameObject go in tutorialPanels)
        {
            go.SetActive(false);
        }

        currentIndex = 0;
        tutorialPanels[currentIndex].SetActive(true);

        previousButton.interactable = false;
        nextButton.interactable = true;
    }

    public void NextPanel(bool forward)
    {
        tutorialPanels[currentIndex].SetActive(false);

        currentIndex += forward ? 1 : -1;
        if (currentIndex < 0)
        {
            currentIndex = 0;
        }
        else if (currentIndex > tutorialPanels.Length - 1)
        {
            currentIndex -= 1;
        }

        tutorialPanels[currentIndex].SetActive(true);

        previousButton.interactable = (currentIndex > 0);
        nextButton.interactable = (currentIndex < tutorialPanels.Length - 1);
    }
}
