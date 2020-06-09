using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class NPC : MonoBehaviour
{


    bool playerInCLOSERange;
    bool playerInBigRange;

    Interactable interactable;
    [SerializeField] Sprite hasQuestsSprite;
    [SerializeField] Sprite hasOnGoingQuestsSprite;
    [SerializeField] GameObject hasQuestsLocation;
    BoxCollider2D boxCollider;
    public GameObject popUpLocation;
    public GameObject popUpDialogueLocation;

    string popUpOverPlayerNameToDestroy;

    void Start()
    {
        interactable = GetComponent<Interactable>();
        boxCollider = GetComponent<BoxCollider2D>();
        GlobalEvents.OnAcceptedQuest += PopQuestsExclamation;
        GlobalEvents.OnQuestCompleted += PopQuestsExclamation;
        PopQuestsExclamation(null);
    }

    public void PopQuestsExclamation(Quest questAdded)
    {
        if (interactable == null) return;
        if (interactable.displayOptions.Count > 0)
        {
            foreach (var item in interactable.displayOptions)
            {
                if (item.quest != null)
                {

                    if (UIManager.Instance.questsService.currentQuests.Contains(item.quest) == true)
                    {
                        ShowQuestsGoingOn();
                    }
                    else if (UIManager.Instance.questsService.completedQuests.Contains(item.quest) == false)
                    {
                        ShowQuestsAvaible();
                    }
                    else
                    {
                        RemoveQustsPopUp();
                    }
                }


            }
        }
    }

    private void RemoveQustsPopUp()
    {
        if (hasQuestsLocation != null)
        {
            foreach (Transform item in hasQuestsLocation.transform)
            {
                Destroy(item.gameObject);
            }
        }

    }

    public void UnsubscribFromEvents()
    {
        GlobalEvents.OnAcceptedQuest -= PopQuestsExclamation;
        GlobalEvents.OnQuestCompleted -= PopQuestsExclamation;
    }

    private void ShowQuestsGoingOn()
    {
        RemoveQustsPopUp();
        EmoteManager.Instance.DisplayCloseByPopUp(hasOnGoingQuestsSprite, hasQuestsLocation);

    }

    private void ShowQuestsAvaible()
    {
        RemoveQustsPopUp();
        EmoteManager.Instance.DisplayCloseByPopUp(hasQuestsSprite, hasQuestsLocation);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerInput>())
        {
            playerInCLOSERange = true;
            popUpOverPlayerNameToDestroy = EmoteManager.Instance.DisplayCloseByPopUp(interactable.popUpToDisplayOverPlayer, popUpLocation);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerInput>())
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
