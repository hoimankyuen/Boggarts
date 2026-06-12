using Input;
using JetBrains.Annotations;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private InputReader m_InputReader;
    [SerializeField] private Fogs m_fogs;
    
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
        EvaluateChoice(0);
    }

    private void OnButtonEast()
    {
        Debug.Log("OnButtonEast");
        EvaluateChoice(1);
    }
    
    private void OnButtonSouth()
    {
        Debug.Log("OnButtonSouth");
        EvaluateChoice(2);
    }

    private void OnButtonWest()
    {
        Debug.Log("OnButtonWest");
        EvaluateChoice(3);
    }

    private void EvaluateChoice(int choice)
    {
        //PLAY SOUND m_currentGate.Choices[choice].Sound
        
        if (m_currentGate.Choices[choice].IsGood)
        {
            TransitionToNextGate(false);
        }
        else
        {
            TransitionToNextGate(true);
        }
    }

    private void TransitionToNextGate(bool angry)
    {
        m_fogs.ShowSwirlingAt(true, angry, Vector3.zero);
        //m_fogs.ShowSurroundFogAt(true);
        m_fogs.ShowCentreFogAt(true);
    }
}