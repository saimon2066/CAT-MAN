using TMPro;
using UnityEngine;

public class VersionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;

    private void Start()
    {
        display.text = Application.version;
    }
}
