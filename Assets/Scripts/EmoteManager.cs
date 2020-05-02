using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteManager : MonoBehaviour
{
    private static EmoteManager instance;
    private static int m_referenceCount = 0;
    [Header("stuff you should not touch")]
    [SerializeField] Transform popUpPosition;
    [SerializeField] Sprite NewQuestSprite;
    [SerializeField] Sprite CompletedQuestSprite;
    [SerializeField] GameObject EmotePrefab;

    public static EmoteManager Instance
    {
        get
        {
            return instance;
        }
    }

    internal void ShowNewQuestEmote()
    {
        EmotePrefab.GetComponentInChildren<SpriteRenderer>().sprite = NewQuestSprite;
        GameObject gOSpawned = Instantiate(EmotePrefab, popUpPosition.position, Quaternion.identity) as GameObject;
        gOSpawned.transform.parent = gameObject.transform;
    }
    internal void ShowCompletedQuestEmote()
    {
        EmotePrefab.GetComponentInChildren<SpriteRenderer>().sprite = CompletedQuestSprite;

        GameObject gOSpawned = Instantiate(EmotePrefab, popUpPosition.position, Quaternion.identity) as GameObject;
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
    public void DisplayPopUp(Sprite sprite)
    {
        EmotePrefab.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        GameObject gOSpawned = Instantiate(EmotePrefab, popUpPosition.position, Quaternion.identity) as GameObject;
        gOSpawned.transform.parent = gameObject.transform;
    }
}
