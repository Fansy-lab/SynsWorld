using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvents : MonoBehaviour
{
    public static event Action<IEnemy> OnEnemyDeath;
    public static event Action<KillGoal> OnKillGoalCompleted;
    public static event Action<PickGoal> OnPickedGoalCompleted;
    public static event Action<Quest> OnAcceptedQuest;
    public static event Action<Quest> OnQuestCompleted;
    public static event Action<InventoryItem> OnPickedItem;
    public static event Action<ReachGoal> OnReachedGoal;
    public static event Action<int> OnGainedExperience;
    public static event Action<int?> OnLevelUp;

    public static void PickedItem(InventoryItem inventoryItem)
    {
        OnPickedItem?.Invoke(inventoryItem);
    }
    public static void EnemyDied(IEnemy enemy)
    {
        OnEnemyDeath?.Invoke(enemy); // if onenemydeath is not null
    }
    public static void PickedGoalCompleted(PickGoal pickGoal)
    {
        OnPickedGoalCompleted?.Invoke(pickGoal);
    }
    public static void KillGoalCompleted(KillGoal killGoal)
    {
        OnKillGoalCompleted?.Invoke(killGoal);
    }
    public static void ReachedGoal(ReachGoal reachGoal)
    {
        OnReachedGoal?.Invoke(reachGoal);
    }
    public static void LeveledUp(int expForNextLevel)
    {
        OnLevelUp?.Invoke(expForNextLevel);
    }
    public static void GainedExperience(int experienceGained)
    {
        OnGainedExperience?.Invoke(experienceGained);
    }
    public static void AcceptedQuest(Quest questAccepted)
    {
        OnAcceptedQuest(questAccepted);
    }
    public static void QuestCompleted(Quest questCompleted)
    {
        OnQuestCompleted(questCompleted);
    }

}
