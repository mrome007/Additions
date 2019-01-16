using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSequence : MonoBehaviour 
{
    [SerializeField]
    private BattleSequenceState initialState;

    [SerializeField]
    private Party darts;

    [SerializeField]
    private Party enemies;

    private void Awake()
    {
        BattleSequenceTransition.BattleSequenceStart += HandleBattleSequenceStart;
    }

    private void OnDestroy()
    {
        BattleSequenceTransition.BattleSequenceStart -= HandleBattleSequenceStart;
    }

    private void HandleBattleSequenceStart(object sender, BattleSequenceStartEventArgs e)
    {
        BattleSequenceTransition.BattleSequenceStart -= HandleBattleSequenceStart;
        StartBattleSequence(e.Players, e.Enemies);
    }

    private void StartBattleSequence(List<BattlePlayer> goodGuys, List<BattlePlayer> badGuys)
    {
        PopulateParties(goodGuys, badGuys);
        initialState.EnterState(new BattleSequenceStateArgs(darts, enemies));
    }
        
    private void PopulateParties(List<BattlePlayer> dts, List<BattlePlayer> enm)
    {
        dts.ForEach(goodGuys => darts.AddPlayerToParty(goodGuys));
        enm.ForEach(badGuys => enemies.AddPlayerToParty(badGuys));
    }
}

