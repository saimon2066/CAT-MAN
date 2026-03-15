using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public MainInputAsset input;

    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }
    private void Awake()
    {
        if (instance == null) instance = this;
        if (input == null) input = new MainInputAsset();
    }
}
