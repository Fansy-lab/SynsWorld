using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ReachGoal : MonoBehaviour
{

    public ReachGoalData reachGoalData;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            reachGoalData.Completed = true;
            GlobalEvents.ReachedGoal(this);
            Destroy(gameObject);
        }
    }


}
