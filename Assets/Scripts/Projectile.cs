using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public int damageAmmount;
    public GameObject specialEffect;

    void Start()
    {
        Destroy(gameObject, 0.53f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(specialEffect, transform.position, Quaternion.identity);
        SoundEffectsManager.instance.PlayHitSound();
        Destroy(gameObject);
    }
}
