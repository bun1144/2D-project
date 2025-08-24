using UnityEngine;

public class BossMusicManager : MonoBehaviour
{
    [Header("Clips")]
    public AudioClip backgroundMusic;
    public AudioClip bossMusic;
    public AudioClip lowHealthMusic;

    [Header("Refs")]
    public Health bossHealth;

    [Header("Options")]
    public int lowHealthThreshold = 50;
    private bool lowHealthTriggered = false;

    void Start()
    {
        // 0) กันพลาด: ปิด playOnAwake ของ AudioSource ที่เผลอใส่มา
        AudioManager.Instance.DisablePlayOnAwakeInScene();

        // 1) หยุดเพลง loop อื่น ๆ ที่อาจค้างจากซีนก่อน
        AudioManager.Instance.StopOtherLoopingMusic();

        // 2) เล่นเพลงบอส (หรือเพลงบรรยากาศ)
        if (bossMusic != null)
            PlayMusic(bossMusic);
        else if (backgroundMusic != null)
            PlayMusic(backgroundMusic);

        if (bossHealth == null) bossHealth = FindObjectOfType<Health>();
    }

    void Update()
    {
        if (!lowHealthTriggered && bossHealth != null)
        {
            if (bossHealth.currentHealth <= lowHealthThreshold)
            {
                if (lowHealthMusic != null)
                {
                    PlayMusic(lowHealthMusic);
                    lowHealthTriggered = true;
                }
            }
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        AudioManager.Instance.PlayMusic(clip, true);
    }

    public void OnBossSceneStart()
    {
        if (bossMusic != null) PlayMusic(bossMusic);
    }
}
