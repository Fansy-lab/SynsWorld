using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class GM : MonoBehaviour
{
 
    private PlayerInput playerMovement;
    private PlayerStats playerStats;
    private PlayersQuests playerQuests;

    public GameObject questsUI;

    private static GM instance;
    private static int m_referenceCount = 0;

    public Quest questToStart;

    public static GM Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
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

    void OnDestroy()
    {
        m_referenceCount--;
        if (m_referenceCount == 0)
        {
            instance = null;
        }

    }

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerInput>();
        playerStats = player.GetComponent<PlayerStats>();
        playerQuests = player.GetComponentInChildren<PlayersQuests>();

    }


    public void DeclineQuest()
    {
        DialogueInstance.Instance.GetComponent<DialogueManager>().EndDialogue();
  
    }

    public void EnableShooting()
    {
        playerStats.EnableShooting();
    }

    

    public void DisableShooting()
    {
        playerStats.DisableShooting();

    }
    public void ToggleQuests()
    {
        bool a = questsUI.activeInHierarchy;
        questsUI.SetActive(!a);
    }

    public void StartQuest()
    {
       if(questToStart !=null)
         playerQuests.AddNewQuest(questToStart);
    }
    public void CompletedQuest(Quest quest)
    {
        playerQuests.AddQuestToCompletedQuestsAndRemoveQuestFromUI(quest);

    }
    public void AbandonQuest(Quest quest)
    {
        playerQuests.AbandonQuest(quest);
    }

    internal void CallMethod(string methodToCallInGm,List<string> parameters)
    {
        Type thisType = this.GetType();
        MethodInfo theMethodToCall = thisType.GetMethod(methodToCallInGm);
        object[] objects;
        objects = parameters.Cast<object>().ToArray();

        theMethodToCall.Invoke(this,objects);
    }
}
