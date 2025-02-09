using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : ScriptableObject
{
    public State nextState;
    public Action action;
    public bool actionValue;

    private bool CheckAction(GameObject owner)
    {
        return action.Check(owner) == actionValue;
    }

    public virtual State Run(GameObject owner)
    {
        if (CheckAction(owner))
        {
            return nextState;
        }
        return this; 
    }

    public virtual void OnStateEnter(GameObject owner)
    {
    }
}
