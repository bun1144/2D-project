using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer")]
    public AudioMixer masterMixer;
    public string musicParam = "MusicVol";
    public string sfxParam = "SFXVol";

    [Header("Defaults (0..1)")]
    [Range(0f, 1f)] public float defaultMusic = 0.7f;
    [Range(0f, 1f)] public float defaultSfx = 0.8f;

    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // เตรียม AudioSource สำหรับ SFX
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        // ให้เสียงนี้ออกทาง Group SFX ใน Mixer
        sfxSource.outputAudioMixerGroup = masterMixer.FindMatchingGroups("SFX")[0];

        // เซ็ตค่า default
        SetMusicVolume(defaultMusic);
        SetSfxVolume(defaultSfx);
    }

    public void SetMusicVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        masterMixer.SetFloat(musicParam, dB);
    }

    public void SetSfxVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        masterMixer.SetFloat(sfxParam, dB);
    }

    public float GetMusicVolume01()
    {
        masterMixer.GetFloat(musicParam, out float dB);
        return Mathf.Pow(10f, dB / 20f);
    }

    public float GetSfxVolume01()
    {
        masterMixer.GetFloat(sfxParam, out float dB);
        return Mathf.Pow(10f, dB / 20f);
    }

    // ใช้เล่น SFX เช่นเสียงปุ่ม
    public void PlaySfx(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
}
