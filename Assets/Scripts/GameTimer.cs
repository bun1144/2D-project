// GameTimer.cs
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    // PlayerPrefs keys
    public const string LastKey = "BossLastClearSec";
    public const string BestKey = "BossBestClearSec";

    // สถานะ
    bool running = false;
    bool paused  = false;

    // การนับเวลา (แบบ realtime ไม่โดน timeScale)
    float lastResumeReal = 0f; // เวลาจริงตอน Start/Resume ล่าสุด
    float accumulated    = 0f; // เวลาที่สะสม (ไม่นับช่วงพัก)
    float lastRunSec     = 0f; // เวลารอบล่าสุดหลัง Stop

    public bool IsRunning => running;
    public bool IsPaused  => running && paused;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // เผื่อเรียกสร้างอัตโนมัติ
    public static GameTimer Ensure()
    {
        if (Instance != null) return Instance;
        var go = new GameObject("GameTimer");
        return go.AddComponent<GameTimer>();
    }

    // เริ่มรอบใหม่
    public void StartNew()
    {
        running        = true;
        paused         = false;
        accumulated    = 0f;
        lastRunSec     = 0f;
        lastResumeReal = Time.realtimeSinceStartup;
    }

    // หยุดชั่วคราว (ตอนกด ESC)
    public void Pause()
    {
        if (!running || paused) return;
        accumulated += Time.realtimeSinceStartup - lastResumeReal;
        paused = true;
    }

    // เล่นต่อ (ตอน Resume)
    public void Resume()
    {
        if (!running || !paused) return;
        lastResumeReal = Time.realtimeSinceStartup;
        paused = false;
    }

    // จบ/บันทึกเวลา (ตอนชนะบอสก่อนโหลด Win)
    public float StopAndSave()
    {
        if (!running) return lastRunSec;

        float final = accumulated + (paused ? 0f : (Time.realtimeSinceStartup - lastResumeReal));

        running    = false;
        paused     = false;
        lastRunSec = final;

        PlayerPrefs.SetFloat(LastKey, lastRunSec);
        float best = PlayerPrefs.GetFloat(BestKey, 0f);
        if (best <= 0f || lastRunSec < best)
            PlayerPrefs.SetFloat(BestKey, lastRunSec);
        PlayerPrefs.Save();

        return lastRunSec;
    }

    // เวลาปัจจุบัน
    public float GetElapsedSec()
    {
        if (running)
            return paused ? accumulated : accumulated + (Time.realtimeSinceStartup - lastResumeReal);
        return lastRunSec;
    }

    // รูปแบบเวลา mm:ss.ss
    public static string Format(float sec)
    {
        if (sec < 0f) sec = 0f;
        int mm = Mathf.FloorToInt(sec / 60f);
        float ss = sec - mm * 60f;
        return $"{mm:00}:{ss:00.00}";
    }
}
