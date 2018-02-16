using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSequence : MonoBehaviour 
{
    #region Instance

    public static BattleSequence Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (BattleSequence)FindObjectOfType(typeof(BattleSequence));
            }
            return instance;
        }
    }

    private static BattleSequence instance = null;

    #endregion

    [SerializeField]
    private Party darts;

    [SerializeField]
    private Party enemies;

    [SerializeField]
    private List<Transform> battleSequenceMenuButtons;

    [SerializeField]
    private EnemyIndicator enemyIndicator;

    private BattlePlayer currentPlayer;
    private BattlePlayer currentTarget;
    private Queue<BattlePlayer> playerBattleQueue;
    private List<int> playersTurnPoints;

    private int turnPointsLimit = 17;

    private void Awake()
    {
        if(instance == null)
        {
            instance = (BattleSequence)FindObjectOfType(typeof(BattleSequence));
        }
    }

    public void EndBattleSequence()
    {
        darts.ClearPlayersFromParty();
        enemies.ClearPlayersFromParty();
        Reset();
    }

    public void StartBattleSequence()
    {
        Reset();
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
        if(show)
        {
            StartCoroutine(PlayerSelectAddition());
        }
    }

    private IEnumerator PlayerSelectAddition()
    {
        var index = 0;
        var currentAddition = battleSequenceMenuButtons[index];
        enemyIndicator.MoveEnemyIndicator(currentAddition.transform.position);
        enemyIndicator.ShowEnemyIndicator(true);

        while(true)
        {
            //TEMPORARY just testing whether I can go back to overworld just fine.
            if(Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(UnloadBattleSequenceScene());
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                var dartPlayer = currentPlayer.GetComponent<DartBattlePlayer>();
                if(dartPlayer != null)
                {
                    dartPlayer.ChangeAddition(index);
                }
                break;
            }

            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                index++;
                index %= battleSequenceMenuButtons.Count;
                currentAddition = battleSequenceMenuButtons[index];
                enemyIndicator.MoveEnemyIndicator(currentAddition.transform.position);
            }

            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                index--;
                if(index < 0)
                {
                    index += battleSequenceMenuButtons.Count;
                }
                currentAddition = battleSequenceMenuButtons[index];
                enemyIndicator.MoveEnemyIndicator(currentAddition.transform.position);
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        enemyIndicator.ShowEnemyIndicator(false);
        BattleAttack();
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

        if(currentPlayer.GetComponent<EnemyBattlePlayer>() != null)
        {
            ShowBattleSequenceMenu(false);
            enemyIndicator.MoveEnemyIndicator(currentPlayer.transform.position);
            enemyIndicator.ShowEnemyIndicator(true);
            //TODO work on how enemy target darts.
            currentPlayer.PlayerAttack(currentTarget);
        }
        else
        {
            enemyIndicator.ShowEnemyIndicator(false);
            ShowBattleSequenceMenu(true);
        }
    }

    private void Reset()
    {
        StopCoroutine("PlayerSelectAddition");
        currentPlayer = null;
        currentTarget = null;
        ShowBattleSequenceMenu(false);
        playerBattleQueue = new Queue<BattlePlayer>();
        playersTurnPoints = new List<int>();
        for(int index = 0; index < darts.NumberOfPlayers + enemies.NumberOfPlayers; index++)
        {
            playersTurnPoints.Add(0);
        }
        playersTurnPoints.ForEach(turnPoints => turnPoints = 0);
        enemyIndicator.ShowEnemyIndicator(false);
    }

    public void PopulateParties(List<BattlePlayerCreator.Darts> dts, List<BattlePlayerCreator.Enemies> enm)
    {
        dts.ForEach(dartType => darts.AddPlayerToParty(BattlePlayerCreator.Instance.CreateDartBattlePlayer(dartType)));
        enm.ForEach(enemyType => enemies.AddPlayerToParty(BattlePlayerCreator.Instance.CreateEnemyBattlePlayer(enemyType)));
    }

    //TEMPORARY
    private IEnumerator UnloadBattleSequenceScene()
    {
        EndBattleSequence();
        var asyncUnload = SceneManager.UnloadSceneAsync("BattleSequence");

        while(!asyncUnload.isDone)
        {
            yield return null;
        }
    }
}

