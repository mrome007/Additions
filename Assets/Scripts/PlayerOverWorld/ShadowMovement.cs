using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMovement : MonoBehaviour 
{
    [SerializeField]
    private Transform dartTransform;

    [SerializeField]
    private float shadowMovementTime;

    [SerializeField]
    private float shadowSpeed;

    [SerializeField]
    private DartOverWorldAnimation shadowAnimation;

    [SerializeField]
    private Collider2D shadowCollider;

    private Coroutine followCoroutine = null;

    public bool Moving { get; set; }

    private Vector3 movementVector;

    private void OnDisable()
    {
        StopFollowing();
    }

    private void OnEnable()
    {
        FollowDart();
        movementVector = Vector3.zero;
    }

    public void MoveShadow()
    {
        StopFollowing();
        StartCoroutine(MoveShadowCoroutine());
    }

    private IEnumerator MoveShadowCoroutine()
    {
        Moving = true;
        shadowCollider.enabled = true;
        var timer = 0f;

        while(timer < shadowMovementTime)
        {
            var shadowH = Input.GetAxis("ShadowHorizontal");
            shadowAnimation.DartWalk(shadowH);
            movementVector.x = shadowH;
            transform.Translate(movementVector * shadowSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        FollowDart();
    }

    private void FollowDart()
    {
        if(followCoroutine == null)
        {
            followCoroutine = StartCoroutine(FollowDartCoroutine());
        }
    }

    private void StopFollowing()
    {
        if(followCoroutine != null)
        {
            StopCoroutine(followCoroutine);
        }
        followCoroutine = null;
    }

    private IEnumerator FollowDartCoroutine()
    {
        Moving = false;
        shadowCollider.enabled = false;
        while(true)
        {
            transform.position = dartTransform.position;
            var shadowH = Input.GetAxis("ShadowHorizontal");
            if(shadowH != 0)
            {
                break;
            }
            yield return null;
        }

        MoveShadow();
    }
}
