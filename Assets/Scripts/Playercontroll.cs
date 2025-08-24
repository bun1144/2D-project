using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroll : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private float Move;

    [Header("Fire")]
    public GameObject firePrefab;   // กระสุน
    public Transform firePoint;     // จุดยิง
    public float fireRate = 0.5f;   // เวลาหน่วงระหว่างยิง (วินาที)
    private float nextFireTime = 0f;

    [Header("Move/Jump")]
    public float speed = 7f;
    public float jump = 12f;
    public int maxJumpCount = 2;
    private int jumpCount = 0;
    private bool grounded;
    private bool facingRight = true;

    [Header("SFX")]
    public AudioClip jumpSfx;
    public AudioClip attackSfx;
    [Tooltip("กันสแปมเสียงสั้น ๆ")]
    [Range(0.01f, 0.2f)] public float sfxMinInterval = 0.05f;
    private float lastJumpSfxTime = -999f;
    private float lastAttackSfxTime = -999f;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb   = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // ป้องกันล้ม
    }

    void Update()
    {
        // เดินซ้าย/ขวา
        Move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(Move * speed, rb.velocity.y);

        // พลิกตัวละคร
        if (Move > 0 && !facingRight)      Flip();
        else if (Move < 0 && facingRight)  Flip();

        // Animation Speed
        if (anim != null) anim.SetFloat("speed", Mathf.Abs(Move));

        // Jump (Space หรือ UpArrow)
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) 
            && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            jumpCount++;
            grounded = false;
            if (anim != null) anim.SetBool("grounded", false);

            // ► เล่นเสียงกระโดด (2D ผ่าน AudioManager → คุมด้วย SfxVolume)
            PlayJumpSfx();
        }

        // กดลงเร่งตก
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && !grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 0.3f);
        }

        // Grounded animation
        if (anim != null) anim.SetBool("grounded", grounded);

        // ยิงเมื่อกด Z
        if (Input.GetKeyDown(KeyCode.Z) && anim != null && Time.time >= nextFireTime)
        {
            anim.SetTrigger("shoot"); // เล่นอนิเมชันยิง

            // กำหนดทิศทางยิงตาม Player หัน
            Vector3 dir = facingRight ? Vector3.right : Vector3.left;

            // สร้างกระสุน
            GameObject bullet = Instantiate(firePrefab, firePoint.position, Quaternion.identity);
            ProjectileMove move = bullet.AddComponent<ProjectileMove>();
            move.velocity = dir * 10f; // ความเร็วกระสุน
            move.shooterTag = "Player";

            // หมุน sprite ให้หันไปทางยิง
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            nextFireTime = Time.time + fireRate;

            // ► เล่นเสียงโจมตี (2D ผ่าน AudioManager → คุมด้วย SfxVolume)
            PlayAttackSfx();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            jumpCount = 0;
            if (anim != null) anim.SetBool("grounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    // ========== SFX helpers ==========
    void PlayJumpSfx()
    {
        if (jumpSfx == null || AudioManager.Instance == null) return;
        if (Time.time - lastJumpSfxTime < sfxMinInterval) return;
        lastJumpSfxTime = Time.time;

        // 2D SFX: ดังเสมอ (แนะนำสำหรับเกม 2D)
        AudioManager.Instance.PlaySfx(jumpSfx);

        // ถ้าอยากให้มีตำแหน่งเสียง ให้ใช้ (positional=false เพื่อยังเป็น 2D):
        // AudioManager.Instance.PlaySfxAt(jumpSfx, transform.position, false);
    }

    void PlayAttackSfx()
    {
        if (attackSfx == null || AudioManager.Instance == null) return;
        if (Time.time - lastAttackSfxTime < sfxMinInterval) return;
        lastAttackSfxTime = Time.time;

        AudioManager.Instance.PlaySfx(attackSfx);
        // หรือแบบกำหนดตำแหน่ง (ยัง 2D):
        // AudioManager.Instance.PlaySfxAt(attackSfx, transform.position, false);
    }

    // ========== รองรับ Animation Event ถ้าต้องการจังหวะตีโดนเป๊ะ ==========
    // ไปที่ Attack.anim → Add Event → เลือกฟังก์ชันนี้
    public void OnAttackSwingEvent() => PlayAttackSfx();
    // ไปที่ Jump.anim (หรือเฟรมเทคออฟ) → Add Event → เลือกฟังก์ชันนี้
    public void OnJumpEvent()        => PlayJumpSfx();
}
