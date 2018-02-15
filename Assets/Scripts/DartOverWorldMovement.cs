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

        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.UnloadSceneAsync("BattleSequence");
        }
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
            SceneManager.LoadScene("BattleSequence", LoadSceneMode.Additive);
        }
    }
}
