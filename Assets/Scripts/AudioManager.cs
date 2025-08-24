using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement; // << เพิ่มบรรทัดนี้

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixer")]
    public AudioMixer masterMixer;
    public string musicParam = "MusicVol";
    public string sfxParam   = "SFXVol";

    [Header("Defaults (0..1)")]
    [Range(0,1)] public float defaultMusic = 0.7f;
    [Range(0,1)] public float defaultSfx   = 0.8f;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // subscribe sceneLoaded เพื่อ auto-switch เพลงตามซีน
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Music
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.outputAudioMixerGroup = masterMixer.FindMatchingGroups("Music")[0];

        // SFX
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.outputAudioMixerGroup = masterMixer.FindMatchingGroups("SFX")[0];

        SetMusicVolume(defaultMusic);
        SetSfxVolume(defaultSfx);
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // เมื่อโหลดซีนใหม่ → หา SceneMusic แล้วสั่งเล่นเพลงประจำซีน
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // กันเพลงค้างจากซีนก่อนที่เป็น AudioSource อื่น
        DisablePlayOnAwakeInScene();
        StopOtherLoopingMusic();

        var sceneMusic = FindObjectOfType<SceneMusic>(true);
        if (sceneMusic != null && sceneMusic.music != null)
        {
            PlayMusic(sceneMusic.music, sceneMusic.loop);
        }
    }

    // ====== เพลง (BGM) ======
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();

    // ====== SFX ======
    public void PlaySfx(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    // ====== Volume ======
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

    // ====== Utilities ======
    public void StopOtherLoopingMusic()
    {
        var all = Object.FindObjectsOfType<AudioSource>(true);
        foreach (var src in all)
        {
            if (src == musicSource) continue;
            if (src.isPlaying && src.loop) src.Stop();
        }
    }

    public void DisablePlayOnAwakeInScene()
    {
        var all = Object.FindObjectsOfType<AudioSource>(true);
        foreach (var src in all)
        {
            if (src == musicSource) continue;
            if (src.playOnAwake) src.playOnAwake = false;
        }
    }
}
