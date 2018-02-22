using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour 
{
    [SerializeField]
    private Transform dartTransform;

    private Vector3 movementVector;

    private void Start()
    {
        movementVector.x = transform.position.x;
        movementVector.y = transform.position.y;
        movementVector.z = -10f;
    }

    private void Update()
    {
        movementVector.x = dartTransform.position.x;
        transform.position = movementVector;
    }
}
