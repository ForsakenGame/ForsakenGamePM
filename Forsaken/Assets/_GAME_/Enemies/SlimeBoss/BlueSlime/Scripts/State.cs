using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public State nextState;
    public bool actionValue;
    public Action action;

    private bool CheckAction(GameObject owner)
    {
        return action.Check(owner) == actionValue; // te devuelve si la accion se ha cumplido  
    }

    public virtual State Run(GameObject owner)
    {
        if(CheckAction(owner))
        {
            return nextState;
        }

        return this;
    }
}
