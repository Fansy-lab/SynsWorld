using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Quest : MonoBehaviour
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




    public void Init()
    {
        //load data for this quest,if no info, its a new quest. set to null all goals and  quest stats, else laod info

        //IsCompleted = false;
        //KillGoals = new List<KillGoal>();
        //PickGoals = new List<PickGoal>();
        //ReachGoals = new List<ReachGoal>();

        SubscribeEvents();

    }
    private void OnDestroy()
    {

        UnsubscribeFromEvents();

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
            if (goal.killGoalData.idKillGoal == completedKillGoal.killGoalData.idKillGoal)
            {
                isFromThisQuest = true;
                break;
            }
        }

        if (isFromThisQuest)
        {
            foreach (var goal in KillGoals)
            {
                if (goal.killGoalData.Completed == false)
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
            if (goal.reachGoalData.ID == goalCompleated.reachGoalData.ID)
            {
                isFromThisQuest = true;
                goal.reachGoalData.Completed = true;
                break;
            }
        }
        if (isFromThisQuest)
        {
            foreach (var goal in ReachGoals)
            {
                if (goal.reachGoalData.Completed == false)
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
            if (goal.pickGoalData.idPickGoal == goalComplted.pickGoalData.idPickGoal)
            {
                isFromThisQuest = true;
                break;
            }
        }

        if (isFromThisQuest)
        {
            foreach (var goal in PickGoals)
            {
                if (goal.pickGoalData.Completed == false)
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

        GiveReward(quest);
        UIManager.Instance.questsService.CompletedQuest(quest);
        GlobalEvents.QuestCompleted(quest);
        UnsubscribeFromEvents();

    }

    public void UnsubscribeFromEvents()
    {
        GlobalEvents.OnKillGoalCompleted -= KillGoalCompleted;


        GlobalEvents.OnPickedGoalCompleted -= PickGoalCompleted;


        GlobalEvents.OnReachedGoal -= ReachedGoalCompleated;
    }

    private void GiveReward(Quest quest)
    {
        LevelSystem.AddExp(quest.ExpReward);
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerStats.gold += quest.GoldReward;
        EmoteManager.Instance.ShowCompletedQuestEmote();
    }
}
