using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform[] teleportPoints;  // จุดที่ Boss สามารถวาร์ปได้ (ตั้งค่าใน Inspector)
    public float teleportCooldown = 2f; // เวลาหน่วงก่อนวาร์ปอีกครั้ง
    private Animator anim;

    private bool canTeleport = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(TeleportLoop());
    }

    IEnumerator TeleportLoop()
    {
        while (true)
        {
            if (canTeleport && teleportPoints.Length > 0)
            {
                // เล่นอนิเมชันวาร์ปออก (ถ้ามี)
                if (anim != null)
                    anim.SetTrigger("disappear");

                yield return new WaitForSeconds(0.5f); // หน่วงก่อนหาย (ให้อนิเมชันเล่น)

                // สุ่มจุดใหม่
                int randIndex = Random.Range(0, teleportPoints.Length);
                transform.position = teleportPoints[randIndex].position;

                // เล่นอนิเมชันวาร์ปเข้า (ถ้ามี)
                if (anim != null)
                    anim.SetTrigger("appear");
            }

            yield return new WaitForSeconds(teleportCooldown);
        }
    }
}
