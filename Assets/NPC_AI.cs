using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NPC_AI : MonoBehaviour
{
    Seeker seeker;
    Path path;
    public Animator anim;

    int moveIndex;
    int currentWayPoint = 0;
    public float nextWaypointDistance = 0.8f;
    public GameObject moveLocationsParent;
    List<Vector3> definitiveMoveLcoations = new List<Vector3>();
    List<MovePoint> movePoints = new List<MovePoint>();
    public float speed;
    bool reachedEndOfPath = false;
    bool relaxTimeFinished = true;
    public bool playerInRange;
    bool npcMoves=false;
    public float minWaitTime;
    public float maxWaitTIme;

    MovePoint.relaxingLookingDirection nextLookingDirection;

    void Start()
    {
        anim = GetComponent<Animator>();
        moveIndex = 0;
        foreach (Transform item in moveLocationsParent.transform)
        {
            npcMoves = true;
            definitiveMoveLcoations.Add(new Vector3(item.position.x, item.position.y, 1));
            movePoints.Add(item.gameObject.GetComponent<MovePoint>());
        }


        if (npcMoves)
        {
            anim = GetComponent<Animator>();
            seeker = GetComponent<Seeker>();
            InvokeRepeating("UpdatePath", 0f, 1f);

        }


    }
    private void Update()
    {

    }




    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null || npcMoves == false)
            return;

        if (currentWayPoint >= path.vectorPath.Count)
        {

            if (relaxTimeFinished == true)
            {


                GetNewMovePoint();


                reachedEndOfPath = true;
                StartCoroutine(GetRelaxTime());
            }
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        if (relaxTimeFinished == false) return;
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - (Vector2)transform.position).normalized;
        Vector2 direction2 = ((Vector2)definitiveMoveLcoations[moveIndex] - (Vector2)transform.position).normalized;




        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

        if (playerInRange == false)
        {

            anim.SetBool("Move", true);
            if (movePoints[moveIndex].randomLookLocationDuringRelaxTime)
            {
                nextLookingDirection = (MovePoint.relaxingLookingDirection)UnityEngine.Random.Range(0,4);

            }
            else
            {
                nextLookingDirection = movePoints[moveIndex].lookDirDuringRelaxTime;

            }
            transform.Translate(direction * Time.deltaTime * speed);
            anim.SetFloat("Vertical", direction2.y);
            anim.SetFloat("Horizontal", direction2.x);


        }

    }

    IEnumerator GetRelaxTime()
    {
        relaxTimeFinished = false;
        anim.SetBool("Move", false);
        anim.SetFloat("lookDir",(float)nextLookingDirection);
        yield return new WaitForSeconds(UnityEngine.Random.Range(minWaitTime, maxWaitTIme));


        relaxTimeFinished = true;
    }

    public void GetNewMovePoint()
    {

        int newIndex = UnityEngine.Random.Range(0, definitiveMoveLcoations.Count);
        do
        {
            newIndex = UnityEngine.Random.Range(0, definitiveMoveLcoations.Count);

        } while (newIndex == moveIndex);
        moveIndex = newIndex;

    }


    void UpdatePath()
    {

        float distance = Vector2.Distance(transform.position, definitiveMoveLcoations[moveIndex]);

        if (seeker.IsDone() && distance > 1.1f)
            seeker.StartPath(transform.position, definitiveMoveLcoations[moveIndex], OnPathComplete);

    }



    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    internal void LookAtPlayer(Collider2D collision)
    {
        if (anim != null)
        {


            Vector2 relativePoint = transform.InverseTransformPoint(collision.transform.position);
            if (relativePoint.x < 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y))
            {
                anim.SetFloat("lookDir", 2);
                return;
            }
            if (relativePoint.x > 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y))
            {
                anim.SetFloat("lookDir", 3);
                return;
            }
            if (relativePoint.y > 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.y))
            {
                anim.SetFloat("lookDir", 1);
                return;
            }
            if (relativePoint.y < 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.y))
            {
                //Debug.Log("Above");
                anim.SetFloat("lookDir", 0);
                return;
            }

        }
    }
}
