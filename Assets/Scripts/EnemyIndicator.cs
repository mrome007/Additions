using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour 
{
    [SerializeField]
    private GameObject enemyIndicatorObject;

    public void MoveEnemyIndicator(Vector3 pos)
    {
        transform.position = pos;
    }

    public void ShowEnemyIndicator(bool show)
    {
        gameObject.SetActive(show);
    }
}
