using Input;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField] private InputReader m_InputReader;
    
    public static ChoiceManager Instance;

    public Gate m_currentGate;
    
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
        SceneManager.LoadScene(m_currentGate.Choices[0].Scene.name);
    }

    private void OnButtonEast()
    {
        Debug.Log("OnButtonEast");
        SceneManager.LoadScene(m_currentGate.Choices[1].Scene.name);
    }

    private void OnButtonWest()
    {
        Debug.Log("OnButtonWest");
        SceneManager.LoadScene(m_currentGate.Choices[2].Scene.name);
    }

    private void OnButtonSouth()
    {
        Debug.Log("OnButtonSouth");
        SceneManager.LoadScene(m_currentGate.Choices[3].Scene.name);
    }
}
