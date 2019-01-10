using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionBattleSequenceState : BattleSequenceState
{
    [SerializeField]
    private BattleSequenceInput battleSequenceInput;
    
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

        battleSequenceInput.StartBattleSequenceInput(enemies);
    }

    public override void ExitState(BattleSequenceStateArgs exitArgs = null)
    {
        #if UNITY_EDITOR
        Debug.Log("Exiting Player Action Battle Sequence State.");
        #endif

        base.ExitState(exitArgs);
    }

    public override void RegisterEvents()
    {
        base.RegisterEvents();

        battleSequenceInput.PlayerActionSelectionFinished += HandlePlayerActionSelectionFinished;
        battleSequenceInput.PlayerEnemyTargetSelectionFinished += HandleEnemyTargetSelectionFinished;
    }

    public override void UnRegisterEvents()
    {
        base.UnRegisterEvents();

        battleSequenceInput.PlayerActionSelectionFinished -= HandlePlayerActionSelectionFinished;
        battleSequenceInput.PlayerEnemyTargetSelectionFinished -= HandleEnemyTargetSelectionFinished;
    }

    private void HandlePlayerActionEnd(object sender, ActionEventArgs e)
    {
        player.ActionEnd -= HandlePlayerActionEnd;
    }

    private void HandleEnemyTargetSelectionFinished(object sender, ActionEventArgs e)
    {
        player.ActionEnd += HandlePlayerActionEnd;

        player.PlayerAttack(e.Target);
    }

    private void HandlePlayerActionSelectionFinished(object sender, ActionEventArgs e)
    {
        player.ActionEnd += HandlePlayerActionEnd;
        switch(e.ActionType)
        {
            case ActionType.Defend:
                player.PlayerDefend(player);
                break;
            case ActionType.Heal:
                player.PlayerHeal(player);
                break;
            default:
                break;
        }
    }
}
