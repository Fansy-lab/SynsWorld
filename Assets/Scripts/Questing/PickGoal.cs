using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class PickGoal : MonoBehaviour
{

    public PickGoalData pickGoalData;
    public void Init()
    {
        GlobalEvents.OnPickedItem += ItemPicked;

    }

    private void OnDestroy()
    {

        UnsubscribeFromEvents();
    }


    void ItemPicked(InventoryItem item)
    {
       if (!pickGoalData.Completed)
        {
            if (item.itemName == pickGoalData.ItemName)
            {
                pickGoalData.CurrentAmmount++;
                #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
                #endif
                Evaluate();
            }
        }

    }
    public void Evaluate()
    {
        if (pickGoalData.CurrentAmmount >= pickGoalData.RequiredAmmount)
        {
            Complete();

        }
    }
    public void Complete()
    {
        pickGoalData.Completed = true;
        GlobalEvents.PickedGoalCompleted(this);

    }

    internal void UnsubscribeFromEvents()
    {
         GlobalEvents.OnPickedItem -= ItemPicked;
    }
}
