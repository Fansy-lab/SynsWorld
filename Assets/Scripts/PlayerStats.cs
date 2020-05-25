using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update

    public int DPS;

    public int maxHealth;
    public int armor;
    public int evasion;

    public int currentHealth;

    public int gold;
    public int experience;

    public HealthBar hpBar;
    public bool insideALocation=true;
    public int maxItemsCanHold;
    public int maxItemsCanHoldInPrivateStash;
    PlayerInput playerInput;
    [SerializeField] GameObject levelUpEffect;

    void Start()
    {
        GlobalEvents.OnEnemyDeath += EnemyDied;
        GlobalEvents.OnLevelUp += PlayerLeveledUp;

        playerInput = GetComponentInChildren<PlayerInput>();
        currentHealth = maxHealth;
        SetMaxHealth();
    }

    public void SetMaxHealth()
    {
        hpBar.SetMaxHealth(maxHealth);
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
        currentHealth -= damage;
        hpBar.SetHealth(currentHealth);
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
        float hpToRecover = maxHealth * 0.25f;
        currentHealth += Convert.ToInt32(hpToRecover);
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        hpBar.SetHealth(currentHealth);

    }
    public void PlayerLeveledUp(int? level)
    {
       GameObject effect = Instantiate(levelUpEffect, transform.position, Quaternion.identity) as GameObject;
        effect.transform.SetParent(gameObject.transform);


    }
    public void EnemyDied(IEnemy enemy)
    {
        LevelSystem.AddExp(enemy.Experience);


    }
}
