using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Questing/New Pick Goal")]

public class PickGoal : ScriptableObject
{
    public int idPickGoal;
    public string Description;
    public bool Completed;
    public int CurrentAmmount;
    public int RequiredAmmount;

    public int ItemID;
    public string ItemName;
    public void Init()
    {
        GlobalEvents.OnPickedItem += ItemPicked;

    }

    private void OnDestroy()
    {
        GlobalEvents.OnPickedItem -= ItemPicked;

    }


    void ItemPicked(InventoryItem item)
    {
       if (!Completed)
        {
            if (item.itemName == this.ItemName)
            {
                this.CurrentAmmount++;
                #if UNITY_EDITOR
                EditorUtility.SetDirty(this);
                #endif
                Evaluate();
            }
        }

    }
    public void Evaluate()
    {
        if (CurrentAmmount >= RequiredAmmount)
        {
            Complete();

        }
    }
    public void Complete()
    {
        Completed = true;
        GlobalEvents.PickedGoalCompleted(this);

    }

}
