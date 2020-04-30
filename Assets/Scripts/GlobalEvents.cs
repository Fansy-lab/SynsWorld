using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvents : MonoBehaviour
{
    public static event Action<IEnemy> OnEnemyDeath;
    public static event Action<KillGoal> OnKillGoalCompleted;

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

    public static void KillGoalCompleted(KillGoal killGoal)
    {
        OnKillGoalCompleted?.Invoke(killGoal); 
    }
}
