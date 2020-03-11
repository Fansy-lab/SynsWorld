using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractPoint : MonoBehaviour
{
    bool sittingOverAnotherInteractableObject = false;
    Interactable currentInteractableObjectScript;

    Tile emptyTile;

    Collider2D currentCollision;


    private void Update()
    {

        if (sittingOverAnotherInteractableObject)
        {
            bool dialogIsUp = DialogueInstance.Instance.GetComponent<DialogueManager>().currentlyADialogIsOn;

            if (Input.GetKeyDown(KeyCode.E) && !dialogIsUp)
            {
                currentInteractableObjectScript.TriggerDialogue();
            }
            if (Input.GetKeyDown(KeyCode.E) && dialogIsUp && IsNewDialog())
            {
                currentInteractableObjectScript.TriggerDialogue();

            }

            //check interactable
            if (Input.GetKeyDown(KeyCode.E))
            {
                CheckInteractable();
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

    private void CheckInteractable()
    {

        if (currentInteractableObjectScript)
        {
            if (currentInteractableObjectScript.interactableMultipleTimes)
            {
                DoInteractEvents();
            
            }
            else if(currentInteractableObjectScript.alreadyInteracted == false)
            {
                DoInteractEvents();
            }
         

            currentInteractableObjectScript.alreadyInteracted = true;
        }


    }

    private void DoInteractEvents()
    {
        Tilemap map = currentCollision.GetComponent<Tilemap>();
        Vector3Int currentCell = map.WorldToCell(transform.position);



        map.layoutGrid.CellToWorld(currentCell);
        TileBase basee = map.GetTile(currentCell);
        if (currentInteractableObjectScript.replaceTileWithEmpty)
        {
            map.SetTile(currentCell, emptyTile);
            GameObject go = GameObject.Find(currentCollision.name);
            Destroy(go);
        }
        else if (currentInteractableObjectScript.replace2UpWithEmpty)
        {
            Vector3Int secondCell = currentCell;
            secondCell.y++;
            map.SetTile(currentCell, emptyTile);
            map.SetTile(secondCell, emptyTile);
            GameObject go = GameObject.Find(currentInteractableObjectScript.name);
            Destroy(go);

        }
        else if (currentInteractableObjectScript.replace2TilesUp)
        {
            Vector3Int secondCell = currentCell;
            secondCell.y++;
            map.SetTile(currentCell, currentInteractableObjectScript.tile1ToReplace);
            map.SetTile(secondCell, currentInteractableObjectScript.tile2ToReplace);
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


        if (collision.name == "Interactable")
        {

            currentCollision = collision;


        }


        if (collision.transform.tag == "Interactable")
        {
            currentInteractableObjectScript = collision.GetComponent<Interactable>();



        }

        if (currentInteractableObjectScript != null)
        {
            sittingOverAnotherInteractableObject = true;



        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        sittingOverAnotherInteractableObject = false;
    }
}
