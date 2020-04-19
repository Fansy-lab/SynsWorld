using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal 
{
    public string Description { get; set; }
    public Quest quest { get; set; }
    public bool Completed { get; set; }
    public int CurrentAmmount { get; set; }
    public int RequiredAmmount { get; set; }

    public virtual void Init()
    {
        //defualt initStuff
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
        this.quest.CheckGoals();
        
    }

}
