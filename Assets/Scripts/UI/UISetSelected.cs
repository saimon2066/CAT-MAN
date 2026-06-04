using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UISetSelected : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject setSelected;

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChanged;
    }
    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChanged;
    }

    private void Start()
    {
        if (Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame)
            eventSystem.SetSelectedGameObject(setSelected);
    }

    private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad && (change == InputDeviceChange.Added || change == InputDeviceChange.Reconnected))
        {
            eventSystem.SetSelectedGameObject(setSelected);
        }
    }
}
