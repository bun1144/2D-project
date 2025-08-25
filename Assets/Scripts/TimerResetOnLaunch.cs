using UnityEngine;

public static class TimerResetOnLaunch
{
    // จะรันอัตโนมัติก่อนซีนแรกทุกครั้งที่เปิดเกม/กด Play ใน Editor
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetTimesOnAppLaunch()
    {
        PlayerPrefs.DeleteKey(GameTimer.LastKey);
        PlayerPrefs.DeleteKey(GameTimer.BestKey);
        PlayerPrefs.Save();
        Debug.Log("[Timer] Reset times on app launch");
    }
}
