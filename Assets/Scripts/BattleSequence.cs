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
    private Queue<Player> playerBattleQueue;
    private List<int> playersTurnPoints;

    private int turnPointsLimit = 17;

    private void Awake()
    {
        currentPlayer = null;
        currentTarget = null;
        ShowBattleSequenceMenu(false);
        playerBattleQueue = new Queue<Player>();
        playersTurnPoints = new List<int>();
        for(int index = 0; index < darts.NumberOfPlayers + enemies.NumberOfPlayers; index++)
        {
            playersTurnPoints.Add(0);
        }
        playersTurnPoints.ForEach(turnPoints => turnPoints = 0);
    }

    private void Start()
    {
        StartBattleSequence();
    }

    public void StartBattleSequence()
    {
        QueuePlayers();
        StartPlayerTurn();
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

    private void HandlePlayerActionEnd(object sender, ActionEventArgs e)
    {
        currentPlayer.ActionEnd -= HandlePlayerActionEnd;
        if(playerBattleQueue.Count == 0)
        {
            QueuePlayers();
        }

        StartPlayerTurn();
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

    private void QueuePlayers()
    {
        while(playerBattleQueue.Count == 0)
        {
            for(int index = 0; index < darts.NumberOfPlayers; index++)
            {
                var dart = darts.GetPlayer(index);
                playersTurnPoints[index] += dart.TurnPoints;

                if(playersTurnPoints[index] >= turnPointsLimit)
                {
                    playerBattleQueue.Enqueue(dart);
                    playersTurnPoints[index] %= turnPointsLimit;
                }
            }      

            for(int index = darts.NumberOfPlayers; index < playersTurnPoints.Count; index++)
            {
                var enemy = enemies.GetPlayer(index - darts.NumberOfPlayers);
                playersTurnPoints[index] += enemy.TurnPoints;

                if(playersTurnPoints[index] >= turnPointsLimit)
                {
                    playerBattleQueue.Enqueue(enemy);
                    playersTurnPoints[index] %= turnPointsLimit;
                }
            }
        }
    }

    private void StartPlayerTurn()
    {
        currentPlayer = playerBattleQueue.Dequeue();
        currentPlayer.ActionEnd += HandlePlayerActionEnd;

        if(currentPlayer.GetComponent<Enemy>() != null)
        {
            ShowBattleSequenceMenu(false);
            enemyIndicator.MoveEnemyIndicator(currentPlayer.transform.position);
            enemyIndicator.ShowEnemyIndicator(true);
            //TODO work on how enemy target darts.
            currentPlayer.PlayerAttack(currentTarget);
        }
        else
        {
            ShowBattleSequenceMenu(true);
            enemyIndicator.ShowEnemyIndicator(false);
        }
    }
}

