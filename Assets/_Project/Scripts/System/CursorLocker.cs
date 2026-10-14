using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Hides and locks the mouse cursor so mouse movement only rotates the camera.
/// Press Escape to release the cursor while testing inside the editor.
/// </summary>
/// 
public class CursorLocker : MonoBehaviour
{
    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        // Keyboard.current is null when no keyboard is connected
        if (Keyboard.current == null)
        {
            return;
        }

        // Releasing the cursor allows clicking on editor windows again
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}