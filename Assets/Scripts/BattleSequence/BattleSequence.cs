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
    private BattleSequenceIndicator battleIndicator;

    private BattlePlayer currentPlayer;
    private BattlePlayer currentTarget;
    private Queue<BattlePlayer> playerBattleQueue;
    private List<int> playersTurnPoints;

    private int turnPointsLimit = 7;

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

    public void StartBattleSequence(List<BattlePlayer> goodGuys, List<BattlePlayer> badGuys)
    {
        PopulateParties(goodGuys, badGuys);
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
        battleIndicator.MoveBattleSequenceIndicator(currentAddition.transform.position);
        battleIndicator.ShowBattleSequenceIndicator(true);

        while(true)
        {
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
                battleIndicator.MoveBattleSequenceIndicator(currentAddition.transform.position);
            }

            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                index--;
                if(index < 0)
                {
                    index += battleSequenceMenuButtons.Count;
                }
                currentAddition = battleSequenceMenuButtons[index];
                battleIndicator.MoveBattleSequenceIndicator(currentAddition.transform.position);
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        battleIndicator.ShowBattleSequenceIndicator(false);
        BattleAttack();
    }

    private void HandlePlayerActionEnd(object sender, ActionEventArgs e)
    {
        currentPlayer.ActionEnd -= HandlePlayerActionEnd;

        //if both parties are still alive.
        if(darts.IsPartyAlive() && enemies.IsPartyAlive())
        {
            //If next player is dead, skip it.
            while(playerBattleQueue.Count > 0 && !playerBattleQueue.Peek().Alive) 
            {
                playerBattleQueue.Dequeue();
            }
            
            if(playerBattleQueue.Count == 0)
            {
                QueuePlayers();
            }

            StartPlayerTurn();
        }
        else        //If either side has died.
        {
            //TODO Temporary Do Winning or Losing animation.
            if(darts.IsPartyAlive())
            {
                var exp = enemies.GetPartyExperiencePoints();
                BattleSequenceTransition.Instance.MainPlayer.IncrementExperiencePoints(exp);
                BattleSequenceTransition.Instance.MainPlayer.Health = darts.GetNextPlayer().PlayerStats.Health;
                BattleSequenceTransition.Instance.UpdateAdditionsInformation(((DartBattlePlayer)darts.GetNextPlayer()).GetAdditionsCount());
            }

            BattleSequenceTransition.Instance.UnloadBattleSequence(darts.IsPartyAlive()); //If darts are alive then they have won the battle.
            EndBattleSequence();
        }
    }

    private IEnumerator PlayerSelectEnemyTarget()
    {
        enemies.Reset();
        currentTarget = enemies.GetNextPlayer();
        battleIndicator.MoveBattleSequenceIndicator(currentTarget.transform.position);
        battleIndicator.ShowBattleSequenceIndicator(true);

        while(true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                break;
            }

            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentTarget = enemies.GetPreviousPlayer();
                battleIndicator.MoveBattleSequenceIndicator(currentTarget.transform.position);
            }

            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentTarget = enemies.GetNextPlayer();
                battleIndicator.MoveBattleSequenceIndicator(currentTarget.transform.position);
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        battleIndicator.ShowBattleSequenceIndicator(false);
        currentPlayer.PlayerAttack(currentTarget);

    }

    private void QueuePlayers()
    {
        while(playerBattleQueue.Count == 0)
        {
            for(int index = 0; index < darts.NumberOfPlayers; index++)
            {
                var dart = darts.GetPlayer(index);
                playersTurnPoints[index] += dart.Alive ? dart.PlayerStats.Speed : 0;

                if(playersTurnPoints[index] >= turnPointsLimit)
                {
                    playerBattleQueue.Enqueue(dart);
                    playersTurnPoints[index] %= turnPointsLimit;
                }
            }      

            for(int index = darts.NumberOfPlayers; index < playersTurnPoints.Count; index++)
            {
                var enemy = enemies.GetPlayer(index - darts.NumberOfPlayers);
                playersTurnPoints[index] += enemy.Alive ? enemy.PlayerStats.Speed : 0;

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
            battleIndicator.MoveBattleSequenceIndicator(currentPlayer.transform.position);
            battleIndicator.ShowBattleSequenceIndicator(true);
            //TODO work on how enemy target darts.
            currentTarget = darts.GetNextPlayer();
            currentPlayer.PlayerAttack(currentTarget);
        }
        else
        {
            battleIndicator.ShowBattleSequenceIndicator(false);
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
        battleIndicator.ShowBattleSequenceIndicator(false);
    }

    public void PopulateParties(List<BattlePlayer> dts, List<BattlePlayer> enm)
    {
        dts.ForEach(goodGuys => darts.AddPlayerToParty(goodGuys));
        enm.ForEach(badGuys => enemies.AddPlayerToParty(badGuys));
    }
}

