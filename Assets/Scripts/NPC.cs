using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class NPC : MonoBehaviour
{


    bool playerInCLOSERange;
    bool playerInBigRange;

    Interactable interactable;

    BoxCollider2D boxCollider;
    public GameObject popUpLocation;
    public GameObject popUpDialogueLocation;

    string popUpOverPlayerNameToDestroy;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerInput>() != false)
        {
            playerInCLOSERange = true;
            Vector2 position = boxCollider.bounds.max;
            popUpOverPlayerNameToDestroy = EmoteManager.Instance.DisplayCloseByPopUp(interactable.popUpToDisplayOverPlayer, popUpLocation);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerInput>() != false)
        {
            playerInCLOSERange = false;

            RemoveSmallPopUp();

        }
    }

    public void RemoveSmallPopUp()
    {
        if (!string.IsNullOrEmpty(popUpOverPlayerNameToDestroy))
        {
            Destroy(GameObject.Find(popUpOverPlayerNameToDestroy));
            popUpOverPlayerNameToDestroy = "";
        }
    }
}
