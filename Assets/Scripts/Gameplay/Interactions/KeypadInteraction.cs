using UnityEngine;

public class KeypadInteraction : MiniInteraction
{
    [SerializeField] private KeyPadController keypadController;

    private string randomPassword;

    private void OnEnable()
    {
        keypadController.onCorrectpassword.AddListener(InvokeOnInteractionCompleted);
    }

    private void OnDisable()
    {
        keypadController.onCorrectpassword.RemoveListener(InvokeOnInteractionCompleted);
    }

    public override void ResetInteraction()
    {
        keypadController.ResetKeypad();
    }

    public override void InitializeInteraction()
    {
        randomPassword = Random.Range(0, 10000).ToString("D4");
        keypadController.InitializeKeypad(randomPassword);
    }
}
