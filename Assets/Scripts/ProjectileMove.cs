using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public Vector3 velocity;
    public int damage ;
    public string shooterTag; // "Player" หรือ "Boss"

    // กำหนดขอบเขตแมพ
    public float minX = -10f, maxX = 10f;
    public float minY = -6f, maxY = 6f;

    void Update()
    {
        // เคลื่อนกระสุน
        transform.position += velocity * Time.deltaTime;

        // ตรวจสอบว่าพ้นขอบแมพหรือไม่
        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.y < minY || transform.position.y > maxY)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (other.CompareTag(shooterTag)) return; // ไม่โดนตัวเอง

        var hp = other.GetComponent<Health>();
        if (hp != null)
        {
            hp.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
