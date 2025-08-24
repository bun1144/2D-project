// BossTimerStart.cs
using UnityEngine;
public class BossTimerStart : MonoBehaviour
{
    void Start()
    {
        GameTimer.Ensure().StartNew();
        Debug.Log("[Timer] StartNew at Boss scene");
    }
}
