using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Questing/New Kill Goal")]
public class KillGoal:ScriptableObject
{
    public int idKillGoal;
    public string Description;
    public bool Completed;
    public int CurrentAmmount;
    public int RequiredAmmount;

    public int EnemyID;
    public string EnemyName;

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
                #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
                #endif
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
