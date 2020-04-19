using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 100;
    public int currentHealth;
    public int Gold;
    public int Experience;


    public HealthBar hpBar;
    PlayerInput playerInput;

    void Start()
    {
        CombatEvents.OnEnemyDeath += EnemyDied;
        playerInput = GetComponentInChildren<PlayerInput>();
        currentHealth = maxHealth;
        hpBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {


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
    public void EnemyDied(IEnemy enemy)
    {
        Gold += enemy.GoldReward;
        Experience += enemy.Experience;
    }
}
