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

    private void Awake()
    {
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
        currentPlayer.ActionEnd += HandleActionEnd;
        Debug.Log("Start");
    }

    public void BattleAttack()
    {
        Debug.Log("Battle Attack");
        ShowBattleSequenceMenu(false);
        currentPlayer.PlayerAttack();
    }

    private void ShowBattleSequenceMenu(bool show)
    {
        battleSequenceMenuButtons.ForEach(button => button.gameObject.SetActive(show));
    }

    private void HandleActionEnd(object sender, EventArgs e)
    {
        currentPlayer.ActionEnd -= HandleActionEnd;
        ShowBattleSequenceMenu(true);
        currentPlayer = darts.GetNextPlayer();
        currentPlayer.ActionEnd += HandleActionEnd;
        Debug.Log("Next");
    }
}

