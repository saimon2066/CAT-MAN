using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonArrows : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{   
    [SerializeField] private string baseText;
    [SerializeField] private TextMeshProUGUI textDisplay;

    private void Start()
    {
        textDisplay.text = baseText;
    }

    public void OnSelect(BaseEventData data)
    {
        textDisplay.text = $"> {baseText} <";
    }
    public void OnDeselect(BaseEventData data)
    {
        textDisplay.text = baseText;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        textDisplay.text = $"> {baseText} <";
    }
    public void OnPointerExit(PointerEventData data)
    {
        textDisplay.text = baseText;
    }
}
