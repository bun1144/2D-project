using UnityEngine;
using TMPro;

public class WinTimeDisplay : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text clearTimeText;   // บรรทัด "Clear"
    public TMP_Text bestTimeText;    // บรรทัด "Best"
    public TMP_Text newBestBadge;    // (ออปชัน) ตัวหนังสือ "NEW BEST!" ถ้ามี

    void Start()
    {
        // อ่านค่าที่บันทึกไว้ตอนชนะบอส
        float last = PlayerPrefs.GetFloat(GameTimer.LastKey, -1f);
        float best = PlayerPrefs.GetFloat(GameTimer.BestKey, -1f);

        // จัดรูปแบบเวลา
        string lastStr = (last >= 0f) ? GameTimer.Format(last) : "--:--.--";
        string bestStr = (best >= 0f) ? GameTimer.Format(best) : "--:--.--";

        if (clearTimeText) clearTimeText.text = $"Clear Time: {lastStr}";
        if (bestTimeText)  bestTimeText .text = $"Best  Time: {bestStr}";

        // โชว์ป้าย NEW BEST! ถ้าเวลาล่าสุดเท่ากับ Best (และมีค่า)
        bool isNewBest = (best > 0f && Mathf.Approximately(last, best));
        if (newBestBadge) newBestBadge.gameObject.SetActive(isNewBest);
    }
}
