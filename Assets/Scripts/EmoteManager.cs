using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteManager : MonoBehaviour
{
    private static EmoteManager instance;
    private static int m_referenceCount = 0;
    [SerializeField] Transform popUpPosition;
    [SerializeField] GameObject newQuestEmote;
    [SerializeField] GameObject CompletedQuestEmote;
    public static EmoteManager Instance
    {
        get
        {
            return instance;
        }
    }

    internal void ShowNewQuestEmote()
    {
        GameObject gOSpawned = Instantiate(newQuestEmote, popUpPosition.position, Quaternion.identity) as GameObject;
        gOSpawned.transform.parent = gameObject.transform;
    }
    internal void ShowCompletedQuestEmote()
    {
        GameObject gOSpawned = Instantiate(CompletedQuestEmote, popUpPosition.position, Quaternion.identity) as GameObject;
        gOSpawned.transform.parent = gameObject.transform;
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

    }
    public void DisplayPopUp(GameObject gO)
    {
        GameObject gOSpawned = Instantiate(gO, popUpPosition.position, Quaternion.identity) as GameObject;
        gOSpawned.transform.parent = gameObject.transform;
    }
}
