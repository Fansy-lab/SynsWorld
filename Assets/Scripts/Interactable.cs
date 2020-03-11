using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Interactable : MonoBehaviour
{
    public Dialogue dialogue;

   
    public Tile tile1ToReplace;
    public Tile tile2ToReplace;

    public bool justDestroyTile=false;
    public bool replaceTileWithEmpty = false;
    public bool replace2UpWithEmpty = false;

    public bool replaceTileWith1Tile = false;
    public bool replace2TilesUp = false;

    public bool hasSomeKindOfEvent = false;
    public bool interactableMultipleTimes = false;
    public bool alreadyInteracted = false;

    public string methodToCallInGm;

    public void TriggerDialogue()
    {
         DialogueInstance.Instance.GetComponent<DialogueManager>().StartDialogue(dialogue);
    }

  
}
