using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class EnemyAI : MonoBehaviour
{
    GameObject target;
    public float speed;
    public float nextWayPointDistance ;

    Path path;
    int currentWayPoint = 0;
  
    bool reachedEndOfPath;
    Animator anim;
    Seeker seeker;
    public bool dead=false;
    void Start()
    {
        anim = GetComponent<Animator>();
        FindTarget();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }


    void UpdatePath()
    {
        float distance = Vector2.Distance(transform.position, target.transform.position);

        if (seeker.IsDone() && target != null && distance>1.5 )
            seeker.StartPath(transform.position, target.transform.position, OnPathComplete);


    }
    private void FindTarget()
    {
        target = GameObject.Find("Player");
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
        if (path == null)
            return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            anim.SetBool("walking", false);

            reachedEndOfPath = true;
            return;
        }
        else
        {
            anim.SetBool("walking", true);
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - (Vector2)transform.position).normalized;
        if(!dead) //dont move if dead
        transform.Translate(direction * Time.deltaTime * speed);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWayPoint]);
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
}
