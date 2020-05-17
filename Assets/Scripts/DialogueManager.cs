using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static DialogueManager instance;
    private static int m_referenceCount = 0;
    private void Awake()
    {
        m_referenceCount++;
        if (m_referenceCount > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        // Use this line if you need the object to persist across scenes
        DontDestroyOnLoad(this.gameObject);
    }

    public  GameObject bubbleToDisplay;


    public GameObject InstantiateBubble(Vector3 position,Dialogue dialogueInfo)
    {
        GameObject go = Instantiate(bubbleToDisplay,new Vector3(position.x,position.y), Quaternion.identity) as GameObject;
        go.name =  Guid.NewGuid().ToString();
        Dialogue dialogue = go.GetComponent<Dialogue>();
        dialogue.sentences = dialogueInfo.sentences;
        dialogue.NPCName = dialogueInfo.NPCName;
        dialogue.Quest = dialogueInfo.Quest;
        if(dialogueInfo.options != null)
        {
            dialogue.options = dialogueInfo.options;
        }


        dialogue.StartDialogue();

        return go;

    }



}
