using System.Collections;
using UnityEngine;

public class InteractionsScreenManager : MonoBehaviour
{
    public enum Interaction
    {
        Left,
        Middle,
        Right
    }

    [SerializeField] private MiniInteraction[] leftSideInteractions;
    [SerializeField] private MiniInteraction[] middleSideInteractions;
    [SerializeField] private MiniInteraction[] rightSideInteractions;
    [SerializeField] private float startingDelay = 0.5f;

    private Interaction currentInteraction;

    private int leftIndex;
    private int middleIndex;
    private int rightIndex;

    private void OnDisable()
    {
        foreach (var interaction in leftSideInteractions)
        {
            interaction.OnInteractionCompleted -= UpdateMiniGameState;
        }

        foreach (var interaction in middleSideInteractions)
        {
            interaction.OnInteractionCompleted -= UpdateMiniGameState;
        }

        foreach (var interaction in rightSideInteractions)
        {
            interaction.OnInteractionCompleted -= UpdateMiniGameState;
        }
    }

    public void InitializeMiniGame()
    {
        leftSideInteractions[leftIndex].OnInteractionCompleted -= UpdateMiniGameState;
        middleSideInteractions[middleIndex].OnInteractionCompleted -= UpdateMiniGameState;
        rightSideInteractions[rightIndex].OnInteractionCompleted -= UpdateMiniGameState;

        foreach (var interaction in leftSideInteractions)
        {
            interaction.ResetInteraction();
            interaction.gameObject.SetActive(false);
        }

        foreach (var interaction in middleSideInteractions)
        {
            interaction.ResetInteraction();
            interaction.gameObject.SetActive(false);
        }

        foreach (var interaction in rightSideInteractions)
        {
            interaction.ResetInteraction();
            interaction.gameObject.SetActive(false);
        }

        StartCoroutine(InitializeCoroutine());
    }

    private void UpdateMiniGameState()
    {
        switch (currentInteraction)
        {
            case Interaction.Left:
                currentInteraction = Interaction.Middle;
                middleSideInteractions[middleIndex].InitializeInteraction();
                break;
            case Interaction.Middle:
                currentInteraction = Interaction.Right;
                rightSideInteractions[rightIndex].InitializeInteraction();
                break;
            case Interaction.Right:
                GameManager.Instance.GoToResultScreen(this.gameObject.GetComponent<CanvasGroup>());
                break;
        }
    }

    private IEnumerator InitializeCoroutine()
    {
        leftIndex = Random.Range(0, leftSideInteractions.Length);
        leftSideInteractions[leftIndex].gameObject.SetActive(true);

        middleIndex = Random.Range(0, middleSideInteractions.Length);
        middleSideInteractions[middleIndex].gameObject.SetActive(true);

        rightIndex = Random.Range(0, rightSideInteractions.Length);
        rightSideInteractions[rightIndex].gameObject.SetActive(true);

        leftSideInteractions[leftIndex].OnInteractionCompleted += UpdateMiniGameState;
        middleSideInteractions[middleIndex].OnInteractionCompleted += UpdateMiniGameState;
        rightSideInteractions[rightIndex].OnInteractionCompleted += UpdateMiniGameState;

        yield return new WaitForSeconds(startingDelay);

        currentInteraction = Interaction.Left;
        leftSideInteractions[leftIndex].InitializeInteraction();
    }
}
