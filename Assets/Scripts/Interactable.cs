using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Interactable : MonoBehaviour
{

    [Header("Tile replacement")]
    public Sprite SpriteToReplace;
    public Sprite SpriteToPlaceOnLeaveTrigger;
    public bool justDestroyTile=false;
    public bool replaceTileWithEmpty = false;
    public bool replaceTile = false;
    [Header("Tile properties")]
    public bool alreadyInteracted = false;
    public bool TriggersDialogueOnLook;
    public bool TriggersDialogueOnClick;


    [Header("Quest")]
    public List<Quest> Quests;
    public Quest selectedQuest;
    public Sprite popUpToDisplayOverPlayer;
    public string tileName;

    [TextArea(3, 5)]
    public string[] SimplePopUpTEXT;
    public bool isInfoBubble=false;


    public bool isNPC;
    public List<DisplayOption> displayOptions;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "InteractPoint")
        {

            InteractPoint.currentInteractableObjectScript = this;
            InteractPoint.sittingOverAnotherInteractableObject = true;
            InteractPoint.currentCollision = collision;



        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        InteractPoint.sittingOverAnotherInteractableObject = false;
        InteractPoint.currentCollision = null;

        if (SpriteToPlaceOnLeaveTrigger)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = SpriteToPlaceOnLeaveTrigger;
        }

    }

}
