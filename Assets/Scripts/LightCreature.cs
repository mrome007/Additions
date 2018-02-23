using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCreature : MonoBehaviour 
{
    [SerializeField]
    private SpriteRenderer lightSprite;

    [SerializeField]
    private float alertTime;

    [SerializeField]
    private LayerMask darkness;
    
    public LightCreatureState CurrentState { get; private set; }

    private Vector2 direction;


    private void Awake()
    {
        CurrentState = LightCreatureState.Oblivious;
        direction = lightSprite.flipX ? Vector2.right : Vector2.left;
    }

    private void Start()
    {
        StartCoroutine(KeepChecking());
    }

    private void CheckForShadow()
    {
        var hit = Physics2D.Raycast(transform.position, direction, 4f, (int)darkness);
        if(hit.collider != null)
        {
            var shadow = hit.collider.GetComponent<ShadowIntimidate>();
            if(shadow != null)
            {
                CurrentState = LightCreatureState.Alert;
                //temporary just to show me.
                lightSprite.color = Color.red;
            }
        }
        else
        {
            CurrentState = LightCreatureState.Oblivious;
            lightSprite.color = Color.white;
        }
    }

    private IEnumerator CheckForShadowRoutine()
    {
        var timer = 0f;
        while(timer < 5f)
        {
            CheckForShadow();
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator KeepChecking()
    {
        while(true)
        {
            lightSprite.flipX = !lightSprite.flipX;
            direction = lightSprite.flipX ? Vector2.right : Vector2.left;
            yield return StartCoroutine(CheckForShadowRoutine());
        }
    }
}

public enum LightCreatureState
{
    Oblivious,
    Alert
}