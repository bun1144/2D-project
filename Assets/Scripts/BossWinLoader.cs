using UnityEngine;
using UnityEngine.SceneManagement;

public class BossWinLoader : MonoBehaviour
{
    public string winSceneName = "Win";

    // เรียกเมื่อตาย/HP หมด
    public void OnBossDefeated()
    {
        if (GameTimer.Instance != null) GameTimer.Instance.StopAndSave();
        SceneManager.LoadScene(winSceneName);
    }
}
