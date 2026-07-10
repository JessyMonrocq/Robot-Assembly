using UnityEngine;

public class FailureScreenManager : MonoBehaviour
{
    public void BackToMenu()
    {
        GameManager.Instance.ReturnToRequestScreen(GetComponent<CanvasGroup>());
    }
}
