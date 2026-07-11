using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    public enum CursorType
    {
        Default,
        Interaction
    }

    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D interactionCursor;

    private CursorType currentCursor;

    private PointerEventData pointerData;
    private readonly List<RaycastResult> results = new();

    private void Awake()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        var pointerData = new PointerEventData(EventSystem.current)
        {
            position = Pointer.current.position.ReadValue()
        };

        results.Clear();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<UnityEngine.UI.Selectable>() != null)
            {
                SetCursor(CursorType.Interaction);
                break;
            }
            else
            {
                SetCursor(CursorType.Default);
            }
        }
    }

    private void SetCursor(CursorType type)
    {
        if (currentCursor == type)
            return;

        currentCursor = type;

        switch (type)
        {
            case CursorType.Default:
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                break;

            case CursorType.Interaction:
                Cursor.SetCursor(interactionCursor, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}