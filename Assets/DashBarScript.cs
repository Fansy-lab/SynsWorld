using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashBarScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Image fill;


    public void SetMaxValue(int value)
    {
        slider.maxValue = value;

    }
    public void SetBarToMax(int health)
    {
        slider.maxValue = health;
        slider.value = health;

    }

    public void SetDashBar(float value)
    {
        slider.value = value;

    }
}
