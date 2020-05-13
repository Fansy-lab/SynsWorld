using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum buttonType
{
    introduction,shop,quest
}

[Serializable]
public class DisplayOption
{
    public string buttonText;
    public buttonType buttonType;
    public Quest quest;
   
}

public class Dialogue: MonoBehaviour
{
    public string NPCName;

    public List<string> sentences;
    public TextMeshProUGUI NpcNameText;
    public TextMeshProUGUI DisplayText;
    public GameObject accept;
    public GameObject decline;

    public Canvas canvas;

    public GameObject next;
    public GameObject last;

    public List<DisplayOption> options;

    public GameObject UIdisplayOptions;
    public GameObject uiOption;

    public int indexOfSentence;

    public Quest Quest;


    Vector2 initialSizeOfCanvas;
    Vector2 initialSizeOfBackGround;




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

        initialSizeOfCanvas = canvas.GetComponent<RectTransform>().sizeDelta;
        initialSizeOfBackGround = gameObject.GetComponent<SpriteRenderer>().size;

        NpcNameText.text = NPCName;
        indexOfSentence = 0;
        last.SetActive(false);
        if (sentences.Count > 1)
            next.SetActive(true);
        else
            next.SetActive(false);

        if (options.Count > 0)//display clickable options
        {
            UIdisplayOptions.SetActive(true);
            int counter = 0;
           
            foreach (var option in options)
            {
                 counter++;
                GameObject ui  =Instantiate(uiOption, transform.position,Quaternion.identity) as GameObject;
                ui.GetComponentInChildren<TextMeshProUGUI>().text =counter+". "+option.buttonText;
                ui.transform.SetParent(UIdisplayOptions.transform);
                if (option.quest != null)
                {
                      ui.GetComponent<Button>().onClick.AddListener(() => DisplayQuest(option.quest));

                }
                if (counter > 2)
                {
                    Vector2 actualPositionOfCanvas = canvas.GetComponent<RectTransform>().sizeDelta;
                    canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(actualPositionOfCanvas.x, actualPositionOfCanvas.y + 0.7f);
                   
                    Vector2 actualPositionOfBackGround= gameObject.GetComponent<SpriteRenderer>().size;
                    gameObject.GetComponent<SpriteRenderer>().size = new Vector2(actualPositionOfBackGround.x, actualPositionOfBackGround.y+0.4f);
                }
            }
        }
        else
        {
            DisplayFirstSentence();

        }
    }

    private void DisplayQuest(Quest quest)
    {
        UIdisplayOptions.SetActive(false);
        sentences = quest.StartQuestDialogue;
        Quest = quest;
        next.SetActive(true);
        DisplayFirstSentence();

    }

    private void DisplayFirstSentence()
    {
        gameObject.GetComponent<SpriteRenderer>().size = new Vector2(initialSizeOfBackGround.x, initialSizeOfBackGround.y);
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(initialSizeOfCanvas.x, initialSizeOfCanvas.y);


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
            if(Quest!=null)
            ShowPopUpYesNo();
        }
      

    }

    private void ShowPopUpYesNo()
    {

        if( !UIManager.Instance.questsService.currentQuests.Any(x=>x.QuestID==Quest.QuestID))
          
        {
            if (!UIManager.Instance.questsService.completedQuests.Any(x => x.QuestID == Quest.QuestID))
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
        if (Quest)
        {
          

            UIManager.Instance.questsService.StartQuest(Quest);
        }
        Destroy(gameObject);
    }
    public void DeclineQuest()
    {
        Destroy(gameObject);
    }
}
