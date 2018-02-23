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

    public event EventHandler<BattleWonArgs> BattleSequenceLoadComplete;
    public event EventHandler<BattleWonArgs> BattleSequenceUnloadComplete;

    [SerializeField]
    private GameObject OverWorldElementsContainer;
    [SerializeField]
    private GameObject mainPlayerContainer;
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
        BattleSequenceLoadComplete += HandleLoadBattleSequenceComplete;
        StartCoroutine(LoadBattleSequenceAsyncCoroutine());
    }

    public void UnloadBattleSequence(bool win)
    {
        BattleSequenceUnloadComplete += HandleUnloadBattleSequenceComplete;
        if(win)
        {
            Destroy(enemyContact);
            enemyContact = null;
        }
        StartCoroutine(UnloadBattleSequenceAsyncCoroutine(win));
    }

    private IEnumerator LoadBattleSequenceAsyncCoroutine()
    {
        var asyncLoad = SceneManager.LoadSceneAsync("BattleSequence", LoadSceneMode.Additive);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        var handler = BattleSequenceLoadComplete;
        if(handler != null)
        {
            handler(this, null);
        }
    }

    private IEnumerator UnloadBattleSequenceAsyncCoroutine(bool win)
    {
        var asyncLoad = SceneManager.UnloadSceneAsync("BattleSequence");

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        var handler = BattleSequenceUnloadComplete;
        if(handler != null)
        {
            handler(this, new BattleWonArgs(win));
        }
    }

    private void ShowOverWorldElements(bool show)
    {
        OverWorldElementsContainer.SetActive(show);
        mainPlayerContainer.SetActive(show);
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
            var teamMate = BattlePlayerCreator.Instance.CreateDartBattlePlayer(member.GetComponent<DartPlayer>().DartType);
            goodGuys.Add(teamMate);
        }

        foreach(var member in enemyOverWorld.PlayerTeam)
        {
            var teamMate = BattlePlayerCreator.Instance.CreateEnemyBattlePlayer(member.GetComponent<EnemyPlayer>().EnemyType);
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

    #region EventHandlers

    private void HandleLoadBattleSequenceComplete(object sender, EventArgs e)
    {
        BattleSequenceLoadComplete -= HandleLoadBattleSequenceComplete;
        ShowOverWorldElements(false);
        StartBattleSequence();
    }

    private void HandleUnloadBattleSequenceComplete(object sender, EventArgs e)
    {
        BattleSequenceUnloadComplete -= HandleUnloadBattleSequenceComplete;
        ShowOverWorldElements(true);
    }

    #endregion
}
