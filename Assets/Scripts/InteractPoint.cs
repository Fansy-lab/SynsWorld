using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractPoint : MonoBehaviour
{
    bool sittingOverAnotherInteractableObject = false;
    public LayerMask interactableLayer;
    Interactable currentInteractableObjectScript;

    Tile emptyTile;

    Collider2D currentCollision;


    private void Update()
    {

        if (sittingOverAnotherInteractableObject)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentInteractableObjectScript != null && currentInteractableObjectScript.ThisInteractableTriggersDialogue &&
                    (currentInteractableObjectScript.alreadyInteracted == false || currentInteractableObjectScript.interactableMultipleTimes))
                {
                   
                    bool dialogIsUp = DialogueInstance.Instance.GetComponent<DialogueManager>().currentlyADialogIsOn;

                    if (!dialogIsUp)
                    {
                        currentInteractableObjectScript.TriggerDialogue();
                    }
                    if (dialogIsUp && IsNewDialog())
                    {
                        currentInteractableObjectScript.TriggerDialogue();

                    }

                }
                CheckInteractableProperties();

            }
              

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //dialog stuff
                bool dialogIsUp = DialogueInstance.Instance.GetComponent<DialogueManager>().currentlyADialogIsOn;
                if (dialogIsUp)
                    DialogueInstance.Instance.GetComponent<DialogueManager>().DisplayNextSentence();


            }
        }



    }

    private void CheckInteractableProperties()
    {

        if (currentInteractableObjectScript)
        {
            if ((currentInteractableObjectScript.interactableMultipleTimes) )
            {
                DoInteractEvents();
            
            } else if (currentInteractableObjectScript.alreadyInteracted == false)
            {
                DoInteractEvents();
            }
            

            currentInteractableObjectScript.alreadyInteracted = true;
        }


    }

    private void DoInteractEvents()
    {
        Tilemap map = currentCollision.transform.parent.GetComponent<Tilemap>();
        Vector3Int currentCell = map.WorldToCell(transform.position);



        map.layoutGrid.CellToWorld(currentCell);
        if (currentInteractableObjectScript.replaceTileWithEmpty)
        {
            map.SetTile(currentCell, emptyTile);
            GameObject go = GameObject.Find(currentCollision.name);
            Destroy(go);
        }

        else if (currentInteractableObjectScript.replaceTileWith1Tile)
        {
            //map.SetTile(currentCell, currentInteractableObjectScript.SpriteToReplace);
            currentCollision.gameObject.GetComponent<SpriteRenderer>().sprite = currentInteractableObjectScript.SpriteToReplace;

        }




        //check if has to do something
        if (currentInteractableObjectScript.hasSomeKindOfEvent)
        {
            GM.Instance.CallMethod(currentInteractableObjectScript.methodToCallInGm);
        }
    }

    private bool IsNewDialog()
    {
        if (DialogueInstance.Instance.GetComponent<DialogueManager>().currentGuid == currentInteractableObjectScript.dialogue.idDialogue)
        {
            return false;
        }
        return true;

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {


         bool touchingInteractable =GetComponent<BoxCollider2D>().IsTouchingLayers(interactableLayer);
        

        if (touchingInteractable)
        {
            currentCollision = collision;
            currentInteractableObjectScript = collision.GetComponent<Interactable>();
            sittingOverAnotherInteractableObject = true;
        }
        else
        {
            sittingOverAnotherInteractableObject = false;
        }

     
       

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        sittingOverAnotherInteractableObject = false;
    }
}
