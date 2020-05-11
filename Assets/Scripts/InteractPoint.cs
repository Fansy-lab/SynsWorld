using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractPoint : MonoBehaviour
{
    public static bool sittingOverAnotherInteractableObject = false;
    
    public LayerMask interactableLayer;
    public static Interactable currentInteractableObjectScript;
    Tile emptyTile;
    public static Collider2D currentCollision;

    string temporaryPopUpText;

    private void Update()
    {
        if (sittingOverAnotherInteractableObject)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentInteractableObjectScript != null && currentInteractableObjectScript.TriggersDialogueOnClick &&
                    (currentInteractableObjectScript.alreadyInteracted == false))
                {

                    if (currentInteractableObjectScript.popUpToDisplayOverPlayer!=null)
                    {
                        EmoteManager.Instance.DisplayPopUp(currentInteractableObjectScript.popUpToDisplayOverPlayer);
                    }
                 
                   

                }
                CheckInteractableProperties();

            }


        }
      



    }

    private void CheckInteractableProperties()
    {

        if (currentInteractableObjectScript)
        {
            

            
            if (currentInteractableObjectScript.alreadyInteracted == false)
            {
                DoInteractEvents();
            }


            currentInteractableObjectScript.alreadyInteracted = true;
        }


    }

    private void DoInteractEvents()
    {
        



        if (currentInteractableObjectScript.replaceTileWithEmpty)
        {
         
            Destroy(currentCollision.gameObject);
        }

        else if (currentInteractableObjectScript.replaceTile)
        {
            //map.SetTile(currentCell, currentInteractableObjectScript.SpriteToReplace);
            currentInteractableObjectScript.gameObject.GetComponent<SpriteRenderer>().sprite = currentInteractableObjectScript.SpriteToReplace;

        }

        //check if has to do something
        if (currentInteractableObjectScript.methodToCallInGm !="")
        {
            GM.Instance.CallMethod(currentInteractableObjectScript.methodToCallInGm,currentInteractableObjectScript.parameters);
        }

        if (currentInteractableObjectScript.TriggersDialogueOnClick && currentInteractableObjectScript.alreadyInteracted==false
            && currentInteractableObjectScript.DialogueText.Length>0)
        {
            temporaryPopUpText= DialogueManager.instance.InstantiateBubble(gameObject.transform.parent.position, new Dialogue() {
                NPCName=currentInteractableObjectScript.tileName,sentences=currentInteractableObjectScript.DialogueText.ToList()});
        }



    }

   



    private void OnTriggerEnter2D(Collider2D collision)
    {


        bool touchingInteractable = GetComponent<BoxCollider2D>().IsTouchingLayers(interactableLayer);


        if (touchingInteractable)
        {
            currentInteractableObjectScript = collision.GetComponent<Interactable>();
            if (currentInteractableObjectScript != null)
            {
                currentCollision = collision;
                sittingOverAnotherInteractableObject = true;
                if (currentInteractableObjectScript.TriggersDialogueOnLook)
                {

                    List<string> sentences = new List<string>();
                    foreach (var item in currentInteractableObjectScript.DialogueText)
                    {
                        sentences.Add(item);
                    }

                    temporaryPopUpText = DialogueManager.instance.InstantiateBubble(transform.position,
                        new Dialogue() { NPCName = currentInteractableObjectScript.tileName, sentences = sentences, quest = currentInteractableObjectScript.questToStart });

                }
            }

        }
        else
        {
            sittingOverAnotherInteractableObject = false;
        }




    }
    private void OnTriggerExit2D(Collider2D collision)
    {
     
            sittingOverAnotherInteractableObject = false;
        if (currentInteractableObjectScript)
        {
            currentInteractableObjectScript.alreadyInteracted = false;
        }
        if (!String.IsNullOrEmpty(temporaryPopUpText))
        {
           
                Destroy(GameObject.Find(temporaryPopUpText));
                temporaryPopUpText = "";
            
        }
     
    }
}
