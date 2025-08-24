using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ใช้ Coroutine

public class Health : MonoBehaviour
{
    Animator anim;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    private bool isDead = false; // กันเรียกซ้ำเมื่อโดนดาเมจซ้อน

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} HP: {currentHealth}");

        if (anim != null) anim.SetTrigger("hurt");
        if (healthBar != null) healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;

            if (anim != null) anim.SetTrigger("die");
            var col = GetComponent<Collider2D>();
            if (col) col.enabled = false;

            if (CompareTag("Boss"))
            {
                // ⏱️ หยุดและบันทึกเวลา "ทันที" ที่บอสตาย (ไม่รวมดีเลย์ 2 วิ)
                if (GameTimer.Instance != null)
                {
                    var sec = GameTimer.Instance.StopAndSave();
                    Debug.Log($"[Timer] StopAndSave = {sec:F2}s");
                }

                Debug.Log("Boss ตาย → ไปหน้า Win");
                StartCoroutine(GoToSceneRealtime("Win", 2f));
            }
            else if (CompareTag("Player"))
            {
                Debug.Log("Player ตาย → ไปหน้า Lose");
                StartCoroutine(GoToSceneRealtime("Lose", 2f));
            }
        }
    }

    // ใช้ Realtime หน่วงเวลา เพื่อไม่ติด Time.timeScale (เช่นถ้าคุณ pause เกม)
    IEnumerator GoToSceneRealtime(string sceneName, float delay)
    {
        float t = 0f;
        while (t < delay)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}
