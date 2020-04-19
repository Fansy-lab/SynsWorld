using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 to;
    SceneManager sceneManager;
    
  
    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            
            StartCoroutine(StartAnimation(collision));
        }

    }
  
  
    IEnumerator StartAnimation(Collider2D collision)
    {
        sceneManager.CallEndAnimation(); //call the animation    
        yield return new WaitForSeconds(1);
        Camera.main.transform.position = collision.transform.position;
        

        collision.gameObject.transform.position = to; // move the object(player) to the position

    }

}
