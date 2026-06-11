using UnityEngine;

/// <summary>
/// Attach to any 3D object to make it clickable via the Igloo crosshair.
/// Cycles through colours each time it is clicked.
/// </summary>
public class ClickableObject : MonoBehaviour
{
    private static readonly Color[] k_Colours = new Color[]
    {
        new Color(1f, 0.3f, 0.3f), // red
        new Color(0.3f, 1f, 0.3f), // green
        new Color(0.3f, 0.5f, 1f), // blue
        new Color(1f, 0.9f, 0.2f), // yellow
        new Color(0.8f, 0.3f, 1f), // purple
    };

    private Renderer m_Renderer;
    private int m_ColourIndex = 0;

    private void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    public void OnClick()
    {
        m_ColourIndex = (m_ColourIndex + 1) % k_Colours.Length;
        m_Renderer.material.color = k_Colours[m_ColourIndex];
    }
}
