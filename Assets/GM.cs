using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GM : MonoBehaviour
{
    private static GM instance;
    private static int m_referenceCount = 0;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;

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
        playerMovement = player.GetComponent<PlayerMovement>();
        playerStats = player.GetComponent<PlayerStats>();

    }

    public void EnableShooting()
    {
        playerStats.EnableShooting();
    }
    public void DisableShooting()
    {
        playerStats.DisableShooting();

    }

    internal void CallMethod(string methodToCallInGm)
    {
        Type thisType = this.GetType();
        MethodInfo theMethodToCall = thisType.GetMethod(methodToCallInGm);
        theMethodToCall.Invoke(this, null);
    }
}
