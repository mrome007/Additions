using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverWorldMovement : MonoBehaviour 
{
    [SerializeField]
    protected float movementSpeed;

    protected Vector2 movementVector;

    protected virtual void Awake()
    {
        movementVector = Vector2.zero;
    }
}
