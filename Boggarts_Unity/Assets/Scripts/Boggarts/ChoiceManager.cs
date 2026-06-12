using System.Collections.Generic;
using Input;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private InputReader m_InputReader;
    [SerializeField] private Fogs m_fogs;
    
    public static ChoiceManager Instance;

    public Gate m_currentGate;

    [SerializeField] private Transform m_onboarding;
    
    [Header("Gate Gameobject Contents")]
    public Transform m_gate_1_contents;
    public Transform m_gate_2_contents;
    public Transform m_gate_3_contents;
    public Transform m_gate_4_contents;

    [Header("Gate Data")] [SerializeField] private List<Gate> m_gates;

    [Header("Choice Circles")]
    public Transform m_choice_circle1;
    public Transform m_choice_circle2;
    public Transform m_choice_circle3;
    public Transform m_choice_circle4;
    
    public enum GameState
    {
        Onboarding,
        Gate1,
        Gate2,
        Gate3,
        Gate4,
        End
    }
    [Header("GameState")]
    public GameState m_gameState;

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
        
        m_gameState = GameState.Onboarding;
        
        //SHOW FOG EFFECTS
        
        Invoke(nameof(TransitionToGateOne), 20f);
    }

    private void TransitionToGateOne()
    {
        m_gameState++;
        //m_fogs.ShowSurroundFogAt(true, false);
        //m_fogs.ShowCentreFogAt(true);
        
        //HIDE ONBOARDING/CURRENT GATE CONTENTS
        m_onboarding.gameObject.SetActive(false);
        m_gate_1_contents.gameObject.SetActive(true);
        
    }

    private void OnButtonNorth()
    {
        Debug.Log("OnButtonNorth");
        if (m_gameState is GameState.Onboarding or GameState.End) return;
        EvaluateChoice(0);
    }

    private void OnButtonEast()
    {
        Debug.Log("OnButtonEast");
        if (m_gameState is GameState.Onboarding or GameState.End) return;
        EvaluateChoice(1);
    }
    
    private void OnButtonSouth()
    {
        Debug.Log("OnButtonSouth");
        if (m_gameState is GameState.Onboarding or GameState.End) return;
        EvaluateChoice(2);
    }

    private void OnButtonWest()
    {
        Debug.Log("OnButtonWest");
        if (m_gameState is GameState.Onboarding or GameState.End) return;
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
        //m_fogs.ShowSurroundFogAt(true, false);
        //m_fogs.ShowCentreFogAt(true);
        
        //HIDE ONBOARDING/CURRENT GATE CONTENTS
        //SHOW NEW GATE CONTENTS
        m_gameState++;
        
        switch (m_gameState)
        {
            case GameState.Gate2:
                m_gate_1_contents.gameObject.SetActive(false);
                m_gate_2_contents.gameObject.SetActive(true);
                break;
            case GameState.Gate3:
                m_gate_2_contents.gameObject.SetActive(false);
                m_gate_3_contents.gameObject.SetActive(true);
                break;
            case GameState.Gate4:
                m_gate_3_contents.gameObject.SetActive(false);
                m_gate_4_contents.gameObject.SetActive(true);
                break;
            case GameState.End:
                m_gate_1_contents.gameObject.SetActive(false);
                m_gate_2_contents.gameObject.SetActive(false);
                m_gate_3_contents.gameObject.SetActive(false);
                m_gate_4_contents.gameObject.SetActive(false);
                break;
            default:
                break;
        }

    }
}