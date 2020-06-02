using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoal : MonoBehaviour
{

    public int ID;
    public bool Completed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Completed = true;
            GlobalEvents.ReachedGoal(this);
            Destroy(gameObject);
        }
    }
}
