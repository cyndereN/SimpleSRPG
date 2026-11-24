using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler UnitDead;
    public event EventHandler UnitDamage;

    [SerializeField] private int health = 100;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        UnitDamage?.Invoke(this, EventArgs.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health<=0)
        {
            Die();
        }
        else
        {
            Damage();
        }


    }

    private void Die()
    {
        UnitDead?.Invoke(this, EventArgs.Empty);
    }

    private void Damage()
    {
        animator.SetTrigger("Damage");
        UnitDamage?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealth()
    {
        return (float)health / (float)maxHealth;
    }
}
