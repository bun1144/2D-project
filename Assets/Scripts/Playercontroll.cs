using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Playercontroll : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private float Move;

    public float speed = 7f;
    public float jump = 12f;
    public int maxJumpCount = 2;

    private int jumpCount = 0;
    private bool grounded;
    private bool facingRight = true; // ตัวละครหันขวาเริ่มต้น

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

    // Grounded animation
    if(anim != null)
        anim.SetBool("grounded", grounded);

    // **Shoot animation ครั้งเดียว**
    if (Input.GetKeyDown(KeyCode.Z) && anim != null)
    {
        anim.SetTrigger("shoot"); // ยิงครั้งเดียวเมื่อกด Z
    }
}
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // พลิกแกน X
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
