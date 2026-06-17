using UnityEngine;
using UnityEngine.InputSystem;

public class Screenshot : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("screenshotting");
            ScreenCapture.CaptureScreenshot("screen.png", 1);
        }
    }
}
