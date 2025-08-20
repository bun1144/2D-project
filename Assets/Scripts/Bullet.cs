using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // ลบกระสุนอัตโนมัติหลัง 2 วิ
    }

    void Update()
    {
        // เดินตามทิศของ localScale.x
        float direction = Mathf.Sign(transform.localScale.x); 
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }
}