using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NumberPopUpManager : MonoBehaviour
{
    private static NumberPopUpManager instance;
    public float destroyTime=3f;
    SortingLayer layer;
    TextMeshPro textMesh;
    Transform rT;

    public Vector3 randomizeIntesity = new Vector3(0.15f,0.0f,0);

    public string textToDisplay;


    public GameObject popUp;

    private static int m_referenceCount = 0;


    public static NumberPopUpManager Instance
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
    private void Start()
    {



    }

    public  void DisplayDamageTaken(string textToDisplay, Vector3 position)
    {

        Vector3 randomizeIntesity = new Vector3(0.15f, 0.0f, 0);
        position += new Vector3(UnityEngine.Random.Range(-randomizeIntesity.x, randomizeIntesity.x), UnityEngine.Random.Range(-randomizeIntesity.y, randomizeIntesity.y), UnityEngine.Random.Range(-randomizeIntesity.z, randomizeIntesity.z));

        GameObject gO = Instantiate(popUp, position, Quaternion.identity) as GameObject;
        gO.GetComponentInChildren<TextMeshPro>().color = Color.red;
        gO.GetComponentInChildren<TextMeshPro>().text = textToDisplay;

        Destroy(gO, 1f);
    }
    public  void DisplayExperienceGained(string textToDisplay, Vector3 position)
    {
        Vector3 randomizeIntesity = new Vector3(0.15f, 0.0f, 0);
        position += new Vector3(UnityEngine.Random.Range(-randomizeIntesity.x, randomizeIntesity.x), UnityEngine.Random.Range(-randomizeIntesity.y, randomizeIntesity.y), UnityEngine.Random.Range(-randomizeIntesity.z, randomizeIntesity.z));

        GameObject gO = Instantiate(popUp, position, Quaternion.identity) as GameObject;
        gO.GetComponentInChildren<TextMeshPro>().color = Color.yellow;
        gO.GetComponentInChildren<TextMeshPro>().text ="+"+textToDisplay+"exp";

        Destroy(gO, 1f);

    }
    public  void DisplayDamageDone(string textToDisplay,Vector3 position)
    {
        Vector3 randomizeIntesity = new Vector3(0.15f, 0.0f, 0);
        position += new Vector3(UnityEngine.Random.Range(-randomizeIntesity.x, randomizeIntesity.x), UnityEngine.Random.Range(-randomizeIntesity.y, randomizeIntesity.y), UnityEngine.Random.Range(-randomizeIntesity.z, randomizeIntesity.z));

        GameObject gO = Instantiate(popUp, position, Quaternion.identity) as GameObject;
        gO.GetComponentInChildren<TextMeshPro>().color = Color.white;
        gO.GetComponentInChildren<TextMeshPro>().text = textToDisplay;
        Destroy(gO, 1f);
    }
}
