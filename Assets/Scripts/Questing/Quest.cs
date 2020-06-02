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
    public List<ReachGoal> ReachGoals = new List<ReachGoal>();

    public string QuestName;
    public Sprite spriteQuestPopUp;
    [TextArea(5, 8)]
    public string QuestDescription;
    public int ExpReward;
    public int GoldReward;
    public string ItemReward;
    public bool IsCompleted;
    [TextArea(5, 8)]
    public List<string> StartQuestDialogue;
    [TextArea(5, 8)]
    public List<string> WhileOnQuestDialogue;
    [TextArea(5, 8)]
    public List<string> FinishedQuestDialogue;




    public void Init(int questId, List<KillGoal> killGoals, List<PickGoal> pickGoals,List<ReachGoal> reachGoals, string questName,string questDescription,int expReward,int goldReward,string itemReward,bool isCompleted)
    {
        QuestID = questId;
        KillGoals = killGoals;
        PickGoals = pickGoals;
        QuestName =questName;
        QuestDescription =questDescription;
        ReachGoals = reachGoals;
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

        if (ReachGoals != null && ReachGoals.Count > 0)
            GlobalEvents.OnReachedGoal -= ReachedGoalCompleated;

    }

    private void SubscribeEvents()
    {
        GlobalEvents.OnKillGoalCompleted += KillGoalCompleted;
        GlobalEvents.OnPickedGoalCompleted += PickGoalCompleted;
        GlobalEvents.OnReachedGoal += ReachedGoalCompleated;
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
    private void CheckReachGoals(ReachGoal goalCompleated)
    {
        bool isFromThisQuest = false;
        foreach (var goal in ReachGoals)
        {
            if (goal.ID == goalCompleated.ID)
            {
                isFromThisQuest = true;
                goal.Completed = true;
                break;
            }
        }
        if (isFromThisQuest)
        {
            foreach (var goal in ReachGoals)
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

    private void ReachedGoalCompleated(ReachGoal goal)
    {
        CheckReachGoals(goal);

    }



    private void CompleteQuest(Quest quest)
    {
        quest.IsCompleted = true;
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
        GiveReward(quest);
        UIManager.Instance.questsService.CompletedQuest(quest);

    }



    private void GiveReward(Quest quest)
    {
        LevelSystem.AddExp(quest.ExpReward);
        PlayerStats playerStats = GameObject.FindObjectOfType<PlayerStats>();
        playerStats.gold += quest.GoldReward;
        EmoteManager.Instance.ShowCompletedQuestEmote();
    }
}
