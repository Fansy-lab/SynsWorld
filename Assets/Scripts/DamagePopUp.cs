using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamagePopUp : MonoBehaviour
{
    public float destroyTime=3f;
    TextMeshPro textMesh;
    Transform rT;

    public Vector3 randomizeIntesity = new Vector3(0.15f,0.0f,0);
  
    public int damageAmmount;
  
    private void Start()
    {
      
        textMesh = GetComponent<TextMeshPro>();
        textMesh.renderer.sortingLayerName = "OverCharacters";
        textMesh.SetText(damageAmmount.ToString());
      //  rT = transform.gameObject.GetComponent<Transform>();
       // rT.localPosition = new Vector2(0, 1.25f);
       // rT.localPosition += new Vector3(Random.Range(-randomizeIntesity.x, randomizeIntesity.x),Random.Range(-randomizeIntesity.y, randomizeIntesity.y), Random.Range(-randomizeIntesity.z, randomizeIntesity.z));
        Destroy(gameObject, destroyTime);
    }
}
