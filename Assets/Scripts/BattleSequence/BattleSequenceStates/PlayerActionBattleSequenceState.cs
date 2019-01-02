using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionBattleSequenceState : BattleSequenceState
{
    #region Inspector Data

    [SerializeField]
    private BattleSequenceIndicator playerIndicator;

    [SerializeField]
    private AdditionButtonController additionButtonController;

    [SerializeField]
    private PlayerActionButtonController actionButtonController;

    #endregion

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

    #region Helpers

    private void ShowBattleSequenceMenu(bool show)
    {
        additionButtonController.ShowAdditionButtons(false);
        actionButtonController.ShowActionButtons(show);

        if(show)
        {
            additionButtonController.ShowAdditionButtons(true, player.GetEnabledAdditions());
            StartCoroutine(PlayerSelectAction());
        }
    }

    //Move this to its own class.
    private IEnumerator PlayerSelectAction()
    {
        var currentAction = actionButtonController.GetCurrentActionButton();
        playerIndicator.MoveBattleSequenceIndicator(currentAction.transform.position);
        playerIndicator.ShowBattleSequenceIndicator(true);

        var currentAddition = additionButtonController.GetCurrentAdditionButton();
        if(currentAction.Action == ActionType.Attack)
        {
            playerIndicator.MoveBattleSequenceIndicator(currentAddition.transform.position);
        }

        while(true)
        {
            if(currentAction.Action == ActionType.Heal || currentAction.Action == ActionType.Defend)
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    break;
                }
            }

            if(currentAction.Action == ActionType.Attack)
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    player.ChangeAddition(currentAddition.AdditionIndex);
                    break;
                }

                if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    currentAddition = additionButtonController.GetNextAdditionButton();
                    playerIndicator.MoveBattleSequenceIndicator(currentAddition.transform.position);
                }

                if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    currentAddition = additionButtonController.GetPreviousAdditionButton();
                    playerIndicator.MoveBattleSequenceIndicator(currentAddition.transform.position);
                }
            }

            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentAction = actionButtonController.GetNextActionButton();
                playerIndicator.MoveBattleSequenceIndicator(currentAction.transform.position);

                if(currentAction.Action == ActionType.Attack)
                {
                    currentAddition = additionButtonController.GetCurrentAdditionButton();
                    playerIndicator.MoveBattleSequenceIndicator(currentAddition.transform.position);
                }
            }

            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentAction = actionButtonController.GetPreviousActionButton();
                playerIndicator.MoveBattleSequenceIndicator(currentAction.transform.position);

                if(currentAction.Action == ActionType.Attack)
                {
                    currentAddition = additionButtonController.GetCurrentAdditionButton();
                    playerIndicator.MoveBattleSequenceIndicator(currentAddition.transform.position);
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        playerIndicator.ShowBattleSequenceIndicator(false);

        switch(currentAction.Action)
        {
            case ActionType.Attack:
                BattleAttack();
                break;

                //TODO if in the future the main player have more party members, I will implement selecting the target
                // and not just healing yourself.
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

    public void BattleAttack()
    {
        //Debug.Log("Battle Attack");
        ShowBattleSequenceMenu(false);
        StartCoroutine(PlayerSelectEnemyTarget());
    }

    //Move this to its own class
    private IEnumerator PlayerSelectEnemyTarget()
    {
        enemies.Reset();
        var currentTarget = enemies.GetNextPlayer();
        playerIndicator.MoveBattleSequenceIndicator(currentTarget.transform.position);
        playerIndicator.ShowBattleSequenceIndicator(true);

        while(true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                break;
            }

            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentTarget = enemies.GetPreviousPlayer();
                playerIndicator.MoveBattleSequenceIndicator(currentTarget.transform.position);
            }

            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentTarget = enemies.GetNextPlayer();
                playerIndicator.MoveBattleSequenceIndicator(currentTarget.transform.position);
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        playerIndicator.ShowBattleSequenceIndicator(false);
        player.PlayerAttack(currentTarget);

        ExitState(stateArgs);
    }

    #endregion
}
