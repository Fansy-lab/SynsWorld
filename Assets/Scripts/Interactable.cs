using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Interactable : MonoBehaviour
{
    [Header("Dialogue")]
    public Dialogue startDialogue;
    [Header("Tile replacement")]
    public Sprite SpriteToReplace;
    public bool justDestroyTile=false;
    public bool replaceTileWithEmpty = false;
    public bool replaceTile = false;
    [Header("Tile properties")]
    public bool interactableMultipleTimes = false;
    public bool alreadyInteracted = false;
    public bool TriggersDialogue;
    public bool ShowQuestTEXTPopUp;
    [Header("Call method in GM")]
    public string methodToCallInGm;
    public List<string> parameters;
    [Header("Quest")]
    public Quest questToStart;
    public Sprite popUpToDisplayOverPlayer;
    public int EndsQuestID;
    public Dialogue EndQuestDialogue;


    public void TriggerDialogue()
    {
         DialogueInstance.Instance.GetComponent<DialogueManager>().StartDialogue(startDialogue);
    }

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
    }

}
