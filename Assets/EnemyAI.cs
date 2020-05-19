using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
[RequireComponent(typeof(IEnemy))]
public class EnemyAI : MonoBehaviour
{
    GameObject target;
    public float speed;
    public float nextWayPointDistance;
    public bool dead = false;

    Path path;
    int currentWayPoint = 0;

    Animator anim;
    Seeker seeker;
    float moveRadius;
    GameObject moveCenter;

    bool retreatingToStart = true;
    bool playerInRange;
    bool hasReseted = true;
    bool reachedEndOfPath;

    IEnemy thisEnemy;

    GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        thisEnemy = GetComponent<IEnemy>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.25f);
    }


    private void Update()
    {
        if (Vector3.Distance(player.transform.position, moveCenter.transform.position) <= moveRadius)
        {
            if (hasReseted)
            {
                playerInRange = true;
                target = player;
                retreatingToStart = false;
                thisEnemy.CanBeDamaged = true;
                hasReseted = false;
            }

        }
        else
        {

            playerInRange = false;
            target = moveCenter;
            retreatingToStart = true;
            // thisEnemy.CanBeDamaged = false;
        }


    }

    void UpdatePath()
    {
        if (retreatingToStart)
        {
            thisEnemy.TakesReducedDamage = true;
            target = moveCenter;
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (seeker.IsDone() && target != null && distance > 1.5)
            {
                seeker.StartPath(transform.position, target.transform.position, OnPathComplete);

            }
            else
            {
                thisEnemy.RegenHealthToMax();
                thisEnemy.CanBeDamaged = false;
            }
        }
        else
        {
            if (playerInRange)
            {
                thisEnemy.TakesReducedDamage = false;

                float distance = Vector2.Distance(transform.position, target.transform.position);
                if (seeker.IsDone() && target != null && distance > 1.5)
                    seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
            }

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

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(moveCenter.transform.position, moveRadius);


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

        if (currentWayPoint >= path.vectorPath.Count)
        {
            anim.SetBool("walking", false);

            reachedEndOfPath = true;
            if (retreatingToStart)
            {

                hasReseted = true;

            }
            return;
        }
        else
        {
            anim.SetBool("walking", true);
            reachedEndOfPath = false;
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
        if (!dead) //dont rotate if dead
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

    public void SetMaximumMovement(float maxMoveRadius, GameObject center)
    {
        moveRadius = maxMoveRadius;
        moveCenter = center;
    }
}
