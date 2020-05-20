using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : MonoBehaviour,IEnemy
{
    public int currentHealth;
    public int _maxHealth;

    public Animator animator;
    public HealthBar hp;
    public GameObject damagePopUp;

    public event Action<IEnemy> OnEnemyDeath;
    public int ID { get; set; } = 2;
    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public int GoldReward { get; set; }
    public int Experience { get; set; }
    public bool CanBeDamaged { get; set; }
    public bool TakesReducedDamage { get; set; }
    public Spawner spawner { get; set; }

    [SerializeField]
    public LootTable MylootTable;

    public LootTable lootTable
    {
        get { return MylootTable; }
        set { MylootTable = value; }
    }


    void Awake()
    {
        ID = 2;
        Name = "Bear";
        GoldReward = 2;
        Experience = 5;

        currentHealth = _maxHealth;
        hp.SetMaxHealth(_maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Projectile")
        {
            if (!CanBeDamaged) return;

            Projectile projectile = collision.transform.GetComponent<Projectile>();
            int damageAmmount = projectile.damageAmmount;
            if (TakesReducedDamage)
                damageAmmount = RNGGod.ReduceDamage(damageAmmount);
            TakeDamage(damageAmmount);
            if (currentHealth <= 0)
            {
                GetComponent<CircleCollider2D>().enabled = false;
                animator.SetTrigger("death");
                GlobalEvents.EnemyDied(this);//event happened
                GetComponent<EnemyAI>().dead = true;

                StartCoroutine(StartDeath());
            }
            else
            {
                animator.SetTrigger("takeDamage");

            }
        }
    }

    IEnumerator StartDeath()
    {

        yield return new WaitForSeconds(1f);
        Die();
    }


    void DropLoot()
    {
        int numberOfItemsToDrop = UnityEngine.Random.Range(UnityEngine.Random.Range(1, 2), 3);
        int numberOfGoldec = UnityEngine.Random.Range(UnityEngine.Random.Range(1, 2), 6);

        if (lootTable != null)
        {
            //items loot
            for (int i = 0; i < numberOfItemsToDrop; i++)
            {
                PhysicalInventoryItem item = lootTable.LootItem();
                if (item != null)
                {
                    Vector2 position = new Vector2(transform.position.x + (float)(UnityEngine.Random.Range(-0.35f, 0.35f)), transform.position.y + (float)(UnityEngine.Random.Range(-0.35f, 0.35f)));
                    Instantiate(item.gameObject, position, Quaternion.identity);
                }
            }

            //gold loot
            for (int i = 0; i < numberOfGoldec; i++)
            {
                PhysicalInventoryItem gold = lootTable.LootGold();
                if (gold != null)
                {

                    Vector2 position = new Vector2(transform.position.x + (float)(UnityEngine.Random.Range(-0.35f, 0.35f)), transform.position.y + (float)(UnityEngine.Random.Range(-0.35f, 0.35f)));
                    Instantiate(gold.gameObject, position, Quaternion.identity);


                }
            }

        }
    }


    private void ShowFloatingTextDamage(int damage)
    {
        Vector3 position = gameObject.transform.position;
        position.y = gameObject.transform.position.y + 1f;
        Vector3 randomizeIntesity = new Vector3(0.15f, 0.0f, 0);
        position += new Vector3(UnityEngine.Random.Range(-randomizeIntesity.x, randomizeIntesity.x), UnityEngine.Random.Range(-randomizeIntesity.y, randomizeIntesity.y), UnityEngine.Random.Range(-randomizeIntesity.z, randomizeIntesity.z));

        GameObject gO = Instantiate(damagePopUp, position, Quaternion.identity) as GameObject;
        gO.GetComponentInChildren<DamagePopUp>().damageAmmount = damage;
    }

    public void Die()
    {
        DropLoot();
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (damagePopUp) ShowFloatingTextDamage(damage);

        currentHealth -= damage;
        hp.SetHealth(currentHealth);
    }

    public void RegenHealthToMax()
    {
        currentHealth = _maxHealth;
        hp.SetMaxHealth(_maxHealth);
    }
  
}
