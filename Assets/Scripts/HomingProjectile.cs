using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public int damage = 0;
    public string shooterTag = "Boss";
    public float homingTime = 1.5f; // ตามผู้เล่น 1.5 วิ แล้วยิงตรงต่อไป
private float timer = 0f;

    void Update()
    {
       timer += Time.deltaTime;

    Vector3 dir;

    if (target != null && timer <= homingTime)
    {
        dir = (target.position - transform.position).normalized;
    }
    else
    {
        // หลังหมดเวลาตามเป้าหมาย → ยิงตรงไปตามทิศปัจจุบัน
        dir = transform.right; 
    }

    transform.position += dir * speed * Time.deltaTime;
    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0, 0, angle);

    // ลบกระสุนถ้าออกนอกแมพ
    if (Mathf.Abs(transform.position.x) > 12f || Mathf.Abs(transform.position.y) > 7f)
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;
        if (!string.IsNullOrEmpty(shooterTag) && other.CompareTag(shooterTag)) return;

        var hp = other.GetComponent<Health>();
        if (hp != null)
        {
            hp.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
