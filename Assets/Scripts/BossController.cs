using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class BossController : MonoBehaviour
{
    public Transform[] teleportPoints;     // จุดวาร์ป
    public float teleportCooldown = 5f;    // หน่วงเวลาก่อนวาร์ปใหม่

    public GameObject bulletPrefab;        // กระสุนปกติ
    public GameObject shockwavePrefab;     // Shockwave Prefab

    public Transform firePoint;            // จุดยิงกระสุน
    private Animator anim;

    private Transform player;              // อ้างอิงผู้เล่น
    private Vector3 originalScale;
    // เก็บขนาดเดิมของ Boss
    public float initialDelay = 20f;
     private BossMusicManager musicManager;
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // หาผู้เล่น
        originalScale = transform.localScale; // จำขนาดเดิม
        StartCoroutine(BossPatternLoop());
        musicManager = FindObjectOfType<BossMusicManager>();

        // ตอนเข้าซีนบอส → เปลี่ยนเป็นเพลงบอส
        if (musicManager != null)
        {
            musicManager.OnBossSceneStart();
        }
    }

    IEnumerator BossPatternLoop()
    {
        // ✅ รอ initialDelay ก่อนเริ่มยิงครั้งแรก
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            // วาร์ป
            if (teleportPoints.Length > 0)
            {
                int randIndex = Random.Range(0, teleportPoints.Length);
                transform.position = teleportPoints[randIndex].position;

                transform.localScale = originalScale;
                FaceCenter();
            }

            // สุ่มเลือกท่าโจมตี
            int randAttack = Random.Range(0, 2);

            if (randAttack == 0)
                DoShockwave();
            else
                DoSpreadShot();

            yield return new WaitForSeconds(teleportCooldown);
        }
    }
    void FaceCenter()
    {
        // สมมติว่ากลางจออยู่ที่ x = 0
        float centerX = 0f;

        if (transform.position.x > centerX)
        {
            // Boss อยู่ขวา → หันซ้าย (ไปทางกลางจอ)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else
        {
            // Boss อยู่ซ้าย → หันขวา (ไปทางกลางจอ)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    void DoShockwave()
    {
        if (anim != null) anim.SetTrigger("attack");
        CreateHomingProjectile(shockwavePrefab, firePoint.position, player, 5f);

    }

    void CreateHomingProjectile(GameObject prefab, Vector3 pos, Transform target, float speed)
    {
        GameObject proj = Instantiate(prefab, pos, Quaternion.identity);
        proj.tag = "BossBullet";

        // เพิ่ม HomingProjectile script
        HomingProjectile homing = proj.AddComponent<HomingProjectile>();
        homing.target = target;
        homing.speed = speed;

        // ป้องกันชนกับบอสตัวเอง
        Collider2D bossCol = GetComponent<Collider2D>();
        Collider2D projCol = proj.GetComponent<Collider2D>();
        if (bossCol != null && projCol != null)
        {
            Physics2D.IgnoreCollision(projCol, bossCol);
        }
    }





    void DoSpreadShot()
    {
        if (anim != null) anim.SetTrigger("attack");

        int bulletCount = 5;
        float spreadAngle = 60f;

        // ✅ หาทิศตามการหันหน้าของบอส (เข้ากลางจอ)
        Vector2 baseDirection = (transform.localScale.x > 0) ? Vector2.left : Vector2.right;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = -spreadAngle / 2 + (spreadAngle / (bulletCount - 1)) * i;

            // หมุน baseDirection ตามมุม spread
            Vector3 dir = Quaternion.Euler(0, 0, angle) * baseDirection;

            CreateProjectile(bulletPrefab, firePoint.position, dir * 6f);
        }
    }

    void CreateProjectile(GameObject prefab, Vector3 pos, Vector3 velocity)
    {
        GameObject proj = Instantiate(prefab, pos, Quaternion.identity);

        proj.tag = "BossBullet";

        ProjectileMove move = proj.AddComponent<ProjectileMove>();
        move.velocity = velocity;
        // move.damage = 10;

        Collider2D bossCol = GetComponent<Collider2D>();
        Collider2D projCol = proj.GetComponent<Collider2D>();
        if (bossCol != null && projCol != null)
        {
            Physics2D.IgnoreCollision(projCol, bossCol);
        }
    }

    


}
