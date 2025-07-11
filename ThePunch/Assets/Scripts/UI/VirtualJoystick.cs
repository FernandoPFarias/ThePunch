using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public Image joystickBG;
    public Image joystickHandle;
    [HideInInspector]
    public Vector2 inputDirection = Vector2.zero;

    private void Start()
    {
        if (joystickBG == null)
            joystickBG = GetComponent<Image>();
        if (joystickHandle == null && transform.childCount > 0)
            joystickHandle = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBG.rectTransform,
            ped.position,
            ped.pressEventCamera,
            out pos))
        {
            pos.x = (pos.x / joystickBG.rectTransform.sizeDelta.x);
            pos.y = (pos.y / joystickBG.rectTransform.sizeDelta.y);

            inputDirection = new Vector2(pos.x * 2, pos.y * 2);
            inputDirection = (inputDirection.magnitude > 1) ? inputDirection.normalized : inputDirection;

            // Move handle
            joystickHandle.rectTransform.anchoredPosition = new Vector2(
                inputDirection.x * (joystickBG.rectTransform.sizeDelta.x / 2.5f),
                inputDirection.y * (joystickBG.rectTransform.sizeDelta.y / 2.5f));
        }
    }

    public void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public void OnPointerUp(PointerEventData ped)
    {
        inputDirection = Vector2.zero;
        joystickHandle.rectTransform.anchoredPosition = Vector2.zero;
    }

    // Método para acessar o valor do joystick
    public float Horizontal() { return inputDirection.x; }
    public float Vertical() { return inputDirection.y; }
    public Vector2 Direction() { return inputDirection; }
    public bool IsActive => inputDirection.magnitude > 0.1f;
} 