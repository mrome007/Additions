using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSequenceTransition : MonoBehaviour 
{
    #region Instance

    public static BattleSequenceTransition Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (BattleSequenceTransition)FindObjectOfType(typeof(BattleSequenceTransition));
            }

            return instance;
        }
    }

    private static BattleSequenceTransition instance = null;

    #endregion

    public DartPlayer MainPlayer
    {
        get
        {
            return mainPlayerContainer;
        }
    }

    [SerializeField]
    private BattlePlayerCreator battlePlayerCreator;
    [SerializeField]
    private GameObject overWorldElementsContainer;
    [SerializeField]
    private DartPlayer mainPlayerContainer;
    [SerializeField]
    private GameObject shadowContainer;
    private GameObject enemyContact;

    private void Awake()
    {
        if(instance == null)
        {
            instance = (BattleSequenceTransition)FindObjectOfType(typeof(BattleSequenceTransition));
        }
    }

    public void LoadBattleSequence(GameObject other)
    {
        enemyContact = other;
		LoadUnloadBattleSequence.BattleSequenceLoadComplete += HandleLoadBattleSequenceComplete;
		LoadUnloadBattleSequence.LoadBattleSequence();
    }

    public void UnloadBattleSequence(bool win)
    {
		LoadUnloadBattleSequence.BattleSequenceUnloadComplete += HandleUnloadBattleSequenceComplete;
        if(win)
        {
            Destroy(enemyContact);
            enemyContact = null;
        }
		LoadUnloadBattleSequence.UnloadBattleSequence(win);
    }

    private void ShowOverWorldElements(bool show)
    {
        overWorldElementsContainer.SetActive(show);
        mainPlayerContainer.gameObject.SetActive(show);
        shadowContainer.gameObject.SetActive(show);
        if(enemyContact != null)
        {
            enemyContact.SetActive(show);
        }
    }

    private void SpawnBattleSequencePlayers(ref List<BattlePlayer> goodGuys, ref List<BattlePlayer> badGuys)
    {
        var dartOverWorld = mainPlayerContainer.GetComponent<PlayerOverWorld>();
        var enemyOverWorld = enemyContact.GetComponent<PlayerOverWorld>();

        //make good guys.
        foreach(var member in dartOverWorld.PlayerTeam)
        {
            //local variable to assign stats in the future.
            var teamMate = battlePlayerCreator.CreateDartBattlePlayer(member.GetComponent<DartPlayer>().DartType);
            mainPlayerContainer.ApplyBoosts((DartBattlePlayer)teamMate);
            teamMate.PlayerStats.Initialize(member);
            goodGuys.Add(teamMate);
        }

        foreach(var member in enemyOverWorld.PlayerTeam)
        {
            var teamMate = battlePlayerCreator.CreateEnemyBattlePlayer(member.GetComponent<EnemyPlayer>().EnemyType);
            teamMate.PlayerStats.Initialize(member);
            badGuys.Add(teamMate);
        }
    }

    private void StartBattleSequence()
    {
        var goodGuys = new List<BattlePlayer>();
        var badGuys = new List<BattlePlayer>();

        SpawnBattleSequencePlayers(ref goodGuys, ref badGuys);
        BattleSequence.Instance.StartBattleSequence(goodGuys, badGuys);
    }

    public void UpdateAdditionsInformation(Dictionary<string, int> additions)
    {
        foreach(var addition in additions)
        {
            mainPlayerContainer.UpdateAdditionMileStoneCount(addition.Key, addition.Value);
        }
    }

    #region EventHandlers

    private void HandleLoadBattleSequenceComplete(object sender, EventArgs e)
    {
		LoadUnloadBattleSequence.BattleSequenceLoadComplete -= HandleLoadBattleSequenceComplete;
        ShowOverWorldElements(false);
        StartBattleSequence();
    }

    private void HandleUnloadBattleSequenceComplete(object sender, EventArgs e)
    {
		LoadUnloadBattleSequence.BattleSequenceUnloadComplete -= HandleUnloadBattleSequenceComplete;
        ShowOverWorldElements(true);
    }

    #endregion
}
