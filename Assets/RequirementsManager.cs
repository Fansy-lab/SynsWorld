using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RequirementsManager : MonoBehaviour
{

    private static RequirementsManager instance;
    private static int m_referenceCount = 0;


    public static RequirementsManager Instance
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

    public bool EvaluateIfMeetsRequirement(Requirements requirements)
    {
        if (requirements.beOnQuests != null)
        {
            foreach (var quest in requirements.beOnQuests)
            {
                if (!UIManager.Instance.questsService.currentQuests.Contains(quest))
                {
                    return false;
                }

            }
        }


        if (requirements.haveFinishedQuest != null)
        {
            foreach (var quest in requirements.haveFinishedQuest)
            {
                if (!UIManager.Instance.questsService.completedQuests.Contains(quest))
                {
                    return false;
                }

            }
        }
        return true;
    }

}
