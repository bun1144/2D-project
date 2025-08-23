using System.Collections;
using System.Collections.Generic;

using UnityEngine;




public class Playercontroll : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private float Move;

    public GameObject firePrefab;   // กระสุน
    public Transform firePoint;     // จุดที่ยิงกระสุนออก (ตั้งค่าใน Inspector)
    
    public float speed = 7f;
    public float jump = 12f;
    public int maxJumpCount = 2;

    private int jumpCount = 0;
    private bool grounded;
    private bool facingRight = true; // ตัวละครหันขวาเริ่มต้น

    public float fireRate = 0.5f;   // เวลาหน่วงระหว่างการยิง (วินาที)
    private float nextFireTime = 0f;
   
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true; // ป้องกันล้ม
    }

    void Update()
    {
        // เดินซ้าย/ขวา
        Move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(Move * speed, rb.velocity.y);

        // พลิกตัวละคร
        if (Move > 0 && !facingRight)
            Flip();
        else if (Move < 0 && facingRight)
            Flip();

        // Animation Speed
        if(anim != null)
            anim.SetFloat("speed", Mathf.Abs(Move));

        // Jump (Space หรือ UpArrow)
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow)) 
            && jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump);
            jumpCount++;
            grounded = false;
            if(anim != null)
                anim.SetBool("grounded", false);
        }
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && !grounded)
{
    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 0.3f); // เพิ่มความเร็วตกลงเรื่อยๆ
}

        // Grounded animation
        if (anim != null)
            anim.SetBool("grounded", grounded);

        // **ยิงเมื่อกด Z**
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
}
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // พลิกแกน X ของ Player
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            jumpCount = 0;
            if(anim != null)
                anim.SetBool("grounded", true);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }
}
