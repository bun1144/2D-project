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

        if (pauseMenuCanvas) pauseMenuCanvas.SetActive(true);
        if (panelPause)      panelPause.SetActive(true);
        if (panelSettings)   panelSettings.SetActive(false);

        FocusDefault();
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (panelPause)      panelPause.SetActive(false);
        if (panelSettings)   panelSettings.SetActive(false);
        if (pauseMenuCanvas) pauseMenuCanvas.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        // กลับไปที่หน้าเมนูหลัก
        Time.timeScale = 1f;  // รีเซ็ตเวลา (เผื่อ pause อยู่)
        SceneManager.LoadScene("Menu");
    }

    // ===== Settings Panel =====
    public void OpenSettingsPanel()
    {
        // ยัง pause อยู่ แต่สลับจากเมนูพัก → เมนูตั้งค่า
        if (panelPause)    panelPause.SetActive(false);
        if (panelSettings) panelSettings.SetActive(true);
    }

    public void BackFromSettings()
    {
        // กลับไปหน้าเมนูพักเกม (ยัง timeScale = 0)
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
