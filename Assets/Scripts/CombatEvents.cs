using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEvents : MonoBehaviour
{
    public static event Action<IEnemy> OnEnemyDeath;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void EnemyDied(IEnemy enemy)
    {
        OnEnemyDeath?.Invoke(enemy); // if onenemydeath is not null
    }
}
