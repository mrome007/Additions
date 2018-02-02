using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
    public override void PlayerAttack(Player target)
    {
        base.PlayerAttack(target);
        StartCoroutine(WaitForEnemyAttack());
    }

    private IEnumerator WaitForEnemyAttack()
    {
        Debug.Log(gameObject.name + " attacks");
        yield return new WaitForSeconds(3f);
        EndAction(currentAction, 0f);
    }
}
