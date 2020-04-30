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

    public event Action<IEnemy> OnEnemyDeath;
    public int ID { get; set; }
    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public int GoldReward { get; set; }
    public int Experience { get; set; }





    void Start()
    {
        ID = 1;
        Name ="Skeleton";
        GoldReward = 1;
        Experience = 2;

        currentHealth = _maxHealth;
        hp.SetMaxHealth(_maxHealth);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Projectile")
        {
            Projectile projectile = collision.transform.GetComponent<Projectile>();
            int damageAmmount = projectile.damageAmmount;
            TakeDamage(damageAmmount);
            if (currentHealth <= 0)
            {
                GetComponent<BoxCollider2D>().enabled = false;
                animator.SetTrigger("death");
                GlobalEvents.EnemyDied(this);//event happened

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

      
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (damagePopUp) ShowFloatingTextDamage(damage);

        currentHealth -= damage;
        hp.SetHealth(currentHealth);
    }
}
