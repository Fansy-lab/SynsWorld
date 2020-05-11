using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 to;
    
    
  
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {


            Camera.main.transform.position = collision.transform.position;


            collision.gameObject.transform.position = to; // move the object(player) to the position        }

        }

    }
}
