using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class MainMenu : MonoBehaviour
{
   

    public void StartGame()
    {
        Time.timeScale = 1f;
        LoadSceneSafe("SampleScene");
    }

    public void OpenOptions()
    {
        Time.timeScale = 1f;
        LoadSceneSafe("OptionsScene"); // ตั้งชื่อให้ตรงกับซีนจริง
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void LoadSceneSafe(string sceneName)
    {
        bool exists = Enumerable.Range(0, SceneManager.sceneCountInBuildSettings)
            .Select(SceneUtility.GetScenePathByBuildIndex)
            .Select(System.IO.Path.GetFileNameWithoutExtension)
            .Any(n => n == sceneName);

        if (!exists)
        {
            Debug.LogError(
                $"Scene '{sceneName}' ไม่ได้ถูกเพิ่มใน Build Settings หรือสะกดไม่ตรง\n" +
                $"ไปที่ File → Build Settings… แล้ว Add scene นี้เข้า 'Scenes In Build'"
            );
            return;
        }

        SceneManager.LoadScene(sceneName);
        // ถ้าอยากนุ่มขึ้น เปลี่ยนเป็น:
        // StartCoroutine(LoadAsync(sceneName));
    }

    // ตัวเลือก: โหลดแบบ Async (ถ้าอยากลื่นขึ้น)
    private System.Collections.IEnumerator LoadAsync(string sceneName)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;
    }
}
