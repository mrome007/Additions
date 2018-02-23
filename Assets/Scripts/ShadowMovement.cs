using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMovement : MonoBehaviour 
{
    [SerializeField]
    private Transform dartTransform;

    [SerializeField]
    private float maxDistance;

    [SerializeField]
    private float shadowSpeed;

    private Coroutine followCoroutine = null;

    public bool Moving { get; set; }

    private void Start()
    {
        FollowDart();
    }

    public void MoveShadow(Vector2 direction)
    {
        StopFollowing();
        StartCoroutine(MoveShadowCoroutine(direction));
    }

    private IEnumerator MoveShadowCoroutine(Vector2 direction)
    {
        Moving = true;
        var distance = 0f;
        while(distance < maxDistance)
        {
            distance += shadowSpeed * Time.deltaTime;
            transform.Translate(direction * shadowSpeed * Time.deltaTime);
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
        while(true)
        {
            transform.position = dartTransform.position;
            yield return null;
        }
    }
}
