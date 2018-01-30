using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Player
{
    public override void PlayerAttack()
    {
        base.PlayerAttack();
        EndAction();
    }
}
