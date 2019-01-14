using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuePlayersBattleSequenceState : BattleSequenceState
{
    //Use DefaultNextState as the player action state and the state below
    //for enemy action state.
    [SerializeField]
    private BattleSequenceState enemyActionBattleSequenceState;
    
    private Queue<BattlePlayer> playerBattleQueue;
    private List<int> playersTurnPoints;
    private int turnPointsLimit = 9;

    public override void EnterState(BattleSequenceStateArgs enterArgs = null)
    {
        #if UNITY_EDITOR
        Debug.Log("Entering Queue Players Battle Sequence State");
        #endif

        base.EnterState(enterArgs);

        if(stateArgs == null)
        {
            stateArgs = new BattleSequenceStateArgs(enterArgs.PlayerParty, enterArgs.EnemyParty);
        }

        if(playerBattleQueue == null)
        {
            playerBattleQueue = new Queue<BattlePlayer>();
            playersTurnPoints = new List<int>();
            var total = enterArgs.PlayerParty.NumberOfPlayers + enterArgs.EnemyParty.NumberOfPlayers;
            for(int index = 0; index < total; index++)
            {
                playersTurnPoints.Add(0);
            }
        }

        QueuePlayers(enterArgs.PlayerParty, enterArgs.EnemyParty);
        stateArgs.CurrentPlayer = GetNextAlivePlayer();
        ExitState(stateArgs);
    }

    public override void ExitState(BattleSequenceStateArgs exitArgs = null)
    {
        #if UNITY_EDITOR
        Debug.Log("Exiting Queue Players Battle Sequence State");
        #endif

        var enemy = exitArgs.CurrentPlayer.GetComponent<EnemyBattlePlayer>() != null;
        nextState = enemy ? enemyActionBattleSequenceState : defaultNextState;
        base.ExitState(exitArgs);
    }

    private void QueuePlayers(Party players, Party enemies)
    {
        if(playerBattleQueue.Count == 0)
        {
            for(int index = 0; index < players.NumberOfPlayers; index++)
            {
                var dart = players.GetPlayer(index);
                playersTurnPoints[index] += dart.Alive ? dart.PlayerStats.Speed : 0;

                if(playersTurnPoints[index] >= turnPointsLimit)
                {
                    playerBattleQueue.Enqueue(dart);
                    playersTurnPoints[index] %= turnPointsLimit;
                }
            }      

            for(int index = players.NumberOfPlayers; index < playersTurnPoints.Count; index++)
            {
                var enemy = enemies.GetPlayer(index - players.NumberOfPlayers);
                playersTurnPoints[index] += enemy.Alive ? enemy.PlayerStats.Speed : 0;

                if(playersTurnPoints[index] >= turnPointsLimit)
                {
                    playerBattleQueue.Enqueue(enemy);
                    playersTurnPoints[index] %= turnPointsLimit;
                }
            }
        }
    }

    private BattlePlayer GetNextAlivePlayer()
    {
        while(playerBattleQueue.Count > 0 && !playerBattleQueue.Peek().Alive)
        {
            playerBattleQueue.Dequeue();
        }

        QueuePlayers(stateArgs.PlayerParty, stateArgs.EnemyParty);

        return playerBattleQueue.Count > 0 ? playerBattleQueue.Dequeue() : null;
    }
}
