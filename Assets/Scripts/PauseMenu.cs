using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject pauseMenuCanvas;   // ทั้ง Canvas
    [SerializeField] private Button defaultSelected;       // ปุ่ม Resume หรือ Restart

    [Header("Panels")]
    [SerializeField] private GameObject panelPause;        // Panel_Pause
    [SerializeField] private GameObject panelSettings;     // Panel_Settings

    [Header("Settings UI")]
    [SerializeField] private Slider musicSlider;           // Slider_Music (0..1)
    [SerializeField] private Slider sfxSlider;             // Slider_SFX (0..1)

    private bool isPaused;

    void Start()
    {
        // ซ่อน UI ตอนเริ่มและกันค้างจากซีนก่อน
        if (pauseMenuCanvas) pauseMenuCanvas.SetActive(false);
        if (panelPause)      panelPause.SetActive(false);
        if (panelSettings)   panelSettings.SetActive(false);
        Time.timeScale = 1f;

        // โหลดค่าจาก AudioManager ใส่ Slider (ถ้ามี)
        if (AudioManager.Instance != null)
        {
            if (musicSlider) musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume01());
            if (sfxSlider)   sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSfxVolume01());
        }

        // ผูกอีเวนต์
        if (musicSlider) musicSlider.onValueChanged.AddListener(OnMusicChanged);
        if (sfxSlider)   sfxSlider.onValueChanged.AddListener(OnSfxChanged);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    // ===== Pause / Resume =====
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        // ⏱️ หยุดตัวจับเวลา (สำคัญ!)
        if (GameTimer.Instance != null) GameTimer.Instance.Pause();

        if (pauseMenuCanvas) pauseMenuCanvas.SetActive(true);
        if (panelPause)      panelPause.SetActive(true);
        if (panelSettings)   panelSettings.SetActive(false);

        FocusDefault();
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        // ⏱️ ให้จับเวลาต่อ (สำคัญ!)
        if (GameTimer.Instance != null) GameTimer.Instance.Resume();

        if (panelPause)      panelPause.SetActive(false);
        if (panelSettings)   panelSettings.SetActive(false);
        if (pauseMenuCanvas) pauseMenuCanvas.SetActive(false);
    }

    public void Restart()
    {
        // ออกจาก pause ก่อนโหลดซีนใหม่
        isPaused = false;
        Time.timeScale = 1f;

        // ไม่ต้อง StartNew ที่นี่ ให้ BossTimerStart ในซีน Boss เป็นคนเริ่มจับเวลาใหม่
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        // กลับเมนูหลัก
        isPaused = false;
        Time.timeScale = 1f;

        // ไม่ต้อง stop/save เวลา (ไม่ใช่ Win) — ปล่อยให้ timer หยุด/รีเซ็ตตอนเข้าเมนูตามที่ออกแบบ
        SceneManager.LoadScene("Menu");
    }

    // ===== Settings Panel =====
    public void OpenSettingsPanel()
    {
        // ยัง pause อยู่ แต่สลับไปหน้า Settings
        if (panelPause)    panelPause.SetActive(false);
        if (panelSettings) panelSettings.SetActive(true);
    }

    public void BackFromSettings()
    {
        // กลับไปหน้า Pause
        if (panelSettings) panelSettings.SetActive(false);
        if (panelPause)    panelPause.SetActive(true);
        FocusDefault();
    }

    private void OnMusicChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
            PlayerPrefs.SetFloat("vol.music", value);
        }
    }

    private void OnSfxChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSfxVolume(value);
            PlayerPrefs.SetFloat("vol.sfx", value);
        }
    }

    private void FocusDefault()
    {
        if (defaultSelected)
            EventSystem.current?.SetSelectedGameObject(defaultSelected.gameObject);
    }
}
