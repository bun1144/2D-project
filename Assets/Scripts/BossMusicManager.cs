using UnityEngine;

public class BossMusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip backgroundMusic;
    public AudioClip bossMusic;
    public AudioClip lowHealthMusic;
    public Health bossHealth;              // Reference บอส
    private bool lowHealthTriggered = false;

   
    void Start()
    {
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        PlayMusic(backgroundMusic);
    }

     void Update()
    {
        if (bossHealth != null && bossHealth.currentHealth <= 50 && !lowHealthTriggered)
        {
            PlayMusic(lowHealthMusic);
            lowHealthTriggered = true;
        }
    }



    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.volume = 0.1f; // ค่า default เต็มเสียง
        audioSource.Play();
    }

    public void OnBossSceneStart()
    {
        PlayMusic(bossMusic);
    }

   

    // 🔹 ฟังก์ชันเบาเสียง
    public void SetVolume(float value)
    {
        audioSource.volume = Mathf.Clamp01(value); // จำกัดไม่ให้เกิน 0–1
    }
}
