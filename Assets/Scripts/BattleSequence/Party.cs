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
    
    private int currentPlayerIndex;
    
    public Party()
    {
        currentPlayerIndex = -1;
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
        battlePlayers.ForEach(member => GameObject.Destroy(member.gameObject));
        battlePlayers.Clear();
    }

    public BattlePlayer GetNextPlayer()
    {
        BattlePlayer player;

        do
        {
            ++currentPlayerIndex;
            currentPlayerIndex %= battlePlayers.Count;
            player = battlePlayers[currentPlayerIndex];
        } 
        while(!player.Alive);

        return player;
    }

    public BattlePlayer GetPreviousPlayer()
    {
        BattlePlayer player;

        do
        {
            if(currentPlayerIndex < 0)
            {
                currentPlayerIndex = currentPlayerIndex + battlePlayers.Count;
            }

            --currentPlayerIndex;

            if(currentPlayerIndex < 0)
            {
                currentPlayerIndex = currentPlayerIndex + battlePlayers.Count;
            }
            else
            {
                currentPlayerIndex %= battlePlayers.Count;
            }

            player = battlePlayers[currentPlayerIndex];
        } 
        while(!player.Alive);

        return player;
    }

    public BattlePlayer GetPlayer(int index)
    {
        var player = battlePlayers[index];
        return player;
    }

    public bool IsPartyAlive()
    {
        return battlePlayers.Exists(player => player.Alive);
    }
    
    public void Reset()
    {
        currentPlayerIndex = -1;
    }
}
