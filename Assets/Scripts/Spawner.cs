using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemyToSpawn;
    public Transform[] spawnPositions;
    public LayerMask layerMask;
    public float radius;
    public float respawnCheck;
    void Start()
    {
        InvokeRepeating("CheckIfHasToSpawn",0,respawnCheck);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CheckIfHasToSpawn()
    {
       Collider2D[] gOsFound= Physics2D.OverlapCircleAll(transform.position, radius);
        int totalToSpawn = spawnPositions.Length;

        if (gOsFound.Length == 0)
        {
            SpawnEnemies(spawnPositions.Length);
        }
        else
        {
            
            foreach (var go in gOsFound)
            {
                if (go.transform.tag == "Enemy")
                {
                    totalToSpawn--;
                }
            }
            SpawnEnemies(totalToSpawn);
            
        }

    }

    private void SpawnEnemies(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
           // int random = UnityEngine.Random.Range(0, spawnPositions.Length);
            Vector3 spawnPosition = GetWhereCanSpawn();
            
            GameObject enemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity) as GameObject;
            IEnemy enemyScript = enemy.GetComponent<IEnemy>();
            enemyScript.Name = enemy.name;
            
        }
    }

    private Vector3 GetWhereCanSpawn()
    {
        foreach (var spawnPoint in spawnPositions)
        {

             Collider2D[] gOsFound = Physics2D.OverlapCircleAll(spawnPoint.position, 0.5f, layerMask);

         
            if (gOsFound.Length == 0)
            {
                return spawnPoint.position;
            }
            else
            {
                foreach (var gO in gOsFound)
                {
                    if (gO.transform.tag == "Enemy")
                    {
                        continue;
                    }
                    else
                    {
                        return spawnPoint.position;
                    }
                }
                
            }
        }

        return spawnPositions[0].position;
        
      
       
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);

        foreach (var item in spawnPositions)
        {
            Gizmos.DrawWireSphere(item.position, 0.75f);

        }
    }

}
