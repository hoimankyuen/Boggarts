using Input;
using JetBrains.Annotations;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private InputReader m_InputReader;
    [SerializeField] private Fogs m_fogs;
    
    public static ChoiceManager Instance;

    public Gate m_currentGate;

    [Header("Gate Contents")]
    public Transform m_gate_1_contents;
    public Transform m_gate_2_contents;
    public Transform m_gate_3_contents;
    public Transform m_gate_4_contents;

    [Header("Choice Circles")]
    public Transform m_choice_circle1;
    public Transform m_choice_circle2;
    public Transform m_choice_circle3;
    public Transform m_choice_circle4;
    
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
        m_audioSource.clip = m_currentGate.Choices[choice].Sound;
        m_audioSource.Play();
        
        if (m_currentGate.Choices[choice].IsGood)
        {
            TransitionToNextGate(true, choice);
        }
        else
        {
            TransitionToNextGate(false, choice);
        }
    }

    private void TransitionToNextGate(bool angry, int choice)
    {
        Vector3 fogPosition = new Vector3();
        
        switch (choice)
        {
            case 0:
                fogPosition = m_choice_circle1.position;
                break;
            case 1:
                fogPosition = m_choice_circle2.position;
                break;
            case 2:
                fogPosition = m_choice_circle3.position;
                break;
            case 3:
                fogPosition = m_choice_circle4.position;
                break;
        }
        
        m_fogs.ShowSwirlingAt(true, angry, fogPosition);
        //m_fogs.ShowSurroundFogAt(true, fogPosition);
        //m_fogs.ShowCentreFogAt(true, fogPosition);
        
        //HIDE ONBOARDING/CURRENT GATE CONTENTS
        //SHOW NEW GATE CONTENTS
    }
}