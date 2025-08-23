using UnityEngine;
using UnityEngine.EventSystems;

public class EnsureSingleEventSystem : MonoBehaviour
{
    void Awake()
    {
        var all = FindObjectsOfType<EventSystem>(true);
        if (all.Length > 1)
        {
            // ปิดตัวที่ไม่ใช่อันแรก เพื่อกันชนกันเวลามีซีน Additive
            for (int i = 1; i < all.Length; i++)
                all[i].gameObject.SetActive(false);
        }
    }
}
