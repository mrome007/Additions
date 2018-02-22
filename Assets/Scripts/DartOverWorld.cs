using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartOverWorld : PlayerOverWorld 
{   
    [SerializeField]
    private DartOverWorldAnimation dartOverWorldAnimation;

    [SerializeField]
    private DartOverWorldAnimation shadowAnimation;
    
    private void Update()
    {
        DartMovement();
    }

    private void DartMovement()
    {
        var h = Input.GetAxis("Horizontal");
        dartOverWorldAnimation.DartWalk(h);
        shadowAnimation.DartWalk(h);
        movementVector.x = h;
        transform.Translate(movementSpeed * movementVector * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var npc = other.GetComponent<NPC>();
        if(npc != null)
        {
            npc.ShowText(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var npc = other.GetComponent<NPC>();
        if(npc != null)
        {
            npc.ShowText(false);
        }
    }
}
