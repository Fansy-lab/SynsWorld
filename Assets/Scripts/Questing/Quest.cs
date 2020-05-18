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
    public List<PickGoal> PickGoals = new List<PickGoal>();

    public string QuestName;
    public Sprite spriteQuestPopUp;
    public string QuestDescription;
    public int ExpReward;
    public int GoldReward;
    public string ItemReward;
    public bool IsCompleted;

    public List<string> StartQuestDialogue;
    public List<string> WhileOnQuestDialogue;
    public List<string> FinishedQuestDialogue;




    public void Init(int questId, List<KillGoal> killGoals, List<PickGoal> pickGoals, string questName,string questDescription,int expReward,int goldReward,string itemReward,bool isCompleted)
    {
        QuestID = questId;
        KillGoals = killGoals;
        PickGoals = pickGoals;
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
        if(KillGoals!=null && KillGoals.Count>0)
            GlobalEvents.OnKillGoalCompleted -= KillGoalCompleted;

        if (PickGoals != null && PickGoals.Count > 0)
            GlobalEvents.OnPickedGoalCompleted -= PickGoalCompleted;

    }

    private void SubscribeEvents()
    {
        GlobalEvents.OnKillGoalCompleted += KillGoalCompleted;
        GlobalEvents.OnPickedGoalCompleted += PickGoalCompleted;
    }

    public void CheckKillGoals(KillGoal completedKillGoal)
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
    private void CheckPickGoals(PickGoal goalComplted)
    {
        bool isFromThisQuest = false;
        foreach (var goal in PickGoals)
        {
            if (goal.idPickGoal == goalComplted.idPickGoal)
            {
                isFromThisQuest = true;
                break;
            }
        }

        if (isFromThisQuest)
        {
            foreach (var goal in PickGoals)
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
        CheckKillGoals(goal);
    }
    private void PickGoalCompleted(PickGoal goal)
    {
        CheckPickGoals(goal);
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
