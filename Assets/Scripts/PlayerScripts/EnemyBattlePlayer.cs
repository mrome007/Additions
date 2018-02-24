using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattlePlayer : BattlePlayer
{
    public override void PlayerAttack(BattlePlayer target)
    {
        base.PlayerAttack(target);
        StartCoroutine(WaitForEnemyAttack(target));
    }

    private IEnumerator WaitForEnemyAttack(BattlePlayer target)
    {
        Debug.Log(gameObject.name + " attacks");
        yield return new WaitForSeconds(3f);
        EndAction(currentAction, 1, target);
    }
}
