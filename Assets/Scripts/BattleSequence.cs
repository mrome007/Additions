using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleSequence : MonoBehaviour 
{
    [SerializeField]
    private Party darts;

    [SerializeField]
    private Party enemies;

    [SerializeField]
    private List<Selectable> battleSequenceMenuButtons;

    [SerializeField]
    private EnemyIndicator enemyIndicator;

    private Player currentPlayer;
    private Player currentTarget;

    private void Awake()
    {
        currentPlayer = null;
        currentTarget = null;
        ShowBattleSequenceMenu(false);
    }

    private void Start()
    {
        StartBattleSequence();
    }

    public void StartBattleSequence()
    {
        ShowBattleSequenceMenu(true);
        enemyIndicator.ShowEnemyIndicator(false);
        currentPlayer = darts.GetNextPlayer();
        currentPlayer.ActionEnd += HandleDartsActionEnd;
        //Debug.Log("Start");
    }

    //Player Choosing To Attack with the menu button.
    public void BattleAttack()
    {
        //Debug.Log("Battle Attack");
        ShowBattleSequenceMenu(false);
        StartCoroutine(PlayerSelectEnemyTarget());
    }

    private void ShowBattleSequenceMenu(bool show)
    {
        battleSequenceMenuButtons.ForEach(button => button.gameObject.SetActive(show));
    }

    private void HandleDartsActionEnd(object sender, ActionEventArgs e)
    {
        currentPlayer.ActionEnd -= HandleDartsActionEnd;
        ShowBattleSequenceMenu(false);
        currentPlayer = enemies.GetNextPlayer();
        currentPlayer.ActionEnd += HandleEnemiesActionEnd;
        currentPlayer.PlayerAttack(darts.GetRandomPlayer());
        //Debug.Log("Next Enemy");
    }

    private void HandleEnemiesActionEnd(object sender, ActionEventArgs e)
    {
        //TODO create a way to cycle between enemies.
        currentPlayer.ActionEnd -= HandleEnemiesActionEnd;
        ShowBattleSequenceMenu(true);
        currentPlayer = darts.GetNextPlayer();
        currentPlayer.ActionEnd += HandleDartsActionEnd;
        //Debug.Log("Next Dart");
    }

    private IEnumerator PlayerSelectEnemyTarget()
    {
        enemies.Reset();
        currentTarget = enemies.GetNextPlayer();
        enemyIndicator.MoveEnemyIndicator(currentTarget.transform.position);
        enemyIndicator.ShowEnemyIndicator(true);

        while(true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                break;
            }

            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentTarget = enemies.GetPreviousPlayer();
                enemyIndicator.MoveEnemyIndicator(currentTarget.transform.position);
            }

            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentTarget = enemies.GetNextPlayer();
                enemyIndicator.MoveEnemyIndicator(currentTarget.transform.position);
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        enemyIndicator.ShowEnemyIndicator(false);
        currentPlayer.PlayerAttack(currentTarget);

    }
}

