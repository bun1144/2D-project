// WinSceneMusic.cs
using UnityEngine;

public class WinSceneMusic : MonoBehaviour
{
    [Header("เสียงหน้า Win")]
    public AudioClip winBgm;      // เพลงพื้นหลังหน้า Win (จะเล่นรอบเดียว)
    public AudioClip winJingle;   // เอฟเฟกต์ชนะสั้น ๆ (ออปชัน)

    [Header("ตัวเลือก")]
    public bool stopOtherLooping = true;   // หยุดเพลงค้างจากซีนก่อน

    void Start()
    {
        if (AudioManager.Instance == null) return;

        // กันเสียงค้างจากซีนก่อน
        AudioManager.Instance.DisablePlayOnAwakeInScene();
        if (stopOtherLooping) AudioManager.Instance.StopOtherLoopingMusic();

        // เล่น Jingle (ครั้งเดียว)
        if (winJingle != null)
        {
            AudioManager.Instance.PlaySfx(winJingle);
        }

        // เล่นเพลง Win รอบเดียว (loop = false)
        if (winBgm != null)
        {
            AudioManager.Instance.PlayMusic(winBgm, false);
        }
    }
}
