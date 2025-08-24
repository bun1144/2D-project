using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    public static SceneTracker Instance;
    public string lastSceneName;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveCurrentScene()
    {
        lastSceneName = SceneManager.GetActiveScene().name;
        // Debug.Log($"Saved last scene: {lastSceneName}");
    }
}
