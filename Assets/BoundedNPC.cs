using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedNPC : MonoBehaviour
{
    // Start is called before the first frame update


    Animator anim;
    bool playerInRange=false;
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
   
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerInput>() != false)
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerInput>() != false)
        {
            playerInRange = false;
        }
    }

}
