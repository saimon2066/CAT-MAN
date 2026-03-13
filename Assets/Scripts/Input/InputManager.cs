using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public MainInputAsset input;

    private void Awake()
    {
        if (instance == null) instance = this;
        input ??= new MainInputAsset();
    }
}
