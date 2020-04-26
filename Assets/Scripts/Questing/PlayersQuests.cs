using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersQuests : MonoBehaviour
{
    public List<Quest> completedQuests;


    public void AddNewQuest(Quest quest)
    {
        
        DialogueInstance.Instance.GetComponent<DialogueManager>().EndDialogue();
        InteractPoint.currentInteractableObjectScript.interactableMultipleTimes = false;
        EmoteManager.Instance.ShowNewQuestEmote();
        GameObject newQuest = Instantiate(quest.gameObject);
        newQuest.transform.parent = gameObject.transform;
        UIManager.Instance.AddNewQuestToTheUIList(quest);

        
    }
    public void AddQuestToCompletedQuestsAndRemoveQuestFromUI(Quest quest)
    {
        completedQuests.Add(quest);
        UIManager.Instance.RemoveQuestScrollFromUI(quest);

    }

    internal void AbandonQuest(Quest quest)
    {
        foreach (Transform item in gameObject.transform)
        {
            Quest questFound = item.GetComponent<Quest>();
            if (questFound != null)
            {
                if (quest.QuestID == questFound.QuestID)
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }
}
