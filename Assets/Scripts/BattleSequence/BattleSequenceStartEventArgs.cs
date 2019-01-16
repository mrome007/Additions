using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequenceStartEventArgs : EventArgs
{
    public List<BattlePlayer> Players { get; private set; }
    public List<BattlePlayer> Enemies { get; private set; }

    public BattleSequenceStartEventArgs(List<BattlePlayer> good, List<BattlePlayer> bad)
    {
        Players = good;
        Enemies = bad;
    }
}
