using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneCode
{
    Title,
    Dungeon,
    Battle,
    End,
    Start
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void LoadScene(SceneCode sceneCode)
    {
        SceneManager.LoadScene(sceneCode.ToString());
    }
}
