using UnityEngine;

public class FailureScreenManager : MonoBehaviour
{
    public void BackToMenu()
    {
        GameManager.Instance.GoToRequestScreen(GetComponent<CanvasGroup>());
    }
}
