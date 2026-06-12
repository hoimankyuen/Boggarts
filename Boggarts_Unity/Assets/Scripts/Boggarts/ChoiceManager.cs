using System.Collections.Generic;
using Input;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioClip m_onboardingAudio;
    [SerializeField] private AudioClip m_dogBarkAudio;
    [SerializeField] private AudioClip m_whatsMyNameAudio;
    [SerializeField] private AudioClip m_winAudio;
    [SerializeField] private AudioClip m_lossAudio;
    [SerializeField] private InputReader m_InputReader;
    [SerializeField] private Fogs m_fogs;
    [SerializeField] private TorchLights m_torchLights;
    [SerializeField] private Animator m_creditsTextAnimator;
    
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
        m_InputReader.Start += OnStartManually;
        m_InputReader.Reset += ResetGame;
        
        m_gameState = GameState.Onboarding;
        
        //SHOW FOG EFFECTS
        
        Invoke(nameof(TransitionToGateOne), 20f);
    }

    #region ButtonInputs

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

    #endregion
    
    
    private void OnStartManually()
    {
        if (m_gameState != GameState.Onboarding) return;
        CancelInvoke(nameof(TransitionToGateOne));
        TransitionToGateOne();
    }

    private void TransitionToGateOne()
    {
        m_gameState = GameState.Gate1;
        m_currentGate = m_gates[0];
        m_audioSource.clip = m_onboardingAudio;
        m_audioSource.Play();
        //m_fogs.ShowSurroundFogAt(true, false);
        //m_fogs.ShowCentreFogAt(true);
        m_torchLights.ShowLight(true, Vector3.zero);
        
        //HIDE ONBOARDING/CURRENT GATE CONTENTS
        m_onboarding.gameObject.SetActive(false);
        m_gate_1_contents.gameObject.SetActive(true);
    }

    private void EvaluateChoice(int choice)
    {
        if (m_gameState is GameState.Onboarding or GameState.End)
        {
            return;
        }

        m_audioSource.clip = m_currentGate.Choices[choice].Sound;
        m_audioSource.Play();
        
        Debug.Log("You chose " + m_currentGate.Choices[choice] + "!");
        
        if (m_currentGate.Choices[choice].IsGood)
        {
            TransitionToNextGate(true, choice);
            Debug.Log("GOOD CHOICE");
        }
        else
        {
            TransitionToNextGate(false, choice);
            Debug.Log("BAD CHOICE");
        }
    }

    private void TransitionToNextGate(bool angry, int choice)
    {
        m_fogs.ShowSwirlingAt(true, angry, Vector3.zero);
        m_torchLights.ShowLight(false, Vector3.zero);

        //m_fogs.ShowSurroundFogAt(true, false);
        //m_fogs.ShowCentreFogAt(true);
        
        //HIDE ONBOARDING/CURRENT GATE CONTENTS
        //SHOW NEW GATE CONTENTS

        if (m_gameState != GameState.End)
        {
            m_gameState++;
            
            if (m_gameState != GameState.End)
            {
                m_currentGate = m_gates[(int)(m_gameState - 1)];
            }
        }

        switch (m_gameState)
        {
            case GameState.Gate2:
                m_gate_1_contents.gameObject.SetActive(false);
                m_gate_2_contents.gameObject.SetActive(true);
                m_audioSource.clip = m_dogBarkAudio;
                m_audioSource.Play();
                break;
            case GameState.Gate3:
                m_gate_2_contents.gameObject.SetActive(false);
                m_gate_3_contents.gameObject.SetActive(true);
                break;
            case GameState.Gate4:
                m_audioSource.clip = m_whatsMyNameAudio;
                m_audioSource.Play();
                m_gate_3_contents.gameObject.SetActive(false);
                m_gate_4_contents.gameObject.SetActive(true);
                break;
            case GameState.End:
                if (angry)
                {
                    m_audioSource.clip = m_winAudio;
                    m_audioSource.Play();
                }
                else
                {
                    m_audioSource.clip = m_lossAudio;
                    m_audioSource.Play();
                }

                m_gate_1_contents.gameObject.SetActive(false);
                m_gate_2_contents.gameObject.SetActive(false);
                m_gate_3_contents.gameObject.SetActive(false);
                m_gate_4_contents.gameObject.SetActive(false);
                m_creditsTextAnimator.Play("CreditsPlay");
                break;
        }
    }

    private void ResetGame()
    {
        m_gameState = GameState.Onboarding;
        m_gate_1_contents.gameObject.SetActive(false);
        m_gate_2_contents.gameObject.SetActive(false);
        m_gate_3_contents.gameObject.SetActive(false);
        m_gate_4_contents.gameObject.SetActive(false);
        m_onboarding.gameObject.SetActive(true);
        m_fogs.ShowAreaFog(false);
        m_fogs.ShowSurroundFogAt(false, false);
        m_fogs.ShowSwirlingAt(false, false, Vector3.zero);
        m_torchLights.ShowLight(false, Vector3.zero);
        m_audioSource.Stop();
        Invoke(nameof(TransitionToGateOne), 20f);
    }
}