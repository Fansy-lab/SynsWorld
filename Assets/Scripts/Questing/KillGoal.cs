using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class KillGoal: MonoBehaviour
{
    public KillGoalData killGoalData;

    public void Init()
    {
        GlobalEvents.OnEnemyDeath += EnemyDied;

    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    public void UnsubscribeFromEvents()
    {
        GlobalEvents.OnEnemyDeath -= EnemyDied;

    }

    void EnemyDied(IEnemy enemy)
    {
        if (!killGoalData.Completed)
        {
            if (enemy.ID == killGoalData.EnemyID)
            {
                killGoalData.CurrentAmmount++;

                Evaluate();
            }
        }

    }
    public void Evaluate()
    {
        if (killGoalData.CurrentAmmount >= killGoalData.RequiredAmmount)
        {
            Complete();

        }
    }
    public void Complete()
    {
        killGoalData.Completed = true;
        GlobalEvents.KillGoalCompleted(this);

    }

}
