using UnityEngine;
using UnityEngine.EventSystems;

public class MouseCameraControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CharacterCreationCameraMovement movement;

    public void OnPointerDown(PointerEventData eventData)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        switch (eventData.pointerId)
        {
            case -1:
                movement.rotatingCamera = true;
                break;
            case -2:
                movement.translatingCamera = true;
                break;
        }      
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        switch (eventData.pointerId)
        {
            case -1:
                movement.rotatingCamera = false;
                break;
            case -2:
                movement.translatingCamera = false;
                break;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        movement.hovering = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        movement.hovering = false;
    }
}