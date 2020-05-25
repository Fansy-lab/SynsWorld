using System;
using UnityEngine;

public class EmoteManager : MonoBehaviour
{
    private static EmoteManager instance;
    private static int m_referenceCount = 0;

    [Header("stuff you should not touch")]
    [SerializeField] private Transform popUpPosition;

    [SerializeField] private Sprite NewQuestSprite;
    [SerializeField] private Sprite CompletedQuestSprite;
    [SerializeField] private GameObject EmotePrefab;

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

    internal string DisplayCloseByPopUp(Sprite popUpToDisplayOverPlayer, GameObject gO)
    {
        EmotePrefab.GetComponentInChildren<SpriteRenderer>().sprite = popUpToDisplayOverPlayer;

        GameObject gOSpawned = Instantiate(EmotePrefab, gO.transform.position, Quaternion.identity, gO.transform) as GameObject;

        gOSpawned.GetComponent<Animator>().Play("PopUpStationary");
        gOSpawned.GetComponent<SelfDestroyAfterSeconds>().enabled = false;
        gOSpawned.name = Guid.NewGuid().ToString();
        return gOSpawned.name;
    }

    internal void ShowCompletedQuestEmote()
    {
        EmotePrefab.GetComponentInChildren<SpriteRenderer>().sprite = CompletedQuestSprite;

        GameObject gOSpawned = Instantiate(EmotePrefab, popUpPosition.position, Quaternion.identity) as GameObject;
        gOSpawned.transform.parent = gameObject.transform;
    }

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

    private void OnDestroy()
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