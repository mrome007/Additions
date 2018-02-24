using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerCreator : MonoBehaviour 
{
    #region Instance

    public static BattlePlayerCreator Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (BattlePlayerCreator)FindObjectOfType(typeof(BattlePlayerCreator));
            }
            return instance;
        }
    }

    private static BattlePlayerCreator instance;

    #endregion

    #region Inspector elements

    [SerializeField]
    private List<BattlePlayer> dartBattlePlayers;

    [SerializeField]
    private List<BattlePlayer> enemyBattlePlayers;

    #endregion

    #region BattlePlayers enums

    public enum Darts
    {
        Dart = 0,
        Shana = 1
    }

    public enum Enemies
    {
        Slime = 0,
        Grunt = 1
    }

    #endregion

    public BattlePlayer CreateDartBattlePlayer(Darts dartType)
    {
        var newDart = Instantiate(dartBattlePlayers[(int)dartType], Vector2.zero, Quaternion.identity);
        return newDart;
    }

    public BattlePlayer CreateEnemyBattlePlayer(Enemies enemyType)
    {
        var newEnemy = Instantiate(enemyBattlePlayers[(int)enemyType], Vector2.zero, Quaternion.identity);
        return newEnemy;
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = (BattlePlayerCreator)FindObjectOfType(typeof(BattlePlayerCreator));
        }
    }
}
