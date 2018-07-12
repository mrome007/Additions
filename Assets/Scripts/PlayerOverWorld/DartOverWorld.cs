using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartOverWorld : PlayerOverWorld 
{   
    [SerializeField]
    private DartOverWorldAnimation dartOverWorldAnimation;

    //TODO temporary, have shadow animations take care of itself.
    [SerializeField]
    private DartOverWorldAnimation shadowAnimation;

    [SerializeField]
    private Rigidbody2D dartRigidBody;

    [SerializeField]
    private Collider2D dartCollider;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform groundCheckRight;

    [SerializeField]
    private Transform groundCheckLeft;

    [SerializeField]
    private float groundCheckRadius;

    [SerializeField]
    private LayerMask groundLayer;

    private bool grounded = false;
    private Collider2D []results;
    private Vector2 jumpForce;
    private bool stopMovement = false;
    private bool jumped = false;
    private WaitForSeconds jumpDelayTime;

    public bool Grounded { get { return grounded; } }

    protected override void Awake()
    {
        base.Awake();
        jumpForce = new Vector2(0f, 600f);
        results = new Collider2D[1];
        jumpDelayTime = new WaitForSeconds(0.1f);
        StoryDialoguePresentation.Instance.DialogueEnded += HandleStoryDialogueEnded;
    }

    private void Update()
    {
        if(stopMovement)
        {
            return;
        }
            
        if(grounded && Input.GetKeyDown(KeyCode.Space))
        {
            //TODO Keep an eye on calls to this.
            StartCoroutine(DelayJump());
            dartRigidBody.AddForce(jumpForce);
        }
    }

    private void FixedUpdate()
    {
        if(stopMovement)
        {
            return;
        }
        
        DartMovement();

        if(!jumped)
        {
            dartRigidBody.isKinematic = dartRigidBody.velocity.sqrMagnitude < 0.5f && grounded;
            dartRigidBody.simulated = !dartRigidBody.isKinematic;
            dartCollider.isTrigger = dartRigidBody.isKinematic;
        }
    }

    private void DartMovement()
    {
        grounded = Physics2D.OverlapCircleNonAlloc(groundCheck.position, groundCheckRadius, results, (int)groundLayer) > 0 ||
                   Physics2D.OverlapCircleNonAlloc(groundCheckLeft.position, groundCheckRadius, results, (int)groundLayer) > 0 ||
                   Physics2D.OverlapCircleNonAlloc(groundCheckRight.position, groundCheckRadius, results, (int)groundLayer) > 0;

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

        var storyChar = other.GetComponent<StoryCharacter>();
        if(storyChar != null)
        {
            if(!storyChar.Activated)
            {
                stopMovement = true;
                storyChar.ShowDialogue();
            }
        }

        var enemy = other.GetComponent<EnemyPlayer>();
        if(enemy != null)
        {
            BattleSequenceTransition.Instance.LoadBattleSequence(other.gameObject);
        }
    }

    private void HandleStoryDialogueEnded(object sender, EventArgs e)
    {
        stopMovement = false;
    }

    private IEnumerator DelayJump()
    {
        jumped = true;
        dartRigidBody.isKinematic = false;
        dartRigidBody.simulated = true;
        yield return jumpDelayTime;

        jumped = false;
    }
}
