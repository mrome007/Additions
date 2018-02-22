using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartOverWorld : PlayerOverWorld 
{   
    [SerializeField]
    private DartOverWorldAnimation dartOverWorldAnimation;

    [SerializeField]
    private DartOverWorldAnimation shadowAnimation;

    [SerializeField]
    private Rigidbody2D dartRigidBody;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private LayerMask groundLayer;

    private bool grounded = false;

    private Collider2D []results;

    private Vector2 jumpForce;

    protected override void Awake()
    {
        base.Awake();
        jumpForce = new Vector2(0f, 600f);
        results = new Collider2D[1];
    }

    private void Update()
    {
        if(grounded && Input.GetKeyDown(KeyCode.Space))
        {
            dartRigidBody.AddForce(jumpForce);
        }
    }

    private void FixedUpdate()
    {
        DartMovement();
    }

    private void DartMovement()
    {
        grounded = Physics2D.OverlapCircleNonAlloc(groundCheck.position, groundCheckRadius, results, (int)groundLayer) > 0;
        
        var h = Input.GetAxis("Horizontal");
        dartOverWorldAnimation.DartWalk(h);
        shadowAnimation.DartWalk(h);
        movementVector.x = h * movementSpeed;
        movementVector.y = dartRigidBody.velocity.y;

        dartRigidBody.velocity = movementVector;
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
