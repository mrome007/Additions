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

    [SerializeField]
    private Collider2D shadowCollider;

    private Coroutine followCoroutine = null;

    public bool Moving { get; set; }

    private void Start()
    {
        FollowDart();
    }

    private void OnDisable()
    {
        StopFollowing();
    }

    private void OnEnable()
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
        shadowCollider.enabled = true;

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
        shadowCollider.enabled = false;
        while(true)
        {
            transform.position = dartTransform.position;
            yield return null;
        }
    }
}
