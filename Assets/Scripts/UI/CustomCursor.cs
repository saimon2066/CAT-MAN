using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    [SerializeField] private Image cursorImage;
    [SerializeField] private RectTransform canvasRectTransform;

    private void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Mouse.current.position.ReadValue(), Camera.main, out Vector2 localPoint);
        cursorImage.rectTransform.localPosition = localPoint;
    }
}
