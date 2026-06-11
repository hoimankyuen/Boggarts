using Igloo.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoggartSceneManager : MonoBehaviour
{
    public static BoggartSceneManager Instance;

    [SerializeField] private IglooManager m_iglooManager;
    [SerializeField] private SceneAsset m_gate1;
    
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
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(m_gate1.name);
        }
    }
}