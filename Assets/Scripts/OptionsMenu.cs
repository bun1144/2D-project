using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu"); // กลับไปหน้าเมนูหลัก
    }

    public void ToggleMusic()
    {
        Debug.Log("Music button clicked!");
        // ตรงนี้สามารถใส่โค้ดเปิด/ปิดเพลงได้
    }

    public void ToggleSound()
    {
        Debug.Log("Sound button clicked!");
        // ตรงนี้สามารถใส่โค้ดเปิด/ปิดเอฟเฟกต์เสียงได้
    }
}
