using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

       fill.color= gradient.Evaluate(slider.normalizedValue);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    internal void RecalculateMaxHP(int maxHealth)
    {
        float percent = slider.value / slider.maxValue;
        slider.maxValue = maxHealth;
        float newValue = maxHealth * percent;
        slider.value = newValue;
    }
}
