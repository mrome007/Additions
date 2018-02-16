using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Party
{
    //TODO Determine which player is the faster and return that for the next player.
    //Also have a method that checks every body's health in the party. This will be used
    //to determine when the battle sequence ends.

    public int NumberOfPlayers { get { return battlePlayers.Count; } }

    private List<BattlePlayer> battlePlayers;

    [SerializeField]
    private Transform partyPosition;

    [SerializeField]
    private float playerPositionOffset;
    
    private int currentPlayer;
    
    public Party()
    {
        currentPlayer = -1;
        battlePlayers = new List<BattlePlayer>();
    }

    public void AddPlayerToParty(BattlePlayer player)
    {
        var partyPos = partyPosition.position;
        player.transform.position = partyPos;
        partyPos.x += playerPositionOffset;
        partyPosition.position = partyPos;
        battlePlayers.Add(player);
    }

    public void ClearPlayersFromParty()
    {
        Reset();
        battlePlayers.Clear();
    }

    public BattlePlayer GetNextPlayer()
    {
        ++currentPlayer;
        currentPlayer %= battlePlayers.Count;
        var player = battlePlayers[currentPlayer];
        return player;
    }

    public BattlePlayer GetPreviousPlayer()
    {
        if(currentPlayer < 0)
        {
            currentPlayer = currentPlayer + battlePlayers.Count;
        }

        --currentPlayer;

        if(currentPlayer < 0)
        {
            currentPlayer = currentPlayer + battlePlayers.Count;
        }
        else
        {
            currentPlayer %= battlePlayers.Count;
        }

        var player = battlePlayers[currentPlayer];

        return player;
    }

    public BattlePlayer GetPlayer(int index)
    {
        var player = battlePlayers[index];
        return player;
    }
    
    public void Reset()
    {
        currentPlayer = -1;
    }
}
