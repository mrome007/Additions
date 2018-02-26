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
    private ShadowMovement shadowMovement;

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

    private bool shadowLeftInputStart = false;
    private float shadowLeftInputTimer = 0f;
    private bool shadowRightInputStart = false;
    private float shadowRightInputTimer = 0f;
    private float shadowTimerCap = 0.35f;
    private bool stopMovement = false;

    protected override void Awake()
    {
        base.Awake();
        jumpForce = new Vector2(0f, 600f);
        results = new Collider2D[1];
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
            dartRigidBody.AddForce(jumpForce);
        }
         
        GetShadowInput();
    }

    private void FixedUpdate()
    {
        if(stopMovement)
        {
            return;
        }
        
        DartMovement();

        dartRigidBody.isKinematic = dartRigidBody.velocity.sqrMagnitude < 0.5f && grounded;
        dartRigidBody.simulated = !dartRigidBody.isKinematic;
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

    private void OnTriggerExit2D(Collider2D other)
    {
        var npc = other.GetComponent<NPC>();
        if(npc != null)
        {
            npc.ShowText(false);
        }
    }

    private void GetShadowInput()
    {
        if(shadowMovement.Moving)
        {
            shadowLeftInputStart = false;
            shadowLeftInputTimer = 0f;
            shadowRightInputStart = false;
            shadowRightInputTimer = 0f;
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(shadowRightInputStart && shadowRightInputTimer <= shadowTimerCap)
            {
                shadowMovement.MoveShadow(Vector2.right);
            }
            shadowRightInputStart = true;
        }

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(shadowLeftInputStart && shadowLeftInputTimer <= shadowTimerCap)
            {
                shadowMovement.MoveShadow(Vector2.left);
            }
            shadowLeftInputStart = true;
        }

        if(shadowRightInputStart)
        {
            shadowRightInputTimer += Time.deltaTime;
            if(shadowRightInputTimer > shadowTimerCap)
            {
                shadowRightInputStart = false;
                shadowRightInputTimer = 0;
            }
        }

        if(shadowLeftInputStart)
        {
            shadowLeftInputTimer += Time.deltaTime;
            if(shadowLeftInputTimer > shadowTimerCap)
            {
                shadowLeftInputStart = false;
                shadowLeftInputTimer = 0;
            }
        }

    }

    private void HandleStoryDialogueEnded(object sender, EventArgs e)
    {
        stopMovement = false;
    }
}
