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
    private QuestsService questsService;
    public GameObject questsUI;
    public GameObject inventoryUI;

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
        questsService = player.GetComponentInChildren<QuestsService>();

    }

    internal void ToggleInventoryPanel()
    {
        bool a = inventoryUI.activeInHierarchy;
        inventoryUI.SetActive(!a);
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
        if(questToStart != null)
        {
            Quest newQuest = ScriptableObject.CreateInstance("Quest") as Quest;
            newQuest.Init(questToStart.QuestID, questToStart.KillGoals, questToStart.QuestName, questToStart.QuestDescription, questToStart.ExpReward, questToStart.GoldReward, questToStart.ItemReward, questToStart.IsCompleted);

            
            questsService.AddNewQuest(newQuest);

        }
    }
    public void CompletedQuest(Quest quest)
    {

        questsService.AddQuestToCompletedQuestsAndRemoveQuestFromUI(quest);
        questsService.RemoveFromCurrentQuests(quest);
       
    }
    public void AbandonQuest(Quest quest)
    {

        questsService.RemoveFromCurrentQuests(quest);
        Quest[] questInstances = GameObject.FindObjectsOfType<Quest>();
        foreach (var questFound in questInstances)
        {
            if(questFound.QuestID==quest.QuestID)
              questsService.AbandonQuest(questFound);

        }

    }

    public void UseItem(InventoryItem itemToUse)
    {
        Debug.Log("Using Item" + itemToUse.itemName);
        Debug.Log("Voy a recuperar: " + itemToUse.usableStats.HPRestoreAmmount + "puntos de vida");

    }
    public void EquipItem(InventoryItem itemToEquip)
    {
        Debug.Log("Equiping Item" + itemToEquip.itemName);

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
