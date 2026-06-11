using UnityEngine;

/// <summary>
/// Attach this to any GameObject in the scene.
/// In the Unity Editor it draws the igloo render texture into the Game View so you
/// can see the output without being on the igloo machine. Does nothing in a build.
/// </summary>
public class IglooEditorPreviewCamera : MonoBehaviour
{
#if UNITY_EDITOR
    private RenderTexture m_IglooRenderTexture;

    private void Awake()
    {
        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            if (cam.targetTexture != null)
            {
                m_IglooRenderTexture = cam.targetTexture;
                break;
            }
        }

        if (m_IglooRenderTexture == null)
            Debug.LogWarning("[IglooEditorPreview] No camera with a render texture found.");
    }

    private void OnGUI()
    {
        if (m_IglooRenderTexture == null) return;

        // Draw the render texture letterboxed into the Game View
        float screenW = Screen.width;
        float screenH = Screen.height;
        float texAspect = (float)m_IglooRenderTexture.width / m_IglooRenderTexture.height;
        float screenAspect = screenW / screenH;

        Rect drawRect;
        if (texAspect > screenAspect)
        {
            float h = screenW / texAspect;
            drawRect = new Rect(0, (screenH - h) * 0.5f, screenW, h);
        }
        else
        {
            float w = screenH * texAspect;
            drawRect = new Rect((screenW - w) * 0.5f, 0, w, screenH);
        }

        GUI.DrawTexture(drawRect, m_IglooRenderTexture);
    }
#endif
}
