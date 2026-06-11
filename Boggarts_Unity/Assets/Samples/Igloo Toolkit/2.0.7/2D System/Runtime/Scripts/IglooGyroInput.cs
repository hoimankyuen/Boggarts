using extOSC;
using Igloo.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class IglooGyroInput : MonoBehaviour
{
    [SerializeField] private Camera m_Igloo2DCamera;
    [SerializeField] private RectTransform m_Crosshair;
    [SerializeField] private GraphicRaycaster m_GraphicRaycaster;
    [SerializeField] private TMP_Text m_DebugText;
    [SerializeField] private float m_GamepadSpeed = 2000f;

    private Vector3 m_LastGyro;
    public bool GyroEnabled { get; private set; } = true;

    public void ToggleGyro()
    {
        GyroEnabled = !GyroEnabled;
    }

    /// <summary>
    /// Mono start function 
    /// Assigns the NetworkManagerOSC interface to the crosshair movement function
    /// </summary>
    private void Start()
    {
        NetworkManagerOSC.instance.Setup();
        NetworkManagerOSC.instance.OnPlayerRotationWarper += SetCrosshairPosition;
    }

    /// <summary>
    /// Called on message received through OSC interface containing XIMU Gyroscope data from the Igloo Core engine suite. 
    /// Converts the gyroscope data to X Y position data for the crosshair
    /// Set's the crosshairs position back to the start if it goes beyond it's bounds
    /// applys the movement to the m_Crosshair object.
    /// </summary>
    /// <param name="name">Not Required - redundant string</param>
    /// <param name="gyroEuler">Vector3 raw gyroscope values</param>
    private void SetCrosshairPosition(string name, Vector3 gyroEuler)
    {
        if (!GyroEnabled) return;
        float width = m_Igloo2DCamera.targetTexture.width;
        float height = m_Igloo2DCamera.targetTexture.height;
        float aspectRatio = width / height; // ~11 for the igloo wall (11914x1080)

        // Horizontal: yaw is -180 to +180. Map to 0..width, always positive, wraps around wall.
        float crosshairX = (gyroEuler.y / 360f) * width;
        crosshairX = ((crosshairX % width) + width) % width;

        // Vertical: absolute mapping — pitch directly sets Y position.
        // Works in both portrait and landscape as the app remaps axes before sending.
        // Offset by half height (bottom-left anchor) and apply aspect ratio compensation.
        m_LastGyro = gyroEuler;
        float crosshairY = height * 0.5f + (-gyroEuler.x / 180f) * height * aspectRatio;
        crosshairY = Mathf.Clamp(crosshairY, 0f, height);

        // Assign position to the crosshair object
        m_Crosshair.anchoredPosition = new Vector2(crosshairX, crosshairY);
    }

#if UNITY_EDITOR
    /// <summary>
    /// Simulates gyro-driven crosshair movement using the mouse, for editor testing
    /// without the igloo/phone OSC setup. Hold right mouse button to move the crosshair.
    /// </summary>
    private void UpdateEditorMouseSimulation()
    {
        if (!Mouse.current.rightButton.isPressed) return;

        float width = m_Igloo2DCamera.targetTexture.width;
        float height = m_Igloo2DCamera.targetTexture.height;

        // Mouse delta in pixels, scaled to match OSC sensitivity
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float deltaX = mouseDelta.x * width * 0.005f;
        float deltaY = mouseDelta.y * height * 0.005f;

        Vector2 pos = m_Crosshair.anchoredPosition;
        pos.x += deltaX;
        pos.y += deltaY;

        // Wrap horizontally, clamp vertically (matches igloo wall behaviour)
        pos.x %= width;
        pos.y = Mathf.Clamp(pos.y, -height * 0.5f, height * 0.5f);

        m_Crosshair.anchoredPosition = pos;
    }
#endif

    /// <summary>
    /// Mono Update function
    /// Handles interaction and buttons presses
    /// </summary>
    private void Update()
    {
#if UNITY_EDITOR
        UpdateEditorMouseSimulation();
#endif
        // Update debug text if assigned
        if (m_DebugText != null)
            m_DebugText.text = $"gyro.x: {m_LastGyro.x:F1}  gyro.y: {m_LastGyro.y:F1}  gyro.z: {m_LastGyro.z:F1}";

        // Xbox left stick moves crosshair
        if (Gamepad.current != null)
        {
            Vector2 stick = Gamepad.current.leftStick.ReadValue();
            if (stick.sqrMagnitude > 0.01f)
            {
                float width = m_Igloo2DCamera.targetTexture.width;
                float height = m_Igloo2DCamera.targetTexture.height;
                Vector2 pos = m_Crosshair.anchoredPosition;
                pos.x += stick.x * m_GamepadSpeed * Time.deltaTime;
                pos.y += stick.y * m_GamepadSpeed * Time.deltaTime;
                pos.x = ((pos.x % width) + width) % width;
                pos.y = Mathf.Clamp(pos.y, 0f, height);
                m_Crosshair.anchoredPosition = pos;
            }
        }

        // Left Mouse Button Down || Xbox Button A Down
        bool firePressed = Mouse.current.leftButton.wasPressedThisFrame ||
                           (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);
        if (firePressed)
        {
            // Interaction Method 1 - Create a raycast that hits stuff. Do something when it his what you want. 
            RaycastAtLocation(m_Crosshair.anchoredPosition3D);

            // Interaction Method 2 - Craete a mouse click at the crosshair location - Good for doing UI events
            SimulateMouseClick(m_Crosshair.anchoredPosition3D);
        }
    }


    /// <summary>
    /// Simulate mouse click if using a UI canvas for selection of objects. 
    /// Uses a raycast for selection like a regular mouse click interaction with a Overlay UI.
    /// </summary>
    /// <param name="location">Vector3 location to generate the mouse click event. </param>
    private void SimulateMouseClick(Vector3 location)
    {
        if (m_GraphicRaycaster == null) return;

        // For Screen Space - Camera canvases, the GraphicRaycaster works in screen space
        // relative to the render texture camera's viewport. Convert anchoredPosition
        // (render texture pixels) to that camera's screen position.
        Vector3 worldPos = m_Crosshair.position; // world position of crosshair
        Vector2 screenPos = m_Igloo2DCamera.WorldToScreenPoint(worldPos);

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        m_GraphicRaycaster.Raycast(pointerData, results);

        foreach (var result in results)
        {
            ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
        }
    }

    private void RaycastAtLocation(Vector3 location)
    {
        Ray ray = Camera.main.ScreenPointToRay(location);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Hit: " + hit.collider.name);

            // You can call a function on the hit object
        }
    }
}
