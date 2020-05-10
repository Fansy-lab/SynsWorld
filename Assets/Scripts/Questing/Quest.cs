using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="Quest",menuName = "Questing/New Quest")]
public class Quest : ScriptableObject
{
    public int QuestID;
    public List<KillGoal> KillGoals = new List<KillGoal>();
    public string QuestName;
    public Sprite spriteQuestPopUp;
    public string QuestDescription;
    public int ExpReward;
    public int GoldReward;
    public string ItemReward;
    public bool IsCompleted;

    
    public void Init(int questId, List<KillGoal> killGoals,string questName,string questDescription,int expReward,int goldReward,string itemReward,bool isCompleted)
    {
        QuestID = questId;
        KillGoals = killGoals;
        QuestName =questName;
        QuestDescription =questDescription;
        ExpReward =expReward;
        GoldReward =goldReward;
        ItemReward =itemReward;
        IsCompleted =isCompleted;
        SubscribeEvents();

    }
    private void OnDestroy()
    {
       GlobalEvents.OnKillGoalCompleted -= KillGoalCompleted;

    }

    private void SubscribeEvents()
    {
        GlobalEvents.OnKillGoalCompleted += KillGoalCompleted;
    }

    public void CheckGoals(KillGoal completedKillGoal)
    {
        bool isFromThisQuest = false;
        foreach (var goal in KillGoals)
        {
            if (goal.idKillGoal == completedKillGoal.idKillGoal)
            {
                isFromThisQuest = true;
                break;
            }
        }

        if (isFromThisQuest)
        {
            foreach (var goal in KillGoals)
            {
                if (goal.Completed == false)
                {

                    return;
                }
            }
            CompleteQuest(this);
        }
        
    }
    private void KillGoalCompleted(KillGoal goal)
    {
        CheckGoals(goal);
    }

    private void CompleteQuest(Quest quest)
    {
        quest.IsCompleted = true;
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
        GiveReward();
        UIManager.Instance.questsService.CompletedQuest(quest);
    }

 

    private void GiveReward()
    {
       
        EmoteManager.Instance.ShowCompletedQuestEmote();
    }
} 
