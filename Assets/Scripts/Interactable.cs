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
    public bool ThisInteractableTriggersDialogue;
    public bool ShowQuestTEXTPopUp;
    [Header("Call method in GM")]
    public string methodToCallInGm;
    public List<string> parameters;
    [Header("Quest")]
    public Quest questToStart;
    public GameObject popUpToDisplayOverPlayer;
    public int EndsQuestID;
    public Dialogue EndQuestDialogue;


    public void TriggerDialogue()
    {
         DialogueInstance.Instance.GetComponent<DialogueManager>().StartDialogue(startDialogue);
    }

  
}
