using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerData playerData;
        
    public HealthBar hpBar;
    PlayerInput playerInput;

    void Start()
    {
        GlobalEvents.OnEnemyDeath += EnemyDied;
        playerInput = GetComponentInChildren<PlayerInput>();
        playerData.currentHealth = playerData.maxHealth;
        SetMaxHealth();
    }

    public void SetMaxHealth()
    {
        hpBar.SetMaxHealth(playerData.maxHealth);
    }

    // Update is called once per frame

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);
        }   
    }

    public void TakeDamage(int damage)
    {
        playerData.currentHealth -= damage;
        hpBar.SetHealth(playerData.currentHealth);
    }

    public void EnableShooting()
    {
        playerInput.learnedToShoot = true;
    }
    public void DisableShooting()
    {
        playerInput.learnedToShoot = false;

    }
    public void DrinkPotion()
    {
        float hpToRecover = playerData.maxHealth * 0.25f;
        playerData.currentHealth += Convert.ToInt32(hpToRecover);
        if (playerData.currentHealth > playerData.maxHealth)
            playerData.currentHealth = playerData.maxHealth;
        hpBar.SetHealth(playerData.currentHealth);

    }
    public void EnemyDied(IEnemy enemy)
    {
        playerData.gold += enemy.GoldReward;
        LevelSystem.AddExp(enemy.Experience);
    }
}
