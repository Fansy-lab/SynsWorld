using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform questParentObject;
    public GameObject questUIGameObject;
    public GameObject questInfo;
    private static UIManager instance;
    private static int m_referenceCount = 0;
    private Quest currentQuestsInfoBeingShown;
    public static UIManager Instance
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

    internal void RemoveQuestScrollFromUI(Quest quest)
    {
        foreach (Transform item in questParentObject)
        {
            if (item.name == quest.QuestID.ToString())
            {
                Destroy(item.gameObject);
            }
        }
    }

    public void AddNewQuestToTheUIList(Quest quest)
    {
        GameObject gO = Instantiate(questUIGameObject, transform.position, transform.rotation);
        RectTransform rectTransform = gO.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector3.zero;
        gO.transform.SetParent(questParentObject, false);
        gO.GetComponentInChildren<TextMeshProUGUI>().text = quest.QuestName;
        gO.name = quest.QuestID.ToString();
        gO.GetComponent<Button>().onClick.AddListener(() => ShowQuestInfo(quest));
    }

    public void AbandonQuest()
    {
        if (currentQuestsInfoBeingShown != null)
        {
            RemoveQuestScrollFromUI(currentQuestsInfoBeingShown);
            GM.Instance.AbandonQuest(currentQuestsInfoBeingShown);
            ToggleQuestInfo();
            currentQuestsInfoBeingShown = null;
        }
    }
    void ShowQuestInfo(Quest quest)
    {
        currentQuestsInfoBeingShown = quest;
        ToggleQuestInfo();
        foreach (Transform child in questInfo.transform)
        {
            if (child.name == "QuestTitle")
            {
                child.GetComponentInChildren<TextMeshProUGUI>().text = quest.QuestName;
            }
            if (child.name == "QuestDescription")
            {
                child.GetComponentInChildren<TextMeshProUGUI>().text = quest.QuestDescription;

            }
            if (child.name == "RewardsPanel")
            {
                foreach (Transform childCHild in child.transform)
                {
                    if (childCHild.name == "GoldImage")
                    {
                        childCHild.GetComponentInChildren<TextMeshProUGUI>().text = quest.GoldReward.ToString();

                    }
                    if (childCHild.name == "ExpImage")
                    {
                        childCHild.GetComponentInChildren<TextMeshProUGUI>().text=quest.ExpReward.ToString();
                    }
                }
            }
        }
    }

    public void ToggleQuestInfo()
    {
        bool state =!questInfo.gameObject.activeInHierarchy;
        questInfo.SetActive(state);

    }
}
