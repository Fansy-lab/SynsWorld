using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsBarManager : MonoBehaviour
{
    private static SkillsBarManager instance;
    private static int m_referenceCount = 0;

    public static SkillsBarManager Instance
    {
        get
        {
            return instance;
        }
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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
