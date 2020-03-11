using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar hpBar;
    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponentInChildren<PlayerMovement>();
        currentHealth = maxHealth;
        hpBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(5);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        hpBar.SetHealth(currentHealth);
    }

    public void EnableShooting()
    {
        playerMovement.learnedToShoot = true;
    }
    public void DisableShooting()
    {
        playerMovement.learnedToShoot = false;

    }
}
