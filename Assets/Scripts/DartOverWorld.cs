using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartOverWorld : PlayerOverWorld 
{   
    [SerializeField]
    private DartOverWorldAnimation dartOverWorldAnimation;
    
    private void Update()
    {
        DartMovement();
    }

    private void DartMovement()
    {
        var h = Input.GetAxis("Horizontal");
        dartOverWorldAnimation.DartWalk(h);
        movementVector.x = h;
        transform.Translate(movementSpeed * movementVector * Time.deltaTime);
    }
}
