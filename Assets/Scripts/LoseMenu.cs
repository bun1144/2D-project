using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{
    // ไปหน้าเมนูหลัก
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;               // กันเผื่อเคย pause/ชะลอเวลา
        SceneManager.LoadScene("Menu");
    }

    // เล่นใหม่ เริ่มที่ฉาก Boss
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Boss");
    }
}
