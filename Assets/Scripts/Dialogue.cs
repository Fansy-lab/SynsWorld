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
    monologue, shop, quest, PrivateChest
}

[Serializable]
public class DisplayOption
{
    public string buttonText;
 //   public buttonType buttonType;
    public List<string> monologue;
    public Quest quest;


    public string methodToCallInGm;
    public List<string> parameters;

}

public class Dialogue : MonoBehaviour
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

    private string methodToCallInGm;
    private List<string> parameters;

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
                DisplayPreviousSentence();
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


        if (options.Count > 0)//display clickable options
        {
            DisplayOptions();
        }
        else //no options, its direct message to the player
        {
            DisplayFirstSentence(null, null);

        }
    }

    private void DisplayOptions()
    {
        next.SetActive(false);
        last.SetActive(false);



        UIdisplayOptions.SetActive(true);
        int counter = 0;

        foreach (var option in options)
        {
            counter++;
            GameObject ui = Instantiate(uiOption, transform.position, Quaternion.identity) as GameObject;
            foreach (var item in ui.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (item.transform.name == "KeyNumber")
                {
                    item.text = "[" + counter + "] ";
                }
                if (item.transform.name == "OptionText")
                {
                    item.text = option.buttonText;

                }
            }

            ui.transform.SetParent(UIdisplayOptions.transform);


            ui.GetComponent<KeyButton>().code = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + counter);

            if (option.quest != null) //start quest dialogue
            {
                ui.GetComponent<Button>().onClick.AddListener(() => DisplayQuest(option.quest));


            }
            else if (option.monologue.Count > 0 && !String.IsNullOrEmpty(option.methodToCallInGm))  //has dialogue and starts a method in GM at the end
            {
                sentences = option.monologue;

                methodToCallInGm = option.methodToCallInGm;
                parameters = option.parameters;

                ui.GetComponent<Button>().onClick.AddListener(() => DisplayFirstSentence(option.monologue, option));

            }
            else if (!String.IsNullOrEmpty(option.methodToCallInGm) && option.monologue.Count == 0)//no text/dialogue, just start a method in GM
            {
                ui.GetComponent<Button>().onClick.AddListener(() => StartMethod(option.methodToCallInGm, option.parameters));

            }
            else if(String.IsNullOrEmpty(option.methodToCallInGm) && option.monologue.Count > 0)//simple text
            {
                sentences = option.monologue;

                ui.GetComponent<Button>().onClick.AddListener(() => DisplayFirstSentence(option.monologue, option));

            }

            if (counter > 2)
            {
                MakeBiggerTheBackground();

            }
        }
    }

    private void StartMethod(string methodToCallInGm, List<string> parameters)
    {
        GM.Instance.CallMethod(methodToCallInGm, parameters);
    }

    private void DisplayQuest(Quest quest)
    {
        indexOfSentence = 0;
        UIdisplayOptions.SetActive(false);
        bool youAreOnThisQuest = UIManager.Instance.questsService.currentQuests.Any(x => x.QuestID == quest.QuestID);
        bool youHaveFinishedThisQuest = UIManager.Instance.questsService.completedQuests.Any(x => x.QuestID == quest.QuestID);

        if (youAreOnThisQuest)
        {
            sentences = quest.WhileOnQuestDialogue;
        }
        else if (youHaveFinishedThisQuest)
        {
            sentences = quest.FinishedQuestDialogue;
        }
        else
        {
            sentences = quest.StartQuestDialogue;
            Quest = quest;

        }

        next.SetActive(true);
        DisplayFirstSentence(sentences, null);

    }

    private void DisplayFirstSentence(List<string> sentencesToDisplay, DisplayOption option)
    {
        UIdisplayOptions.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().size = new Vector2(initialSizeOfBackGround.x, initialSizeOfBackGround.y);
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(initialSizeOfCanvas.x, initialSizeOfCanvas.y);

        indexOfSentence = 0;

        string sentence = "";
        if (sentencesToDisplay == null)// no display options, direct message is displayed
        {
            sentence = sentences[indexOfSentence];
        }

        else if (sentencesToDisplay.Count > 0)
        {

            sentences = sentencesToDisplay;
            sentence = sentences[indexOfSentence];

        }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        if (options.Count > 0)
        {
            last.SetActive(true);
        }
        if (sentences.Count > 1)
        {
            next.SetActive(true);
        }
        else
        {
            next.SetActive(false);
        }


        //to delete

    }

    public void DisplayPreviousSentence()
    {
        indexOfSentence--;
        if (indexOfSentence >= 0)
        {
            if (options.Count == 0)
            {
                last.SetActive(false);
            }
            HidePopUpYesNo();
            next.SetActive(true);
            string sentence = sentences[indexOfSentence];
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
        else if (indexOfSentence == -1) //back to main menu of the dialogue
        {
            StopAllCoroutines();

            if (options.Count > 0)
            {
                int counter = 0;
                foreach (var item in options)
                {
                    counter++;
                    if (counter > 2)
                    {
                        MakeBiggerTheBackground();

                    }

                }


                next.SetActive(false);
                last.SetActive(false);
                DisplayText.text = "";
                Quest = null;
                UIdisplayOptions.SetActive(true);


            }
        }



    }

    private void MakeBiggerTheBackground()
    {
        Vector2 actualPositionOfCanvas = canvas.GetComponent<RectTransform>().sizeDelta;
        canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(actualPositionOfCanvas.x, actualPositionOfCanvas.y + 0.7f);

        Vector2 actualPositionOfBackGround = gameObject.GetComponent<SpriteRenderer>().size;
        gameObject.GetComponent<SpriteRenderer>().size = new Vector2(actualPositionOfBackGround.x, actualPositionOfBackGround.y + 0.4f);
    }

    public void DisplayNextSentence()
    {

        indexOfSentence++;
        last.SetActive(true);
        string sentence = sentences[indexOfSentence];
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        if (indexOfSentence + 1 == sentences.Count) //end of lines
        {
            next.SetActive(false);

            if (Quest != null)
            {
                ShowPopUpYesNo();

            }
            if (!String.IsNullOrEmpty(methodToCallInGm))
            {
                GM.Instance.CallMethod(methodToCallInGm, parameters);
            }

        }


    }

    private void ShowPopUpYesNo()
    {

        if (!UIManager.Instance.questsService.currentQuests.Any(x => x.QuestID == Quest.QuestID))

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
        //  float waitFrames = Time.deltaTime * 1.3f;
        DisplayText.text = "";
        bool a = true;
        foreach (char letter in sentence.ToCharArray())
        {
            DisplayText.text += letter;
            if (a)
            {
                SoundEffectsManager.instance.PlayLetter();


            }
            a = !a;
            yield return new WaitForSeconds(0.02f);
        }

    }

    public void PlaySoundWhenFinishedAppearing()
    {
        SoundEffectsManager.instance.PlaySpeechBubblePop();
    }

    public void AcceptQuest()
    {
        if (Quest)
        {


            UIManager.Instance.questsService.StartQuest(Quest,true);
        }
        Destroy(gameObject);
    }
    public void DeclineQuest()
    {
        Destroy(gameObject);
    }
}
