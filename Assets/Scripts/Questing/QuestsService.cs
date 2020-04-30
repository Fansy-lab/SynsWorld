using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestsService : MonoBehaviour
{
    public List<Quest> completedQuests;
    public List<Quest> currentQuests;

    public void AddNewQuest(Quest quest)
    {

        DialogueInstance.Instance.GetComponent<DialogueManager>().EndDialogue();
        InteractPoint.currentInteractableObjectScript.interactableMultipleTimes = false;

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

    internal void AbandonQuest(Quest quest)
    {
        Destroy(quest);
    }
}
