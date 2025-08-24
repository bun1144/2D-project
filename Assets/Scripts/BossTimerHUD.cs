// BossTimerHUD.cs
using UnityEngine;
using TMPro;

public class BossTimerHUD : MonoBehaviour
{
    public TMP_Text tmpText;
    public string displayFormat = "{0}";         // ใช้ "Time {0}" ก็ได้
    [Range(0.02f,0.5f)] public float updateEvery = 0.1f;
    float nextUpdateAt;

    void Start() => SetText("--:--.--");

    void Update()
    {
        if (Time.unscaledTime < nextUpdateAt) return;
        nextUpdateAt = Time.unscaledTime + updateEvery;

        if (GameTimer.Instance == null) { SetText("--:--.--"); return; }

        float t = GameTimer.Instance.GetElapsedSec();
        SetText(GameTimer.Format(t));
    }

    void SetText(string t) { if (tmpText) tmpText.text = string.Format(displayFormat, t); }
}
