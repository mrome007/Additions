using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEventArgs : EventArgs
{
    public ActionType ActionType { get; private set; }
    public int HitPoints { get; private set; }
    public BattlePlayer Target { get; private set; }

    public ActionEventArgs(ActionType act, int hp, BattlePlayer targ)
    {
        this.ActionType = act;
        this.HitPoints = hp;
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
