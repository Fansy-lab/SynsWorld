using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{

    public static TransitionManager instance;
    Animator anim;
    private static int m_referenceCount = 0;

    private void Awake()
    {
        m_referenceCount++;
        if (m_referenceCount > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowNormalTransition()
    {
        anim.SetTrigger("Start");
    }
    public void EndNormalTransition()
    {
        anim.SetTrigger("End");
    }
}
