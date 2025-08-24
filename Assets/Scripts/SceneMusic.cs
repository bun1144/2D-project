using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    [Header("เพลงประจำซีนนี้")]
    public AudioClip music;
    public bool loop = true;

    void Start()
    {
        // เผื่อกรณีเข้าซีนนี้แบบ additive หรือมีการ reload ใน runtime
        if (AudioManager.Instance != null && music != null)
        {
            AudioManager.Instance.PlayMusic(music, loop);
        }
    }
}
