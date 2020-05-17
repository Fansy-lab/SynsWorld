using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterLeave : MonoBehaviour
{

    public string Location;
    public bool enter;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != false)
        {
            if (playerStats.insideALocation)
            {
                EnterLeaveLocationManager.instance.ShowWhereYouLeave(Location);
                playerStats.insideALocation = false;

            }
            else
            {
                EnterLeaveLocationManager.instance.ShowWhereYouEnter(Location);
                playerStats.insideALocation = true;

            }
        }
          

    }
}
