using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Dialogue: MonoBehaviour
{
    public string NPCName;

    public List<string> sentences;
    public TextMeshProUGUI NpcNameText;
    public TextMeshProUGUI DisplayText;
    public GameObject accept;
    public GameObject decline;

    public GameObject next;
    public GameObject last;

    public int indexOfSentence;

    public Quest quest;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (next.gameObject.activeInHierarchy)
            {
                DisplayNextSentence();
            }

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (last.gameObject.activeInHierarchy)
            {
                DisplayLastSentence();
            }

        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (accept.gameObject.activeInHierarchy)
            {
                AcceptQuest();
            }

        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (decline.gameObject.activeInHierarchy)
            {
                DeclineQuest();
            }

        }
    }

    public void StartDialogue()
    {


        NpcNameText.text = NPCName;
        indexOfSentence = 0;
        last.SetActive(false);
        if (sentences.Count > 1)
            next.SetActive(true);
        else
            next.SetActive(false);
      
        DisplayFirstSentence();
    }

    private void DisplayFirstSentence()
    {
        string sentence = sentences[indexOfSentence];
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void DisplayLastSentence()
    {
        indexOfSentence--;
        if (indexOfSentence == 0)
        {
            last.SetActive(false);
        }
        HidePopUpYesNo();
        next.SetActive(true);
        string sentence = sentences[indexOfSentence];
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }
    public void DisplayNextSentence()
    {
    
        indexOfSentence++;
        last.SetActive(true);
        string sentence = sentences[indexOfSentence];
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        if (indexOfSentence+1 == sentences.Count) //end of lines
        {
            next.SetActive(false);
            if(quest!=null)
            ShowPopUpYesNo();
        }
      

    }

    private void ShowPopUpYesNo()
    {

        if( !UIManager.Instance.questsService.currentQuests.Any(x=>x.QuestID==quest.QuestID))
          
        {
            if (!UIManager.Instance.questsService.completedQuests.Any(x => x.QuestID == quest.QuestID))
            {
                accept.SetActive(true);
                decline.SetActive(true);

            }
            
        }

      

    }
    private void HidePopUpYesNo()
    {
        accept.SetActive(false);
        decline.SetActive(false);

    }

    IEnumerator TypeSentence(string sentence)
    {
        DisplayText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            DisplayText.text += letter;
            yield return null;
        }
    }


    public void AcceptQuest()
    {
        if (quest)
        {
            UIManager.Instance.questsService.StartQuest(quest);
        }
        Destroy(gameObject);
    }
    public void DeclineQuest()
    {
        Destroy(gameObject);
    }
}
