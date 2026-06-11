using Input;
using TMPro;
using UnityEngine;

public class DebugHeightReader : MonoBehaviour
{
    [SerializeField] private InputReader m_InputReader;
    [SerializeField] private Transform m_ground;
    [SerializeField] private TextMeshPro m_debugText;

    private void Start()
    {
        m_InputReader.EnablePlayerActions();
        m_InputReader.Move += OnMove;
    }

    void Update()
    {
        m_debugText.text = "Height: " + m_ground.position.y;
    }
    
    private void OnMove(float context)
    {
        if (context != 0)
        {
            var vector3 = m_ground.position;
            vector3.y += context;
            m_ground.position = vector3;
        }
    }
}