using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int currentHealth;
    public int maxHealth;
    public string Name;
    public Animator animator;
    public HealthBar hp;
    public GameObject damagePopUp;
    void Start()
    {
        currentHealth = maxHealth;
        hp.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Projectile")
        {
            Projectile projectile = collision.transform.GetComponent<Projectile>();
            int damageAmmount = projectile.damageAmmount;
            animator.SetTrigger("takeDamage");
            TakeDamage(damageAmmount);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }


    public void TakeDamage(int damage)
    {
        if(damagePopUp) ShowFloatingTextDamage(damage);

        currentHealth -= damage;
        hp.SetHealth(currentHealth);
    }

    private void ShowFloatingTextDamage(int damage)
    {
        GameObject gO= Instantiate(damagePopUp, transform.position, Quaternion.identity,transform) as GameObject;
        gO.GetComponentInChildren<DamagePopUp>().damageAmmount = damage;
    }
}
