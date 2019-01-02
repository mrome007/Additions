using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActionBattleSequenceState : BattleSequenceState
{
    #region Inspector data

    [SerializeField]
    private BattleSequenceIndicator enemyIndicator;

    #endregion
    
    private BattlePlayer enemy;

    public override void EnterState(BattleSequenceStateArgs enterArgs = null)
    {
        #if UNITY_EDITOR
        Debug.Log("Entering Enemy Action Battle Sequence State.");
        #endif

        base.EnterState(enterArgs);

        stateArgs = enterArgs;

        enemy = enterArgs.CurrentPlayer;
        enemy.ActionEnd += HandleEnemyActionEnd;
        var target = enterArgs.PlayerParty.GetNextPlayer();
        enemy.PlayerAttack(target);

        enemyIndicator.MoveBattleSequenceIndicator(enemy.transform.position);
        enemyIndicator.ShowBattleSequenceIndicator(true);
    }

    public override void ExitState(BattleSequenceStateArgs exitArgs = null)
    {
        #if UNITY_EDITOR
        Debug.Log("Exiting Enemy Action Battle Sequence State.");
        #endif

        enemyIndicator.ShowBattleSequenceIndicator(false);

        base.ExitState(exitArgs);
    }

    private void HandleEnemyActionEnd(object sender, ActionEventArgs e)
    {
        enemy.ActionEnd -= HandleEnemyActionEnd;
        ExitState(stateArgs);
    }
}
