using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Attach to a UI Button. Cycles through colours each time it is clicked.
/// Works with both regular mouse clicks and the crosshair SimulateMouseClick system.
/// </summary>
public class ClickableButton : MonoBehaviour, IPointerClickHandler
{
    private static readonly Color[] k_Colours = new Color[]
    {
        new Color(1f, 0.3f, 0.3f), // red
        new Color(0.3f, 1f, 0.3f), // green
        new Color(0.3f, 0.5f, 1f), // blue
        new Color(1f, 0.9f, 0.2f), // yellow
        new Color(0.8f, 0.3f, 1f), // purple
    };

    private Image m_Image;
    private int m_ColourIndex = 0;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_ColourIndex = (m_ColourIndex + 1) % k_Colours.Length;
        m_Image.color = k_Colours[m_ColourIndex];
    }
}
