using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private Vector2 scrollSpeed = new Vector2(0.1f, 0f);
    private RawImage rawImage;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    void Update()
    {
        Rect uv = rawImage.uvRect;
        uv.x += scrollSpeed.x * Time.unscaledDeltaTime; // เลื่อนแกน X
        uv.y += scrollSpeed.y * Time.unscaledDeltaTime; // เผื่ออยากเลื่อนแกน Y
        rawImage.uvRect = uv;
    }
}
