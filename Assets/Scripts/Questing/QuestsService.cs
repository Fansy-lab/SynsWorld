using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsService : MonoBehaviour
{


    public List<Quest> AllQuests;

    public List<Quest> completedQuests;
    public List<Quest> currentQuests;

    public List<Quest> initialQuests;

    private PlayerStats playerStats;

    private void Start()
    {

    }



    public void AddNewQuestToUIAndAlertListeners(Quest quest,bool playPopUpAnimation)
    {

        if(playPopUpAnimation)
            EmoteManager.Instance.ShowNewQuestEmote();

        UIManager.Instance.AddNewQuestToTheUIList(quest);

        StartQuestListeners(quest);

    }

    private void StartQuestListeners(Quest quest)
    {
        currentQuests.Add(quest);

        quest.Init();

        foreach (var goal in quest.KillGoals)
        {
            goal.Init();
        }
        foreach (var goal in quest.PickGoals)
        {
            goal.Init();
        }

    }

    public void EnableShooting()
    {
        playerStats = GameObject.Find("Player").GetComponentInChildren<PlayerStats>();

        playerStats.EnableShooting();
    }



    public void DisableShooting()
    {
        playerStats = GameObject.Find("Player").GetComponentInChildren<PlayerStats>();

        playerStats.DisableShooting();

    }

    public void CompletedQuest(Quest quest)
    {

        AddQuestToCompletedQuestsAndRemoveQuestFromUI(quest);
        RemoveFromCurrentQuests(quest);
        SoundEffectsManager.instance.PlayDoneQuestSound();

    }
    public void AbandonQuest(Quest quest)
    {

        RemoveFromCurrentQuests(quest);
        Quest[] questInstances = GameObject.FindObjectsOfType<Quest>();
        foreach (var questFound in questInstances)
        {
            if (questFound.QuestID == quest.QuestID)
                DestroyQuest(questFound);

        }

    }

    public void DeclineQuest()
    {

    }

    public void StartQuest(Quest questToStart,bool playPopUpAnimation)
    {
        if (questToStart != null)
        {

            SoundEffectsManager.instance.PlayNewQuestSound();

            AddNewQuestToUIAndAlertListeners(questToStart,playPopUpAnimation);

        }
    }


    public void AddQuestToCompletedQuestsAndRemoveQuestFromUI(Quest quest)
    {
        completedQuests.Add(quest);
        UIManager.Instance.RemoveQuestScrollFromUI(quest);

    }
    public void RemoveFromCurrentQuests(Quest quest)
    {
        currentQuests.Remove(quest);

    }

    private void DestroyQuest(Quest quest)
    {
        Destroy(quest);
    }

    internal void LoadQuestLists(List<KillGoalData> killGoalData, List<ReachGoalData> reachGoalData, List<PickGoalData> pickGoalData, List<int> currentQuestIDs, List<int> doneQuestIDs)
    {
        foreach (var quest in AllQuests)
        {
            #region resetKillGoals
            foreach (var killGoal in quest.KillGoals)
            {
                killGoal.killGoalData.Completed = false;
                killGoal.killGoalData.CurrentAmmount = 0;
            }
            foreach (var reachGoal in quest.ReachGoals)
            {
                reachGoal.reachGoalData.Completed = false;
            }
            foreach (var pickGoal in quest.PickGoals)
            {
                pickGoal.pickGoalData.Completed = false;
                pickGoal.pickGoalData.CurrentAmmount = 0;

            }
            #endregion
            foreach (var playerCurrentQuest in currentQuestIDs)
            {
                if(quest.QuestID == playerCurrentQuest)
                {
                    foreach (var killGoal in killGoalData)
                    {
                        foreach (var questKillGoal in quest.KillGoals)
                        {
                            if (killGoal.idKillGoal == questKillGoal.killGoalData.idKillGoal)
                            {
                                questKillGoal.killGoalData = killGoal;
                            }
                        }
                    }
                    foreach (var reachGoal in reachGoalData)
                    {
                        foreach (var questreachGoal in quest.ReachGoals)
                        {
                            if (reachGoal.ID == questreachGoal.reachGoalData.ID)
                            {
                                questreachGoal.reachGoalData = reachGoal;
                            }
                        }
                    }
                    foreach (var pickGoal in pickGoalData)
                    {
                        foreach (var questPickGoal in quest.PickGoals)
                        {
                            if(pickGoal.idPickGoal == questPickGoal.pickGoalData.idPickGoal)
                            {
                                questPickGoal.pickGoalData = pickGoal;
                            }
                        }
                    }

                    StartQuest(quest,false);

                }

            }
        }
        foreach (var quest in AllQuests)
        {
            foreach (var playerDoneQuest in doneQuestIDs)
            {
                if (quest.QuestID == playerDoneQuest)
                {
                    completedQuests.Add(quest);
                }
            }
        }
    }
}
