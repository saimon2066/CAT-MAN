using TMPro;
using UnityEngine;

public class UIVersion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;

    private void Start()
    {
        display.text = Application.version;
    }
}
