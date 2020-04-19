using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public List<Goal> goals { get; set; } = new List<Goal>();
    public string QuestName { get; set; }
    public string QuestDescription { get; set; }
    public int ExpReward { get; set; }
    public int GoldReward { get; set; }
    public string ItemReward { get; set; }
    public bool IsCompleted { get; set; }

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
            CompleteQuest();
        }
   
        
    }

    private void CompleteQuest()
    {
        IsCompleted = true;
        GiveReward();
    }

    private void GiveReward()
    {
        print("QUEST:Completed");
    }
} 
