﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateChest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerStats>() != null)
        {
            if(GM.Instance.PrivateChestInventoryUI.activeInHierarchy)
                GM.Instance.CloseInventoryAndPrivateChest();
        }
    }
}
