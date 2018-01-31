using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEventArgs : EventArgs
{
    public ActionType ActionType { get; private set; }
    public float HitPoints { get; private set; }

    public ActionEventArgs(ActionType act, float hp)
    {
        this.ActionType = act;
        this.HitPoints = hp;
    }
}

public enum ActionType
{
    Idle,
    Attack,
    Defend,
    Heal
}
