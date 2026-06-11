using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Press 1 to load the 2D Example scene, press 2 to load the 3D Example scene.
/// Add this to any persistent GameObject in your scene.
/// Make sure both scenes are added to File > Build Settings > Scenes In Build.
/// </summary>
public class SceneSwitcher : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            SceneManager.LoadScene("2D Example");
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
            SceneManager.LoadScene("3D Example");
    }
}
