using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleWonArgs : EventArgs 
{
    public bool Win { get; private set; }

    public BattleWonArgs(bool win)
    {
        this.Win = win;
    }
}
