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
            }
        }
        else
        {
            CurrentState = LightCreatureState.Oblivious;
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
            yield return StartCoroutine(CheckForShadowRoutine());
            lightSprite.flipX = !lightSprite.flipX;
            direction = lightSprite.flipX ? Vector2.right : Vector2.left;
        }
    }
}

public enum LightCreatureState
{
    Oblivious,
    Alert
}