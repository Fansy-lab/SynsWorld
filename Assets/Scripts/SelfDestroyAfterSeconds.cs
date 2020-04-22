using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyAfterSeconds : MonoBehaviour
{
    // Start is called before the first frame update
    public float seconds;
    void Start()
    {
        Destroy(gameObject, seconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
