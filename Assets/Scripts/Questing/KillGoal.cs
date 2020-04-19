using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal
{
    public int EnemyID { get; set; }

    public KillGoal(Quest quest,int enemyID,string description,bool completed,int currentAmmount,int requieredAmmount)
    {
        this.quest = quest;
        this.EnemyID = enemyID;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmmount = currentAmmount;
        this.RequiredAmmount = requieredAmmount;
    }
    public override void Init()
    {
        base.Init();
        CombatEvents.OnEnemyDeath += EnemyDied;
    }

    void EnemyDied(IEnemy enemy)
    {
        if(enemy.ID == this.EnemyID)
        {
            this.CurrentAmmount++;
            Evaluate(); //from goal
        }
    }
}
