using UnityEngine;

public class BoggartSceneManager : MonoBehaviour
{
    [HideInInspector]
    public BoggartSceneManager Instance;
    
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
}