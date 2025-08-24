// WinTimeDisplay.cs
using UnityEngine;
using TMPro;
public class WinTimeDisplay : MonoBehaviour
{
    public TMP_Text clearTimeText;
    public TMP_Text bestTimeText;

    void Start()
    {
        float last = PlayerPrefs.GetFloat(GameTimer.LastKey, -1f);
        float best = PlayerPrefs.GetFloat(GameTimer.BestKey, -1f);

        if (clearTimeText)
            clearTimeText.text = (last >= 0f) ? $"Clear Time: {GameTimer.Format(last)}"
                                              : "Clear Time: --:--.--";
    }
}
