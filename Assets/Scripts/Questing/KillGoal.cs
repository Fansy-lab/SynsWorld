using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Questing/New Goal")]
public class KillGoal:ScriptableObject
{
    public int idKillGoal;
    public string Description;
    public bool Completed;
    public int CurrentAmmount;
    public int RequiredAmmount;

    public int EnemyID;

    public void Init()
    {
        GlobalEvents.OnEnemyDeath += EnemyDied;

    }

    private void OnDestroy()
    {
      GlobalEvents.OnEnemyDeath -= EnemyDied;

    }


    void EnemyDied(IEnemy enemy)
    {
        if (!Completed)
        {
            if (enemy.ID == this.EnemyID)
            {
                this.CurrentAmmount++;
                Evaluate(); 
            }
        }

    }
    public void Evaluate()
    {
        if (CurrentAmmount >= RequiredAmmount)
        {
            Complete();

        }
    }
    public void Complete()
    {
        Completed = true;
        GlobalEvents.KillGoalCompleted(this);
        
    }

}
