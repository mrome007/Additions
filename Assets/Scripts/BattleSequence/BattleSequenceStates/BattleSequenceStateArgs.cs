using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequenceStateArgs
{
    public Party PlayerParty { get; private set; }
    public Party EnemyParty { get; private set; }
    public BattlePlayer CurrentPlayer { get; set; }

    public BattleSequenceStateArgs(Party player = null, Party enemy = null, BattlePlayer current = null)
    {
        PlayerParty = player;
        EnemyParty = enemy;
        CurrentPlayer = current;
    }
}
