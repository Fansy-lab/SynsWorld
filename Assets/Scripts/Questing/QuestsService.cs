using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsService : MonoBehaviour
{

    public List<Quest> completedQuests;
    public List<Quest> currentQuests;


    private PlayerStats playerStats;




    public void AddNewQuest(Quest quest)
    {


        EmoteManager.Instance.ShowNewQuestEmote();
        UIManager.Instance.AddNewQuestToTheUIList(quest);

        StartQuestListeners(quest);

    }

    private void StartQuestListeners(Quest quest)
    {
        currentQuests.Add(quest);
      
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

    public void StartQuest(Quest questToStart)
    {
        if (questToStart != null)
        {
            Quest newQuest = ScriptableObject.CreateInstance("Quest") as Quest;
            newQuest.Init(questToStart.QuestID, questToStart.KillGoals,questToStart.PickGoals, questToStart.QuestName, questToStart.QuestDescription, questToStart.ExpReward, questToStart.GoldReward, questToStart.ItemReward, questToStart.IsCompleted);

            SoundEffectsManager.instance.PlayNewQuestSound();

            AddNewQuest(newQuest);

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
}
