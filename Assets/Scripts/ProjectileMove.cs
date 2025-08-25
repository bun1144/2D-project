using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public Vector3 velocity;
    public int damage;
    public string shooterTag;

    public float minX = -10f, maxX = 10f;
    public float minY = -6f, maxY = 6f;

    private bool isDestroyed = false; // ป้องกัน Destroy ซ้ำ

    void Update()
    {
        if (isDestroyed) return;

        transform.position += velocity * Time.deltaTime;

        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.y < minY || transform.position.y > maxY)
        {
            Destroy(gameObject);
            isDestroyed = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (other == null) return;

    // ✅ กันยิงโดนตัวเอง
    if (!string.IsNullOrEmpty(shooterTag) && other.CompareTag(shooterTag))
        return;

    Destroy(gameObject);

    var hp = other.GetComponent<Health>();
    if (hp != null)
    {
        hp.TakeDamage(damage);
    }
}

}
