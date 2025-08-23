using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;  // อ้างถึง Slider UI
   
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

      
    }

    public void SetHealth(int health)
    {
        slider.value = health;

       
    }
}
