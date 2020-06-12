using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_AI : MonoBehaviour
{
    Seeker seeker;
    Path path;
    Animator anim;

    int currentWayPoint = 0;
    public float nextWaypointDistance = 0.8f;
    public Transform[] moveLocations;

    public float speed = 100f;
    bool reachedEndOfPath = false;
    void Start()
    {
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 1);
    }





    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - (Vector2)transform.position).normalized;

        transform.Translate(direction * Time.deltaTime * speed);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(transform.position, GetPosition(), OnPathComplete);

    }

    private Vector3 GetPosition()
    {

        return moveLocations[UnityEngine.Random.Range(0, moveLocations.Length)].position;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
}
