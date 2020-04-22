using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator animator;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public bool currentlyADialogIsOn = false;
    public Guid? currentGuid = null;
    public GameObject YesNo;

    Queue<string> sentences;
    void Start()
    {
        sentences = new Queue<string>();
    }


    public void StartDialogue(Dialogue dialogue)
    {
        currentlyADialogIsOn = true;
        currentGuid = dialogue.idDialogue;
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentecen in dialogue.sentences)
        {
            sentences.Enqueue(sentecen);
        }


        DisplayNextSentence(false);
    }

    public void DisplayNextSentence(bool showYesNo)
    {
        if(sentences.Count==0) //its end of queue
        {
            if (showYesNo)
            {
                ShowPopUpYesNo();
            }
            else
            {
                EndDialogue();

            }
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private void ShowPopUpYesNo()
    {
        YesNo.SetActive(true);
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        YesNo.SetActive(false);
        animator.SetBool("IsOpen", false);
        currentlyADialogIsOn = false;
    }
}
