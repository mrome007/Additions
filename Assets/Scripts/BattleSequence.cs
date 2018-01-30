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

    private Player currentPlayer;
    private Player targetPlayer;

    private void Awake()
    {
        currentPlayer = null;
        targetPlayer = null;
        ShowBattleSequenceMenu(false);
    }

    private void Start()
    {
        StartBattleSequence();
    }

    public void StartBattleSequence()
    {
        ShowBattleSequenceMenu(true);
        currentPlayer = darts.GetNextPlayer();
        currentPlayer.ActionEnd += HandleDartsActionEnd;
        Debug.Log("Start");
    }

    public void BattleAttack()
    {
        Debug.Log("Battle Attack");
        ShowBattleSequenceMenu(false);
        currentPlayer.PlayerAttack(targetPlayer);
    }

    private void ShowBattleSequenceMenu(bool show)
    {
        battleSequenceMenuButtons.ForEach(button => button.gameObject.SetActive(show));
    }

    private void HandleDartsActionEnd(object sender, EventArgs e)
    {
        currentPlayer.ActionEnd -= HandleDartsActionEnd;
        ShowBattleSequenceMenu(false);
        currentPlayer = enemies.GetNextPlayer();
        currentPlayer.ActionEnd += HandleEnemiesActionEnd;
        currentPlayer.PlayerAttack(darts.GetRandomPlayer());
        Debug.Log("Next Enemy");
    }

    private void HandleEnemiesActionEnd(object sender, EventArgs e)
    {
        //TODO create a way to cycle between enemies.
        targetPlayer = enemies.GetRandomPlayer();
        currentPlayer.ActionEnd -= HandleEnemiesActionEnd;
        ShowBattleSequenceMenu(true);
        currentPlayer = darts.GetNextPlayer();
        currentPlayer.ActionEnd += HandleDartsActionEnd;
        Debug.Log("Next Dart");
    }
}

