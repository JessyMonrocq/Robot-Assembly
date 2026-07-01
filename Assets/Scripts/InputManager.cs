using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private InputActionReference mousePosition;

    public InputAction MousePosition => mousePosition.action;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetUIInputState(bool state)
    {
        SetUIInputState(MousePosition, state);
    }

    private void SetUIInputState(InputAction action, bool state)
    {
        if (state)
        {
            action.Enable();
        }
        else
        {
            action.Disable();
        }
    }
}
