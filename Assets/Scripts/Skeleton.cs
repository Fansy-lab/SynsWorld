using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour,IEnemy
{
    public int currentHealth;
    public int _maxHealth;

    public Animator animator;
    public HealthBar hp;
    public GameObject damagePopUp;
    public AudioClip damageSound;
    public AudioClip missAttackSound;

    public event Action<IEnemy> OnEnemyDeath;
    public int ID { get; set; }
    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public int Experience { get; set; }
    public bool CanBeDamaged { get; set; }
    public bool TakesReducedDamage { get; set; }
    public Spawner spawner { get; set; }
    public AudioClip DoDamageSoundEffect { get; set; }
    public AudioClip MissAttackSoundEffect { get; set; }


    [SerializeField]
    public LootTable MylootTable;


    public LootTable lootTable
    {
        get { return MylootTable; }
        set { MylootTable = value; }
    }

    void Start()
    {
        ID = 1;
        Name ="Skeleton";

        Experience = 2;
        DoDamageSoundEffect = damageSound;
        MissAttackSoundEffect = missAttackSound;
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
                GetComponent<BoxCollider2D>().enabled = false;
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

        if (lootTable!=null)
        {
            //items loot
            for (int i = 0; i < numberOfItemsToDrop; i++)
            {
                PhysicalInventoryItem item = lootTable.LootItem();
                if (item != null)
                {
                    Vector2 position =new Vector2(transform.position.x+(float)(UnityEngine.Random.Range(-0.35f,0.35f)),transform.position.y+(float)(UnityEngine.Random.Range(-0.35f,0.35f)));
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
        Vector3 positionPopUp = gameObject.transform.position;
        positionPopUp.y += 1;
        NumberPopUpManager.Instance.DisplayDamageDone(damage.ToString(), positionPopUp);
    }

    public void Die()
    {
        DropLoot();
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {

            ShowFloatingTextDamage(damage);

        currentHealth -= damage;
        hp.SetHealth(currentHealth);
    }
    public void RegenHealthToMax()
    {
        currentHealth = _maxHealth;
        hp.SetMaxHealth(_maxHealth);
    }
}
