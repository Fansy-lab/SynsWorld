using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DummyEnemy : MonoBehaviour, IEnemy
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public int Experience { get; set; }
    public int GoldReward { get; set; }
    public LootTable lootTable { get; set; }
    public bool CanBeDamaged { get; set; }
    public bool TakesReducedDamage { get; set; }
    public Spawner spawner { get; set; }


    public Animator animator;
    public HealthBar hp;
    public GameObject damagePopUp;

    public int currentHealth;
    public int _maxHealth;


    public void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (damagePopUp) ShowFloatingTextDamage(damage);

        currentHealth -= damage;
        hp.SetHealth(currentHealth);
    }
    private void ShowFloatingTextDamage(int damage)
    {
        Vector3 position = gameObject.transform.position;
        position.y = gameObject.transform.position.y + 1f;
        Vector3 randomizeIntesity = new Vector3(0.15f, 0.0f, 0);
        position += new Vector3(Random.Range(-randomizeIntesity.x, randomizeIntesity.x), Random.Range(-randomizeIntesity.y, randomizeIntesity.y), Random.Range(-randomizeIntesity.z, randomizeIntesity.z));
        GameObject gO = Instantiate(damagePopUp, position, Quaternion.identity) as GameObject;
        gO.GetComponentInChildren<DamagePopUp>().damageAmmount = damage;

    }

    // Start is called before the first frame update
    void Start()
    {
        ID = 0;
        Name = "Dummy";
        currentHealth = _maxHealth;
        hp.SetMaxHealth(_maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
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
                if(animator!=null)
                    animator.SetTrigger("death");
                GlobalEvents.EnemyDied(this);//event happened
                Die();
            }
            else
            {
                if(animator!=null)
                animator.SetTrigger("takeDamage");



            }
        }
    }
    public void RegenHealthToMax()
    {
        currentHealth = _maxHealth;
        hp.SetMaxHealth(_maxHealth);
    }

}
