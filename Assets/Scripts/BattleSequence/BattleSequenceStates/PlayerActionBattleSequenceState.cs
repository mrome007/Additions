using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionBattleSequenceState : BattleSequenceState
{
    public override void EnterState(BattleSequenceStateArgs enterArgs = null)
    {
        #if UNITY_EDITOR
        Debug.Log("Entering Player Action Battle Sequence State.");
        #endif

        base.EnterState(enterArgs);
    }

    public override void ExitState(BattleSequenceStateArgs exitArgs = null)
    {
        #if UNITY_EDITOR
        Debug.Log("Exiting Player Action Battle Sequence State.");
        #endif

        base.ExitState(exitArgs);
    }
}
