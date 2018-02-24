using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequenceIndicator : MonoBehaviour 
{
    [SerializeField]
    private GameObject enemyIndicatorObject;

    public void MoveBattleSequenceIndicator(Vector3 pos)
    {
        transform.position = pos;
    }

    public void ShowBattleSequenceIndicator(bool show)
    {
        gameObject.SetActive(show);
    }
}
