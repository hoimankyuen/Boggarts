using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor window that previews the igloo render texture during Play Mode,
/// so you can see the scene output without being on the igloo machine.
/// Open via: Window > Igloo Render Texture Preview
/// </summary>
public class RenderTexturePreviewWindow : EditorWindow
{
    private Camera m_PreviewCamera;

    [MenuItem("Window/Igloo Render Texture Preview")]
    public static void Open()
    {
        GetWindow<RenderTexturePreviewWindow>("Igloo Preview");
    }

    private void OnGUI()
    {
        if (!EditorApplication.isPlaying)
        {
            EditorGUILayout.HelpBox("Enter Play Mode to preview the render texture.", MessageType.Info);
            return;
        }

        if (m_PreviewCamera == null)
            m_PreviewCamera = FindPreviewCamera();

        if (m_PreviewCamera == null)
        {
            EditorGUILayout.HelpBox("No camera with a render texture found in the scene.", MessageType.Warning);
            if (GUILayout.Button("Retry")) m_PreviewCamera = FindPreviewCamera();
            return;
        }

        RenderTexture rt = m_PreviewCamera.targetTexture;
        if (rt == null)
        {
            EditorGUILayout.HelpBox($"Camera '{m_PreviewCamera.name}' has no render texture assigned.", MessageType.Warning);
            return;
        }

        // Draw camera name and texture info
        EditorGUILayout.LabelField($"Camera: {m_PreviewCamera.name}   |   Texture: {rt.width}x{rt.height}", EditorStyles.miniLabel);

        // Fill the remaining window space with the render texture, preserving aspect ratio
        Rect area = GUILayoutUtility.GetRect(position.width, position.height - 30f);
        float texAspect = (float)rt.width / rt.height;
        float areaAspect = area.width / area.height;

        Rect drawRect;
        if (texAspect > areaAspect)
        {
            float h = area.width / texAspect;
            drawRect = new Rect(area.x, area.y + (area.height - h) * 0.5f, area.width, h);
        }
        else
        {
            float w = area.height * texAspect;
            drawRect = new Rect(area.x + (area.width - w) * 0.5f, area.y, w, area.height);
        }

        GUI.DrawTexture(drawRect, rt, ScaleMode.StretchToFill);
        Repaint();
    }

    private Camera FindPreviewCamera()
    {
        // Prefer a camera explicitly named for the igloo 2D output
        foreach (Camera cam in FindObjectsOfType<Camera>())
        {
            if (cam.targetTexture != null)
                return cam;
        }
        return null;
    }
}
