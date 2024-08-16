using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TMP_Text healthText;
    public void SetHealth(int health, int maxHealth)
    {
        slider.value = health;
        slider.maxValue = maxHealth;
        healthText.text = health.ToString() + " / " + slider.maxValue;

    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

    }

}
