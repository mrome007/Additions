using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Player : MonoBehaviour 
{
    public event EventHandler ActionStart;
    public event EventHandler ActionEnd;

    protected virtual void StartAction()
    {
        var handler = ActionStart;
        if(handler != null)
        {
            handler(this, null);
        }
    }

    protected virtual void EndAction()
    {
        var handler = ActionEnd;
        if(handler != null)
        {
            handler(this, null);
        }
    }

    public virtual void PlayerAttack()
    {
        StartAction();
    }
}
