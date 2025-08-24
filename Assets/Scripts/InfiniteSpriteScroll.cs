using UnityEngine;

public class InfiniteSpriteScroll : MonoBehaviour
{
    public float speed = 0.1f;
    private Transform[] layers;
    private float spriteWidth;

    void Awake()
    {
        // ดึงลูกทั้งหมดของ SkyLayer
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }

        // เอาความกว้างจาก SpriteRenderer ของลูกตัวแรก
        spriteWidth = layers[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        foreach (Transform layer in layers)
        {
            layer.position += Vector3.left * speed * Time.deltaTime;

            // ถ้าเลื่อนออกนอกจอ → ย้ายไปต่อด้านขวา
            if (layer.position.x < -spriteWidth)
            {
                layer.position += Vector3.right * spriteWidth * layers.Length;
            }
        }
    }
}
