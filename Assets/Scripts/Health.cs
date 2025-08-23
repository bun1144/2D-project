using UnityEngine;

public class Health : MonoBehaviour
{
    Animator anim;
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " HP: " + currentHealth);
        anim.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            if (anim != null)
        anim.SetTrigger("die"); // เล่น animation ตาย
         Destroy(gameObject, 2f);
           
        }
    }
}
