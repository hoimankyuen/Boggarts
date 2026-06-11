using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Attach to a UI Button. Toggles gyro input on/off on IglooGyroInput.
/// Button label and colour update to reflect current state.
/// </summary>
public class GyroToggleButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private IglooGyroInput m_GyroInput;

    private Image m_Image;
    private TMP_Text m_Label;

    private readonly Color k_OnColour  = new Color(0.3f, 1f, 0.3f);
    private readonly Color k_OffColour = new Color(1f, 0.3f, 0.3f);

    private void Awake()
    {
        m_Image = GetComponent<Image>();
        m_Label = GetComponentInChildren<TMP_Text>();
        Refresh();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_GyroInput.ToggleGyro();
        Refresh();
    }

    private void Refresh()
    {
        bool on = m_GyroInput.GyroEnabled;
        if (m_Image != null) m_Image.color = on ? k_OnColour : k_OffColour;
        if (m_Label != null) m_Label.text  = on ? "Gyro: ON" : "Gyro: OFF";
    }
}
