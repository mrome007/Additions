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
    
    [SerializeField]
    private List<Player> players;
    
    private int currentPlayer;
    
    public Party()
    {
        currentPlayer = -1;
    }
    
    public Player GetNextPlayer()
    {
        ++currentPlayer;
        currentPlayer %= players.Count;
        var player = players[currentPlayer];
        return player;
    }

    public Player GetPreviousPlayer()
    {
        if(currentPlayer < 0)
        {
            currentPlayer = currentPlayer + players.Count;
        }

        --currentPlayer;

        if(currentPlayer < 0)
        {
            currentPlayer = currentPlayer + players.Count;
        }
        else
        {
            currentPlayer %= players.Count;
        }

        var player = players[currentPlayer];

        return player;
    }

    public Player GetRandomPlayer()
    {
        var player = players[UnityEngine.Random.Range(0, players.Count)];
        return player;
    }
    
    public void Reset()
    {
        currentPlayer = -1;
    }
}
