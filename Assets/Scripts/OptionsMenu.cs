using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Volume Sliders (0..1)")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    // ใช้จำค่าก่อน mute (ถ้าอยากตั้งค่า default อื่น เปลี่ยนตรงนี้ได้)
    private const string PrevMusicKey = "PrevMusicVolume";
    private const string PrevSfxKey   = "PrevSfxVolume";
    private const float DefaultMusic  = 0.7f;
    private const float DefaultSfx    = 0.8f;

    void Awake()
    {
        // กัน Inspector ลืมลาก
        if (!musicSlider) Debug.LogWarning("[OptionsMenu] musicSlider not assigned.");
        if (!sfxSlider)   Debug.LogWarning("[OptionsMenu] sfxSlider not assigned.");
    }

    void OnEnable()
    {
        // เวลากลับเข้าหน้านี้ ให้ซิงก์ค่า UI กับ AudioManager เสมอ
        SyncUIWithAudio();
    }

    void Start()
    {
        // ผูกอีเวนต์ครั้งเดียว
        if (musicSlider)
        {
            musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }
        if (sfxSlider)
        {
            sfxSlider.onValueChanged.RemoveListener(OnSfxChanged);
            sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        }
    }

    private void SyncUIWithAudio()
    {
        if (AudioManager.Instance == null) return;

        if (musicSlider)
            musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume01());
        if (sfxSlider)
            sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSfxVolume01());
    }

    // ====== Slider handlers ======
    private void OnMusicChanged(float v)
    {
        if (AudioManager.Instance == null) return;
        AudioManager.Instance.SetMusicVolume(v);
        // เก็บค่าล่าสุดไว้เป็น "ก่อน mute"
        if (v > 0f) PlayerPrefs.SetFloat(PrevMusicKey, v);
    }

    private void OnSfxChanged(float v)
    {
        if (AudioManager.Instance == null) return;
        AudioManager.Instance.SetSfxVolume(v);
        if (v > 0f) PlayerPrefs.SetFloat(PrevSfxKey, v);
    }

    // ====== ปุ่มเปลี่ยนฉาก ======
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    // ====== ปุ่ม Toggle ======
    public void ToggleMusic()
    {
        if (AudioManager.Instance == null) return;

        float cur = AudioManager.Instance.GetMusicVolume01();
        if (cur > 0f)
        {
            // จำค่าไว้ แล้วปิดเสียง
            PlayerPrefs.SetFloat(PrevMusicKey, cur);
            AudioManager.Instance.SetMusicVolume(0f);
            if (musicSlider) musicSlider.SetValueWithoutNotify(0f);
        }
        else
        {
            // คืนค่าก่อน mute (หรือ default ถ้ายังไม่เคยตั้ง)
            float prev = PlayerPrefs.GetFloat(PrevMusicKey, DefaultMusic);
            AudioManager.Instance.SetMusicVolume(prev);
            if (musicSlider) musicSlider.SetValueWithoutNotify(prev);
        }
    }

    public void ToggleSound()
    {
        if (AudioManager.Instance == null) return;

        float cur = AudioManager.Instance.GetSfxVolume01();
        if (cur > 0f)
        {
            PlayerPrefs.SetFloat(PrevSfxKey, cur);
            AudioManager.Instance.SetSfxVolume(0f);
            if (sfxSlider) sfxSlider.SetValueWithoutNotify(0f);
        }
        else
        {
            float prev = PlayerPrefs.GetFloat(PrevSfxKey, DefaultSfx);
            AudioManager.Instance.SetSfxVolume(prev);
            if (sfxSlider) sfxSlider.SetValueWithoutNotify(prev);
        }
    }
}
