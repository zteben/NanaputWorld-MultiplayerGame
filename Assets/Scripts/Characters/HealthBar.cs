using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void ChangeColor(Color32 color)
    {
        fill.color = color;
    }
}
