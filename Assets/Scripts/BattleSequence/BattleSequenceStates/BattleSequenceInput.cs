using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequenceInput : MonoBehaviour 
{
    [SerializeField]
    private BattleSequenceIndicator selectionIndicator;

    [SerializeField]
    private PlayerActionButtonController playerActionButtonController;
    
    public event EventHandler<ActionEventArgs> PlayerActionSelectionFinished;
    public event EventHandler<ActionEventArgs> PlayerEnemyTargetSelectionFinished;

    public void StartBattleSequenceInput(Party enemies)
    {
        StartCoroutine(PlayerSelectAction(enemies));
    }

    public void StartEnemyTargetSelection(Party enemies)
    {
        StartCoroutine(EnemyTargetSelection(enemies));
    }

    private IEnumerator EnemyTargetSelection(Party enemies)
    {
        var currentTarget = enemies.GetNextPlayer();
        selectionIndicator.MoveBattleSequenceIndicator(currentTarget.transform.position);
        selectionIndicator.ShowBattleSequenceIndicator(true);

        var enemySelect = true;
        while(true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                break;
            }

            if(Input.GetKeyDown(KeyCode.Backspace))
            {
                enemySelect = false;
                break;
            }

            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentTarget = enemies.GetPreviousPlayer();
                selectionIndicator.MoveBattleSequenceIndicator(currentTarget.transform.position);
            }

            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentTarget = enemies.GetNextPlayer();
                selectionIndicator.MoveBattleSequenceIndicator(currentTarget.transform.position);
            }

            yield return null;
        }

        if(enemySelect)
        {
            yield return new WaitForSeconds(0.25f);
            PostPlayerEnemyTargetSelectionFinished(currentTarget);
        }
        else
        {
            StartCoroutine(PlayerSelectAction(enemies));
        }
    }

    //Move this to its own class.
    private IEnumerator PlayerSelectAction(Party enemies)
    {
        playerActionButtonController.ShowActionButtons(true);
        var currentAction = playerActionButtonController.GetCurrentActionButton();
        selectionIndicator.MoveBattleSequenceIndicator(currentAction.transform.position);
        selectionIndicator.ShowBattleSequenceIndicator(true);

        while(true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                break;
            }

            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentAction = playerActionButtonController.GetPreviousActionButton();
                selectionIndicator.MoveBattleSequenceIndicator(currentAction.transform.position);
            }

            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentAction = playerActionButtonController.GetNextActionButton();
                selectionIndicator.MoveBattleSequenceIndicator(currentAction.transform.position);
            }

            yield return null;
        }

        switch(currentAction.Action)
        {
            case ActionType.Defend:
            case ActionType.Heal:
                PostPlayerActionSelectionFinished(currentAction.Action);
                break;
            case ActionType.Attack:
                yield return new WaitForSeconds(0.25f);
                StartEnemyTargetSelection(enemies);
                break;
            default:
                break;
        }
        playerActionButtonController.ShowActionButtons(false);
    }

    private void PostPlayerActionSelectionFinished(ActionType action)
    {
        var handler = PlayerActionSelectionFinished;
        if(handler != null)
        {
            handler(this, new ActionEventArgs(action, 0, null));
        }
    }

    private void PostPlayerEnemyTargetSelectionFinished(BattlePlayer target)
    {
        var handler = PlayerEnemyTargetSelectionFinished;
        if(handler != null)
        {
            handler(this, new ActionEventArgs(ActionType.Attack, 0, target));
        }
    }
}
