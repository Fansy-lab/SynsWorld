using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GM : MonoBehaviour
{
 


   [SerializeField] private GameObject questsUI;
   [SerializeField] private GameObject inventoryUI;
   [SerializeField] private GameObject mainMenuUI;
   [SerializeField] private GameObject settingsUI;
    [SerializeField] private GameObject enterLeaveLocationUI;

     public GameObject Player;




    private static GM instance;
    private static int m_referenceCount = 0;


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
        InitialSetup();

    }

    public void InitialSetup()
    {
        mainMenuUI.SetActive(true);
        inventoryUI.SetActive(false);
        questsUI.SetActive(false);
        settingsUI.SetActive(false);

        enterLeaveLocationUI.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (questsUI.activeInHierarchy)
            {
                questsUI.SetActive(false);
                return;
            }
          


            if (inventoryUI.activeInHierarchy)
            {
                inventoryUI.SetActive(false);
                return;
            }

            if (settingsUI.activeInHierarchy)
            {
                settingsUI.SetActive(false);
                return;
            }
    


            bool mainMenuStatus = mainMenuUI.activeSelf;
            mainMenuUI.SetActive(!mainMenuStatus);
        }
    }

    internal void ToggleInventoryPanel()
    {
        bool a = inventoryUI.activeInHierarchy;
        inventoryUI.SetActive(!a);
    }



   
    public void ToggleQuests()
    {
        bool a = questsUI.activeInHierarchy;
        if (a)
        {
            SoundEffectsManager.instance.PlayCloseQuestSound();
        }
        else
        {
            SoundEffectsManager.instance.PlayOpenQuestSound();
        }
        questsUI.SetActive(!a);
    }
    public void ToggleSettings()
    {
        mainMenuUI.SetActive(false);
        bool a = settingsUI.activeInHierarchy;
        settingsUI.SetActive(!a);
    }

    public void PrintHi()
    {
        print("hi");
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
