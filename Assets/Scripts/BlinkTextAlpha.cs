using UnityEngine;
using TMPro;

public class BlinkTextAlpha : MonoBehaviour
{
    public float speed = 2f; // ความเร็วกระพริบ (2 = 0.5 วิ)
    private TextMeshProUGUI textUI;
    private Color originalColor;

    void Start()
    {
        textUI = GetComponent<TextMeshProUGUI>();
        originalColor = textUI.color;
    }

    void Update()
    {
        float alpha = (Mathf.Sin(Time.time * Mathf.PI * speed) + 1f) / 2f; 
        textUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }
}
