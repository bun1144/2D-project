using UnityEngine;
using System.Collections;

public class BossBulletSpawner : MonoBehaviour
{
    private Animator anim;

    [Header("Bullet Prefab")]
    public GameObject bulletPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 10f;   // ใช้ spawn ทุก 10 วินาที
    public int bulletsPerWave = 5;
    public float bulletSpeed = 5f;

    [Header("Area Settings")]
    public float topY = 6f;
    public float minX = -8f, maxX = 8f;
    public float sideYmin = -3f, sideYmax = 3f;

    [Header("Boss Health Settings")]
    public Health bossHealth;           // ใส่ reference บอส
    public float triggerHealth = 50f;   // ยิงเมื่อเลือดบอส ≤ 50

    private bool isSpawning = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (bossHealth != null && bossHealth.currentHealth <= triggerHealth && !isSpawning)
        {
            StartCoroutine(SpawnLoop());
            isSpawning = true;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (bossHealth != null && bossHealth.currentHealth > 0)
        {
            int rand = Random.Range(0, 2);

            if (rand == 0)
                yield return StartCoroutine(SpawnRainAnimation());
            else
                yield return StartCoroutine(SpawnSideAnimation());

            yield return new WaitForSeconds(spawnInterval); // รอ 10 วิ ก่อนยิงท่าถัดไป
        }
    }

   IEnumerator SpawnRainAnimation()
{
    if (anim != null)
    {
        Debug.Log("Set air = true");
        anim.SetBool("air", true);
    }

    yield return new WaitForSeconds(1f); // รอให้ animation เล่นก่อนยิง

    SpawnRainBullets(); // ยิงกระสุนจริง

    yield return new WaitForSeconds(0.5f); // รอให้ animation เล่นต่ออีกนิด
    if (anim != null)
    {
        anim.SetBool("air", false);
      //  Debug.Log("Set air = false");
    }
}

    void SpawnRainBullets()
    {
        for (int i = 0; i < bulletsPerWave; i++)
        {
            float randX = Random.Range(minX, maxX);
            Vector3 spawnPos = new Vector3(randX, topY, 0);

            GameObject b = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

            ProjectileMove move = b.GetComponent<ProjectileMove>();
            if (move == null) move = b.AddComponent<ProjectileMove>();

            move.velocity = Vector3.down * bulletSpeed;
            move.shooterTag = "Boss";

            b.transform.rotation = Quaternion.Euler(0, 0, -90f);

          //  Debug.Log($"SpawnRainBullets at x={randX}");
        }
    }

    IEnumerator SpawnSideAnimation()
{
    if (anim != null)
    {
      //  Debug.Log("Set side = true");
        anim.SetBool("side", true);
    }

    yield return new WaitForSeconds(1f); // ดีเลย์ก่อนยิง

    SpawnSideBullets(); // ยิงกระสุนจริง

    yield return new WaitForSeconds(0.5f); // รอ animation ต่อ
    if (anim != null)
    {
        anim.SetBool("side", false);
       // Debug.Log("Set side = false");
    }
}
    void SpawnSideBullets()
    {
        bool fromLeft = Random.value > 0.5f;
        float x = fromLeft ? minX : maxX;
        float y = Random.Range(sideYmin, sideYmax);

        Vector3 spawnPos = new Vector3(x, y, 0);
        GameObject b = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        ProjectileMove move = b.GetComponent<ProjectileMove>();
        if (move == null) move = b.AddComponent<ProjectileMove>();

        move.velocity = fromLeft ? Vector3.right * bulletSpeed : Vector3.left * bulletSpeed;
        move.shooterTag = "Boss";

        b.transform.rotation = fromLeft ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, 180);

       // Debug.Log($"SpawnSideBullets from {(fromLeft ? "Left" : "Right")} at x={x}, y={y}");
    }
}
