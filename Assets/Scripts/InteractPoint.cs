using System;
using System.Collections;
using System.Collections.Generic;
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


    private void Update()
    {
        if (sittingOverAnotherInteractableObject)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentInteractableObjectScript != null && currentInteractableObjectScript.TriggersDialogue &&
                    (currentInteractableObjectScript.alreadyInteracted == false || currentInteractableObjectScript.interactableMultipleTimes))
                {

                    if (currentInteractableObjectScript.popUpToDisplayOverPlayer!=null)
                    {
                        EmoteManager.Instance.DisplayPopUp(currentInteractableObjectScript.popUpToDisplayOverPlayer);
                    }
                    bool dialogIsUp = DialogueInstance.Instance.GetComponent<DialogueManager>().currentlyADialogIsOn;

                    if (!dialogIsUp)
                    {
                        currentInteractableObjectScript.TriggerDialogue();
                    }
                    if (dialogIsUp && IsNewDialog())
                    {
                        currentInteractableObjectScript.TriggerDialogue();

                    }
                    if(dialogIsUp && !IsNewDialog())
                    {
                        DialogueInstance.Instance.GetComponent<DialogueManager>().DisplayNextSentence(currentInteractableObjectScript.ShowQuestTEXTPopUp);

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
                {
                    DialogueInstance.Instance.GetComponent<DialogueManager>().DisplayNextSentence(currentInteractableObjectScript.ShowQuestTEXTPopUp);

                }


            }
        }



    }

    private void CheckInteractableProperties()
    {

        if (currentInteractableObjectScript)
        {
            if ((currentInteractableObjectScript.interactableMultipleTimes))
            {
                DoInteractEvents();

            }
            else if (currentInteractableObjectScript.alreadyInteracted == false)
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

        //check if prepare quest question
        if (currentInteractableObjectScript.questToStart !=null)
        {
            UIManager.Instance.questsService.questToStart = currentInteractableObjectScript.questToStart; ;


        }
    
    }

    private bool IsNewDialog()
    {
        if (DialogueInstance.Instance.GetComponent<DialogueManager>().currentGuid == currentInteractableObjectScript.startDialogue.idDialogue)
        {
            return false;
        }
        return true;

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {


        //bool touchingInteractable = GetComponent<BoxCollider2D>().IsTouchingLayers(interactableLayer);


        //if (touchingInteractable)
        //{
        //    currentInteractableObjectScript = collision.GetComponent<Interactable>();
        //    if (currentInteractableObjectScript != null)
        //    {
        //        currentCollision = collision;
        //        sittingOverAnotherInteractableObject = true;
        //    }
          
        //}
        //else
        //{
        //    sittingOverAnotherInteractableObject = false;
        //}




    }
    private void OnTriggerExit2D(Collider2D collision)
    {
     
        //    sittingOverAnotherInteractableObject = false;
    }
}
