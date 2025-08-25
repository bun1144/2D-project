using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Teleport")]
    public Transform[] teleportPoints;     // จุดวาร์ป
    public float teleportCooldown = 5f;    // หน่วงเวลาก่อนวาร์ปใหม่

    [Header("Attacks")]
    public GameObject bulletPrefab;        // กระสุนปกติ
    public GameObject shockwavePrefab;     // Shockwave Prefab
    public Transform firePoint;            // จุดยิงกระสุน

    [Header("Audio (คุมด้วย SfxVolume)")]
    public AudioClip teleportSfx;          // เสียงตอนวาร์ป
    public AudioClip attackStartSfx;       // เสียงเริ่มท่าโจมตี (ร่วมกันได้)
    public AudioClip shockwaveSfx;         // เสียงเฉพาะท่า Shockwave
    public AudioClip spreadShotSfx;        // เสียงเฉพาะท่า Spread Shot
    [Tooltip("กันสแปมเสียง สั้น ๆ หน่วย:วินาที")]
    [Range(0.01f, 0.2f)] public float sfxMinInterval = 0.05f;

    private float lastSfxTime = -999f;

    private Animator anim;
    private Transform player;              // อ้างอิงผู้เล่น
    private Vector3 originalScale;         // เก็บขนาดเดิมของ Boss
    public float initialDelay = 20f;

    private BossMusicManager musicManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        originalScale = transform.localScale;

        musicManager = FindObjectOfType<BossMusicManager>();
        if (musicManager != null) musicManager.OnBossSceneStart();

        StartCoroutine(BossPatternLoop());
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

                // ► เล่นเสียงวาร์ป (ผ่าน AudioManager → SFX group)
                PlaySfx(teleportSfx);
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

        // ► เสียงเริ่มท่า (ถ้ามี)
        PlaySfx(attackStartSfx);
        // ► เสียงเฉพาะท่า Shockwave (ถ้ามี)
        PlaySfx(shockwaveSfx);

        CreateHomingProjectile(shockwavePrefab, firePoint.position, player, 5f);
    }

    void CreateHomingProjectile(GameObject prefab, Vector3 pos, Transform target, float speed)
    {
        if (prefab == null) return;

        GameObject proj = Instantiate(prefab, pos, Quaternion.identity);
        proj.tag = "Boss";

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

        // ► เสียงเริ่มท่า (ถ้ามี)
        PlaySfx(attackStartSfx);
        // ► เสียงเฉพาะท่า Spread (ถ้ามี)
        PlaySfx(spreadShotSfx);

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
    if (prefab == null) return;

    GameObject proj = Instantiate(prefab, pos, Quaternion.identity);

    ProjectileMove move = proj.GetComponent<ProjectileMove>();
    if (move == null) move = proj.AddComponent<ProjectileMove>();

    move.velocity = velocity;
    move.damage = 10;
    move.shooterTag = gameObject.tag;   // ✅ ส่ง tag ของ Boss มาแทน

    Collider2D bossCol = GetComponent<Collider2D>();
    Collider2D projCol = proj.GetComponent<Collider2D>();
    if (bossCol != null && projCol != null)
    {
        Physics2D.IgnoreCollision(projCol, bossCol);
    }
}
    // ==================== SFX Helper ====================
    void PlaySfx(AudioClip clip, float volume01 = 1f)
    {
        if (clip == null) return;
        if (AudioManager.Instance == null) return;

        // กันสแปมสั้น ๆ (หลายเสียงซ้อนในเฟรมเดียว)
        if (Time.time - lastSfxTime < sfxMinInterval) return;
        lastSfxTime = Time.time;

        // เล่นผ่าน AudioManager → ไป Mixer Group "SFX" ⇒ สไลเดอร์ SfxVolume คุมได้
        // ถ้าใน AudioManager ของคุณมี overload รองรับ volumeScale ให้ใช้บรรทัดล่าง:
        // AudioManager.Instance.PlaySfx(clip, Mathf.Clamp01(volume01));
        AudioManager.Instance.PlaySfx(clip);
    }
}
