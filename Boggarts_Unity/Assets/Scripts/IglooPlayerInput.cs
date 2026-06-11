using Igloo.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Handles crosshair click input for the 3D Igloo Player scene.
/// Place on any active GameObject in the 3D scene.
/// Press Xbox A (or left mouse) to click whatever the crosshair is pointing at.
/// Works for both 3D objects (ClickableObject) and World Space UI canvases.
/// </summary>
public class IglooPlayerInput : MonoBehaviour
{
    [SerializeField] private Canvas m_WorldCanvas;

    private void Update()
    {
        // IglooManager spawns the camera at runtime — keep trying until assigned
        if (m_WorldCanvas != null && m_WorldCanvas.worldCamera == null && Camera.main != null)
            m_WorldCanvas.worldCamera = Camera.main;

        bool firePressed = Mouse.current.leftButton.wasPressedThisFrame ||
                           (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);

        if (!firePressed) return;

        // Cast ray from the main camera forward (same direction the player is looking)
        Camera cam = Camera.main;
        if (cam == null) return;

        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit)) return;

        // Try 3D clickable object first
        ClickableObject clickable = hit.collider.GetComponent<ClickableObject>();
        if (clickable != null)
        {
            clickable.OnClick();
            return;
        }

        // Try World Space canvas — find which UI element the world hit point is inside
        GraphicRaycaster raycaster = hit.collider.GetComponentInParent<GraphicRaycaster>();
        if (raycaster != null)
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            foreach (var selectable in raycaster.GetComponentsInChildren<UnityEngine.UI.Selectable>())
            {
                RectTransform rect = selectable.GetComponent<RectTransform>();
                if (RectTransformUtility.RectangleContainsScreenPoint(rect, cam.WorldToScreenPoint(hit.point), cam))
                    ExecuteEvents.Execute(selectable.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}
