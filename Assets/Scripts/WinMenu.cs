using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");   // กลับไปหน้าเมนูหลัก
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Boss");   // เล่นใหม่ที่ฉาก Boss
    }
}
