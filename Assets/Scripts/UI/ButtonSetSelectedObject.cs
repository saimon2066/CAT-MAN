using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSetSelectedObject : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    
    public void SetSelected(GameObject select)
    {
        eventSystem.SetSelectedGameObject(select);
    }
}
