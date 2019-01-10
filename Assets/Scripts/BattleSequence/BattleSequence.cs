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
    private BattleSequenceState initialState;

    [SerializeField]
    private Party darts;

    [SerializeField]
    private Party enemies;

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
    }

    public void StartBattleSequence(List<BattlePlayer> goodGuys, List<BattlePlayer> badGuys)
    {
        PopulateParties(goodGuys, badGuys);
        initialState.EnterState(new BattleSequenceStateArgs(darts, enemies));
    }
        
    public void PopulateParties(List<BattlePlayer> dts, List<BattlePlayer> enm)
    {
        dts.ForEach(goodGuys => darts.AddPlayerToParty(goodGuys));
        enm.ForEach(badGuys => enemies.AddPlayerToParty(badGuys));
    }
}

