using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
    public override void PlayerAttack(Player target)
    {
        base.PlayerAttack(target);
        EndAction(currentAction, 0f);
    }
}
