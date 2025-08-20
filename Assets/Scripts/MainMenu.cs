using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game"; // ตั้งชื่อซีนเกมจริงของคุณ

    public void StartGame()
    {
        // เผื่อกรณีเคย Pause ไว้
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

     public void OpenOptions()
    {
        Time.timeScale = 1f; // กันค้างจาก pause
        SceneManager.LoadScene("OptionsScene");  // <-- ใช้ชื่อซีนจริง
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();

        // สำหรับทดสอบใน Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
