using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverWorld : MonoBehaviour 
{
    [SerializeField]
    private List<Player> playerTeam;

    public List<Player> PlayerTeam { get { return playerTeam; } }
    
    [SerializeField]
    protected float movementSpeed;

    protected Vector2 movementVector;

    protected virtual void Awake()
    {
        movementVector = Vector2.zero;
    }
}
