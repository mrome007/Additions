using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionBattleSequenceState : BattleSequenceState
{
    private DartBattlePlayer player;
    private Party enemies;

    public override void EnterState(BattleSequenceStateArgs enterArgs = null)
    {
        #if UNITY_EDITOR
        Debug.Log("Entering Player Action Battle Sequence State.");
        #endif

        stateArgs = enterArgs;

        base.EnterState(enterArgs);

        player = enterArgs.CurrentPlayer as DartBattlePlayer;
        enemies = enterArgs.EnemyParty;
    }

    public override void ExitState(BattleSequenceStateArgs exitArgs = null)
    {
        #if UNITY_EDITOR
        Debug.Log("Exiting Player Action Battle Sequence State.");
        #endif

        base.ExitState(exitArgs);
    }
}
