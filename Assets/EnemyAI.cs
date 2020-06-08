using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
[RequireComponent(typeof(IEnemy))]
public class EnemyAI : MonoBehaviour
{
    GameObject target;
    public GameObject attackPoint;
    public float attackRange;
    public LayerMask playerLayer;
    public float speed;
    public float nextWayPointDistance;
    public bool dead = false;

    Path path;
    int currentWayPoint = 0;

    Animator anim;
    Seeker seeker;
    float moveRadius;
    public float attakRate;
    public float lastAttack = 0f;
    GameObject moveCenter;

    bool retreatingToStart = true;
    bool playerInsideMovingRange;
    bool hasReseted = true;
    bool reachedPlayer;


    IEnemy thisEnemy;

    GameObject player;

    void Start()
    {
        //    InvokeRepeating("TryFindPlayer", 0, 2f);
        anim = GetComponent<Animator>();
        thisEnemy = GetComponent<IEnemy>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.25f);
    }

    public void TryFindPlayer()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");

        }

    }
    private void Update()
    {
        if (player == null) return;
        if (Vector3.Distance(player.transform.position, moveCenter.transform.position) <= moveRadius)
        {
            if (hasReseted)
            {
                playerInsideMovingRange = true;
                target = player;
                retreatingToStart = false;
                thisEnemy.CanBeDamaged = true;
                hasReseted = false;
            }

        }
        else
        {

            playerInsideMovingRange = false;
            target = moveCenter;
            retreatingToStart = true;
            // thisEnemy.CanBeDamaged = false;
        }


    }

    void UpdatePath()
    {
        TryFindPlayer();
        if (retreatingToStart)
        {
            thisEnemy.TakesReducedDamage = true;
            target = moveCenter;
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (seeker.IsDone() && target != null && distance > 1.1)
            {
                seeker.StartPath(transform.position, target.transform.position, OnPathComplete);

            }
            else
            {
                thisEnemy.RegenHealthToMax();
                thisEnemy.CanBeDamaged = false;
                hasReseted = true;
            }
        }
        else
        {
            if (playerInsideMovingRange)
            {
                thisEnemy.TakesReducedDamage = false;

                if (target != null)
                {
                    float distance = Vector2.Distance(transform.position, target.transform.position);
                    if (seeker.IsDone() && target != null && distance > 1.5)
                        seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
                }
                else
                {
                    retreatingToStart = true;
                }

            }

        }
        RotateTowardsPlayer();
        CheckIfCanAttack();
    }

    private void RotateTowardsPlayer()
    {
        if (!dead) //dont rotate if dead
        {
            if (target != null)
            {
                if (target.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);

                }
            }
        }

    }

    private void CheckIfCanAttack()
    {

        if (lastAttack < attakRate)
        {
            lastAttack += Time.fixedDeltaTime;

        }
        if (lastAttack > attakRate && reachedPlayer && !dead)
        {

            anim.SetTrigger("attack");
            lastAttack = 0;

        }

    }

    public void Attack()
    {
       Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, playerLayer);
        if (colliders.Length > 0)
        {
            foreach (var collider in colliders)
            {
                if (collider.gameObject.tag == "Player")
                {
                    PlayerStats playerStats = collider.GetComponent<PlayerStats>();
                    bool attackMissed =playerStats.DidTheAttackMiss();
                    if (attackMissed == false)
                    {
                        if (playerStats != null)
                        {
                            int damageToDo = playerStats.GetDamageAfterArmorReduction(RNGGod.GetSmallRandomDamage());
                            playerStats.TakeDamage(damageToDo);
                            collider.GetComponent<Animator>().SetTrigger("TakeDamage");
                            SoundEffectsManager.instance.PlaySound(gameObject.GetComponent<IEnemy>().DoDamageSoundEffect);
                        }
                    }
                    else
                    {
                        Vector3 position = new Vector3(collider.gameObject.transform.position.x, collider.gameObject.transform.position.y + 1);
                        NumberPopUpManager.Instance.DisplayEvadeText("Evaded", position);
                        SoundEffectsManager.instance.PlaySound(gameObject.GetComponent<IEnemy>().MissAttackSoundEffect);

                    }


                }
            }
        }
        else
        {
            SoundEffectsManager.instance.PlaySound(gameObject.GetComponent<IEnemy>().MissAttackSoundEffect);

        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        GoToDestination();

    }



    private void GoToDestination()
    {
        if (path == null)
        {
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count && retreatingToStart)//reached home/spawn
        {
            anim.SetBool("walking", false);
            hasReseted = true;
            path = null;
            return;

        }
        else if (currentWayPoint >= path.vectorPath.Count && !retreatingToStart  )//reached player/target
        {
            anim.SetBool("walking", false);
            reachedPlayer = true;
            return;
        }
        else
        {
            anim.SetBool("walking", true);
            reachedPlayer = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - (Vector2)transform.position).normalized;
        if (!dead) //dont move if dead
            transform.Translate(direction * Time.deltaTime * speed);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWayPoint]);
        float distanceMaxMovement = Vector3.Distance(transform.position, moveCenter.transform.position);
        if (distanceMaxMovement > moveRadius)
        {
            retreatingToStart = true;

        }
        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }


    }

    public void SetMaximumMovement(float maxMoveRadius, GameObject center)
    {
        moveRadius = maxMoveRadius;
        moveCenter = center;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(moveCenter.transform.position, moveRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);


    }
}
