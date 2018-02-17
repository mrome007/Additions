using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DartBattleSequenceTransition : MonoBehaviour 
{
    #region Instance

    public static DartBattleSequenceTransition Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (DartBattleSequenceTransition)FindObjectOfType(typeof(DartBattleSequenceTransition));
            }

            return instance;
        }
    }

    private static DartBattleSequenceTransition instance = null;

    #endregion

    public event EventHandler BattleSequenceLoadComplete;
    public event EventHandler BattleSequenceUnloadComplete;

    [SerializeField]
    private GameObject OverWorldElementsContainer;
    [SerializeField]
    private GameObject mainPlayerContainer;
    private GameObject enemyContact;

    private void Awake()
    {
        if(instance == null)
        {
            instance = (DartBattleSequenceTransition)FindObjectOfType(typeof(DartBattleSequenceTransition));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<EnemyPlayer>();
        if(enemy != null)
        {
            enemyContact = other.gameObject;
            other.enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;

            LoadBattleSequence();
        }
    }

    public void LoadBattleSequence()
    {
        BattleSequenceLoadComplete += HandleLoadBattleSequenceComplete;
        StartCoroutine(LoadBattleSequenceAsyncCoroutine());
    }

    public void UnloadBattleSequence()
    {
        BattleSequenceUnloadComplete += HandleUnloadBattleSequenceComplete;
        StartCoroutine(UnloadBattleSequenceAsyncCoroutine());
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

    private IEnumerator UnloadBattleSequenceAsyncCoroutine()
    {
        var asyncLoad = SceneManager.UnloadSceneAsync("BattleSequence");

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        var handler = BattleSequenceUnloadComplete;
        if(handler != null)
        {
            handler(this, null);
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
        var dartOverWorld = gameObject.GetComponent<PlayerOverWorld>();
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
