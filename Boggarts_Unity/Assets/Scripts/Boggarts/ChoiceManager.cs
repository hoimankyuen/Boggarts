using Input;
using JetBrains.Annotations;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private InputReader m_InputReader;
    
    public static ChoiceManager Instance;

    public Gate m_currentGate;

    public Transform m_gate_1_contents;
    public Transform m_gate_2_contents;
    public Transform m_gate_3_contents;
    public Transform m_gate_4_contents;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        m_InputReader.EnablePlayerActions();

        m_InputReader.Button_North += OnButtonNorth;
        m_InputReader.Button_East += OnButtonEast;
        m_InputReader.Button_West += OnButtonWest;
        m_InputReader.Button_South += OnButtonSouth;
    }

    private void OnButtonNorth()
    {
        Debug.Log("OnButtonNorth");
        m_gate_1_contents.gameObject.SetActive(true);
    }

    private void OnButtonEast()
    {
        Debug.Log("OnButtonEast");
        m_gate_2_contents.gameObject.SetActive(true);
    }
    
    private void OnButtonSouth()
    {
        Debug.Log("OnButtonSouth");
        m_gate_3_contents.gameObject.SetActive(true);
    }

    private void OnButtonWest()
    {
        Debug.Log("OnButtonWest");
        m_gate_4_contents.gameObject.SetActive(true);
    }

    
}
