using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DartOverWorldMovement : PlayerOverWorldMovement 
{
    private Collider2D playerCollider;

    protected override void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
    }
    
    private void Update()
    {
        DartMovement();
    }

    private void DartMovement()
    {
        var h = Input.GetAxis("Horizontal");
        movementVector.x = h;
        transform.Translate(movementSpeed * movementVector * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<EnemyPlayer>();
        if(enemy != null)
        {
            playerCollider.enabled = false;
            other.enabled = false;
            enabled = false;
            StartCoroutine(LoadBattleSequenceScene());
        }
    }

    IEnumerator LoadBattleSequenceScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("BattleSequence", LoadSceneMode.Additive);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }

        //TEMPORARY FOR POPULATING WHAT PLAYERS ARE PRESENT IN BATTLE SEQUENCE.
        var darts = new List<BattlePlayerCreator.Darts>();
        darts.Add(BattlePlayerCreator.Darts.Dart);

        var enemies = new List<BattlePlayerCreator.Enemies>();
        for(int index = 0; index < 3; index++)
        {
            enemies.Add(BattlePlayerCreator.Enemies.Slime);
        }

        BattleSequence.Instance.PopulateParties(darts, enemies);
        BattleSequence.Instance.StartBattleSequence();
    }
}
