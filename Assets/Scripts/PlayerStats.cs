using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update

    public int DPS;
    public float  attackSpeed;
    public int maxHealth;
    public int armor;
    public int evasion;
    public int currentHealth;
    public int gold;
    public int experience;

    public int skillPointsToSpend=10;


    public HealthBar hpBar;
    public bool learnedToShoot = false;

    [SerializeField]public bool insideALocation=true;
    public int maxItemsCanHold;
    public int maxItemsCanHoldInPrivateStash;
    public PlayerInput playerInput;
    [SerializeField] GameObject levelUpEffect;
    [SerializeField] GameObject experiencePopUp;

    void Start()
    {
        GlobalEvents.OnEnemyDeath += EnemyDied;
        GlobalEvents.OnLevelUp += PlayerLeveledUp;
        GlobalEvents.OnGainedExperience += PlayerGainedExperience;
        playerInput = GetComponentInChildren<PlayerInput>();
        currentHealth = maxHealth;
        SetMaxHealth();
    }

    private void PlayerGainedExperience(int exp)
    {
        Vector3 positionPopUp = gameObject.transform.position;
        positionPopUp.y += 1;
        if (exp > 0)
        {
            NumberPopUpManager.Instance.DisplayExperienceGained(exp.ToString(), positionPopUp);

        }

    }

    public void SetMaxHealth()
    {
        hpBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame

    private void Update()
    {

    }

    public void TakeDamage(int damage)
    {

        int damageAfterArmorReduction = GetDamageAfterArmorReduction(damage);


        currentHealth -= damageAfterArmorReduction;
        ShowPopUpDamageTaken(damage);
        hpBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            PlayerDie();
        }
    }

    public int GetDamageAfterArmorReduction(int damage)
    {

        float damageCalculated =(float)damage * (100f / (100f + (float)armor));

        return Convert.ToInt32(damageCalculated);
    }

    private void ShowPopUpDamageTaken(int damage)
    {
        Vector3 positionPopUp = gameObject.transform.position;
        positionPopUp.y += 1;
        NumberPopUpManager.Instance.DisplayDamageTaken("-" + damage, positionPopUp);


    }

    public void PlayerDie()
    {
        gold /= 3;
        LevelSystem.experienceForNextLevel /= 3;

        Destroy(gameObject);

        GM.Instance.RespawnPlayer();

    }



    public void EnableShooting()
    {
        learnedToShoot = true;
    }
    public void DisableShooting()
    {
        learnedToShoot = false;

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
        skillPointsToSpend++;
        GameObject effect = Instantiate(levelUpEffect, transform.position, Quaternion.identity) as GameObject;
        effect.transform.SetParent(gameObject.transform);


    }
    public void EnemyDied(IEnemy enemy)
    {
        LevelSystem.AddExp(enemy.Experience);


    }

    private void OnDestroy()
    {
        GlobalEvents.OnEnemyDeath -= EnemyDied;
        GlobalEvents.OnLevelUp -= PlayerLeveledUp;
        GlobalEvents.OnGainedExperience -= PlayerGainedExperience;

    }

    internal void RecalculateMaxHP()
    {
        hpBar.RecalculateMaxHP(maxHealth);
    }

    internal bool DidTheAttackMiss()
    {
        var chanceToHit =UnityEngine.Random.Range(0f, 100f);
        if(chanceToHit>= (float)evasion/3)
        {
            return false;
        }
        return true;
    }
}
