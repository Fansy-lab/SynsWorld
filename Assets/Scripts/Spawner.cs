using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemyToSpawn;
    public Transform[] spawnPositions;
    public LayerMask layerMask;
    public float spawnRadius;
    public float respawnCheck;

    public float maximumMovementRadiusOfSpawnedEnemies;
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
        Collider2D[] gOsFound= Physics2D.OverlapCircleAll(transform.position, spawnRadius);

        gOsFound = gOsFound.Where(val => val.isTrigger == false).ToArray(); //ignore the moveRangeCollider


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
            GameObject spawnPosition = GetWhereCanSpawn();

            enemyToSpawn.SetActive(false);
            GameObject enemy = Instantiate(enemyToSpawn, spawnPosition.transform.position, Quaternion.identity) as GameObject;
            IEnemy enemyScript = enemy.GetComponent<IEnemy>();
            enemy.GetComponent<EnemyAI>().SetMaximumMovement(maximumMovementRadiusOfSpawnedEnemies, spawnPosition);
            enemyScript.Name = enemy.name;
            enemy.SetActive(true);
            
        }
    }

    private GameObject GetWhereCanSpawn()
    {
        foreach (var spawnPoint in spawnPositions)
        {

             Collider2D[] gOsFound = Physics2D.OverlapCircleAll(spawnPoint.position, 0.5f, layerMask);

            gOsFound = gOsFound.Where(val => val.isTrigger == false).ToArray(); //ignore the moveRangeCollider


            if (gOsFound.Length == 0)
            {
                return spawnPoint.gameObject;
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
                        return spawnPoint.gameObject;
                    }
                }
                
            }
        }

        return spawnPositions[0].gameObject;
        
      
       
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        foreach (var item in spawnPositions)
        {
            Gizmos.DrawWireSphere(item.position, 0.75f);

        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maximumMovementRadiusOfSpawnedEnemies);

    }

}
