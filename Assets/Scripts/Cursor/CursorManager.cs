using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D customCursor;
    [SerializeField] Vector2 hotSpot = Vector2.zero;
    [SerializeField] CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        ChangeCursor();
    }

    public void ChangeCursor()
    {
        Cursor.SetCursor(customCursor, hotSpot, cursorMode);
    }

    // Optional: Method to reset cursor to default
    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
