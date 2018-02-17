using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartOverWorld : PlayerOverWorld 
{   
    private void Update()
    {
        DartMovement();
    }

    private void DartMovement()
    {
        var h = Input.GetAxis("Horizontal");
        movementVector.x = h;
        transform.Translate(movementSpeed * movementVector * Time.deltaTime);
    }
}
