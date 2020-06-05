using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
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

            if (currentInteractableObjectScript)
                currentInteractableObjectScript.alreadyInteracted = true;
        }


    }

    private void DoInteractEvents()
    {


        if (currentInteractableObjectScript.soundEffectOnInteract != null)
        {
            SoundEffectsManager.instance.PlaySound(currentInteractableObjectScript.soundEffectOnInteract);
        }





        if (!String.IsNullOrEmpty(currentInteractableObjectScript.methodToCallInGmOnFirstClick))
        {
            GM.Instance.CallMethod(currentInteractableObjectScript.methodToCallInGmOnFirstClick, currentInteractableObjectScript.parameters);
        }


        if (currentInteractableObjectScript.TriggersDialogueOnClick && currentInteractableObjectScript.alreadyInteracted==false
            && (currentInteractableObjectScript.SimplePopUpTEXT.Length>0 || currentInteractableObjectScript.displayOptions.Count>0))
        {
            Vector2 targetLocationForDialoguePopUp = new Vector2(currentCollision.bounds.max.x, currentCollision.bounds.max.y);
            if (currentInteractableObjectScript.GetComponent<NPC>() != null)
            {
                targetLocationForDialoguePopUp = currentInteractableObjectScript.GetComponent<NPC>().popUpDialogueLocation.transform.position;
                currentInteractableObjectScript.GetComponent<NPC>().RemoveSmallPopUp();
            }
            GameObject gODialogue= DialogueManager.instance.InstantiateBubble(targetLocationForDialoguePopUp, new Dialogue() {
                NPCName=currentInteractableObjectScript.tileName,sentences=currentInteractableObjectScript.SimplePopUpTEXT.ToList(),
                options=currentInteractableObjectScript.displayOptions},currentInteractableObjectScript.isInfoBubble);
            temporaryPopUpText = gODialogue.name;
        }

        if (currentInteractableObjectScript.isLoot)
        {
            GetLoot();
            currentInteractableObjectScript.isLoot = false;
        }
        if (currentInteractableObjectScript.replaceTile != null)
        {
            //map.SetTile(currentCell, currentInteractableObjectScript.SpriteToReplace);
            //currentInteractableObjectScript.gameObject.GetComponent<SpriteRenderer>().sprite = currentInteractableObjectScript.SpriteToReplace;

            Instantiate(currentInteractableObjectScript.replaceTile, currentInteractableObjectScript.gameObject.transform.position, Quaternion.identity);
            if (currentInteractableObjectScript.SimplePopUpTEXT.Length > 0)
            {
                Vector2 targetLocationForDialoguePopUp = currentInteractableObjectScript.gameObject.transform.position;

                DialogueManager.instance.InstantiateBubbleAtPositionForXTime(targetLocationForDialoguePopUp, new Dialogue()
                {
                    NPCName = currentInteractableObjectScript.tileName,
                    sentences = currentInteractableObjectScript.SimplePopUpTEXT.ToList()
                }, true, 3);
            }
            Destroy(currentInteractableObjectScript.gameObject);
        }



    }

    private void GetLoot()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        bool touchingInteractable = GetComponent<BoxCollider2D>().IsTouchingLayers(interactableLayer);

        if (currentInteractableObjectScript) return;
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
                    if (currentInteractableObjectScript.SimplePopUpTEXT.Length > 0)
                    {
                        foreach (var item in currentInteractableObjectScript.SimplePopUpTEXT)
                        {
                            sentences.Add(item);
                        }
                    }

                    else if (currentInteractableObjectScript.Quests.Count == 1)
                    {
                        foreach (var item in currentInteractableObjectScript.Quests[0].StartQuestDialogue)
                        {
                            sentences.Add(item);
                        }
                        currentInteractableObjectScript.selectedQuest = currentInteractableObjectScript.Quests[0];

                    }

                    Vector2 dialogueLocation = new Vector2(currentInteractableObjectScript.transform.position.x, currentInteractableObjectScript.transform.position.y + 0.5f);
                    GameObject gODialogue = DialogueManager.instance.InstantiateBubble(dialogueLocation,
                        new Dialogue() { NPCName = currentInteractableObjectScript.tileName, sentences = sentences, Quest = currentInteractableObjectScript.selectedQuest }, currentInteractableObjectScript.isInfoBubble);

                    temporaryPopUpText = gODialogue.name;

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
        if (collision.transform.gameObject.layer == LayerMask.NameToLayer("interactable"))
        {
            sittingOverAnotherInteractableObject = false;
            if (currentInteractableObjectScript)
            {
                currentInteractableObjectScript.alreadyInteracted = false;
                currentInteractableObjectScript = null;
                if (!String.IsNullOrEmpty(temporaryPopUpText))
                {
                    Destroy(GameObject.Find(temporaryPopUpText));
                    temporaryPopUpText = "";

                }
            }
        }



    }


}
