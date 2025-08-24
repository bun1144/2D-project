using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ✅ ต้องใส่ด้วยเพื่อใช้ Coroutine

public class Health : MonoBehaviour
{
    Animator anim;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " HP: " + currentHealth);
        anim.SetTrigger("hurt");
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            anim.SetTrigger("die"); // เล่น animation ตาย
            GetComponent<Collider2D>().enabled = false; //

            if (CompareTag("Boss"))
            {
                Debug.Log("Boss ตาย → ไปหน้า Win");
                StartCoroutine(GoToScene("Win", 2f));
            }
            else if (CompareTag("Player"))
            {
                Debug.Log("Player ตาย → ไปหน้า Lose");
                StartCoroutine(GoToScene("Lose", 2f));
            }
        }
    }

    IEnumerator GoToScene(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay); // 
        SceneManager.LoadScene(sceneName);
    }
}
