using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEventArgs : EventArgs
{
    public ActionType ActionType { get; private set; }
    public int DataPoints { get; private set; }
    public BattlePlayer Target { get; private set; }

    public ActionEventArgs(ActionType act, int dat, BattlePlayer targ)
    {
        this.ActionType = act;
        this.DataPoints = dat;
        this.Target = targ;
    }
}

public enum ActionType
{
    Idle,
    Attack,
    Defend,
    Heal
}
