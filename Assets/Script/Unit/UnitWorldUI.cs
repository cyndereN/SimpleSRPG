using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField]private Slider healthSlider;
    [SerializeField] private HealthSystem healthSystem;
    // Start is called before the first frame update
    void Start()
    {
        healthSystem.UnitDamage += HSChangeSlider;
        healthSlider.value = healthSystem.GetHealth();
    }

    private void HSChangeSlider(object sender, EventArgs e)
    {
        healthSlider.value = healthSystem.GetHealth();
    }

}
