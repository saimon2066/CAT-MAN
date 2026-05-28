using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ButtonSetSelectedObject : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    
    public void SetSelected(GameObject select)
    {
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
            eventSystem.SetSelectedGameObject(select);
    }
}
