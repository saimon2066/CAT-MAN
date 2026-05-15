using UnityEditor;
using UnityEngine;

public class ButtonQuit : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        EditorApplication.isPlaying = false;
    }
}
