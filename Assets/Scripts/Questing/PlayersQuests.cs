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
    public void CompleteQuest(Quest quest)
    {
        completedQuests.Add(quest);
        UIManager.Instance.RemoveQuestScrollFromUI(quest);

    }
}
