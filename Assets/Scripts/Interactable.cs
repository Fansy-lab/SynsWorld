using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Interactable : MonoBehaviour
{
    public Dialogue dialogue;

   
    public Sprite SpriteToReplace;

    public bool justDestroyTile=false;
    public bool replaceTileWithEmpty = false;

    public bool replaceTileWith1Tile = false;

    public bool hasSomeKindOfEvent = false;
    public bool interactableMultipleTimes = false;
    public bool alreadyInteracted = false;

    public bool ThisInteractableTriggersDialogue;


    public string methodToCallInGm;

    public void TriggerDialogue()
    {
         DialogueInstance.Instance.GetComponent<DialogueManager>().StartDialogue(dialogue);
    }

  
}
