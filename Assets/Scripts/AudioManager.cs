using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Mixer")]
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private string musicParam = "MusicVol"; // ชื่อต้องตรงกับ Exposed Param
    [SerializeField] private string sfxParam = "SFXVol";

    [Header("Defaults (0..1)")]
    [Range(0f, 1f)] public float defaultMusic = 0.7f;
    [Range(0f, 1f)] public float defaultSfx = 0.8f;

    void Awake()
    {
        // Singleton + ข้ามซีน
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // โหลดค่าที่เคยเซฟไว้
        float music = PlayerPrefs.GetFloat("MusicVolume", defaultMusic);
        float sfx   = PlayerPrefs.GetFloat("SFXVolume", defaultSfx);

        SetMusicVolume(music);
        SetSfxVolume(sfx);
    }

    // v: 0..1 → dB
    public void SetMusicVolume(float v)
    {
        PlayerPrefs.SetFloat("MusicVolume", v);
        masterMixer.SetFloat(musicParam, LinearToDb(v));
    }

    public void SetSfxVolume(float v)
    {
        PlayerPrefs.SetFloat("SFXVolume", v);
        masterMixer.SetFloat(sfxParam, LinearToDb(v));
    }

    // แปลง 0..1 เป็นเดซิเบล: 0 = -80dB (เงียบ), 1 = 0dB
    private float LinearToDb(float v)
    {
        if (v <= 0.0001f) return -80f;         // mute
        return Mathf.Log10(v) * 20f;           // standard
    }

    // ใช้ถ้าต้องอ่านกลับไปตั้งค่า Slider ตอนเปิดหน้า Options
    public float GetMusicVolume01() => PlayerPrefs.GetFloat("MusicVolume", defaultMusic);
    public float GetSfxVolume01()   => PlayerPrefs.GetFloat("SFXVolume", defaultSfx);
}
