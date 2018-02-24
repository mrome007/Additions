using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayer : Player
{
    [SerializeField]
    private BattlePlayerCreator.Enemies enemyType;

    public BattlePlayerCreator.Enemies EnemyType { get { return enemyType; } }
}
