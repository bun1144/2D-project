using UnityEngine;

public class LoseSceneMusic : MonoBehaviour
{
    [Header("เสียงหน้า Lose")]
    public AudioClip loseBgm;     // เพลงพื้นหลังแพ้ (จะเล่นรอบเดียว)
    public AudioClip loseJingle;  // เอฟเฟกต์แพ้สั้น ๆ (ออปชัน)

    [Header("ตัวเลือก")]
    public bool stopOtherLooping = true;   // หยุดเพลงที่ค้างจากซีนก่อน

    void Start()
    {
        if (AudioManager.Instance == null) return;

        // กันเสียงค้างจากซีนก่อน
        AudioManager.Instance.DisablePlayOnAwakeInScene();
        if (stopOtherLooping) AudioManager.Instance.StopOtherLoopingMusic();

        // เล่น Jingle (ครั้งเดียว)
        if (loseJingle != null)
        {
            AudioManager.Instance.PlaySfx(loseJingle);
        }

        // เล่นเพลงแพ้ (รอบเดียว ไม่วน)
        if (loseBgm != null)
        {
            AudioManager.Instance.PlayMusic(loseBgm, false);
        }
    }
}
