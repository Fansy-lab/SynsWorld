using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public int QuestID;
    public List<Goal> goals = new List<Goal>();
    public string QuestName;
    public string QuestDescription;
    public int ExpReward;
    public int GoldReward;
    public string ItemReward;
    public bool IsCompleted;

    public void CheckGoals()
    {
        if (goals != null && goals.Count>0)
        {
            foreach (var goal in goals)
            {
                if (goal.Completed == false)
                {
                   
                    return;
                }
            }
            CompleteQuest(this);
        }
   
        
    }

    private void CompleteQuest(Quest quest)
    {
        IsCompleted = true;
        GiveReward();
        GM.Instance.CompletedQuest(quest);
        Destroy(gameObject);
    }

 

    private void GiveReward()
    {
        print("QUEST:Completed");
        EmoteManager.Instance.ShowCompletedQuestEmote();
    }
} 
