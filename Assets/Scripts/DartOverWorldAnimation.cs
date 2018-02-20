using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartOverWorldAnimation : MonoBehaviour 
{
    [SerializeField]
    private SpriteRenderer dartSpriteRenderer;

    [SerializeField]
    private Animator dartAnimator;

    [SerializeField]
    private string walkParameter;

    private float sign = 1f;

    public void DartWalk(float speed)
    {
        if(speed != 0f && Mathf.Sign(speed) != sign)
        {
            sign = Mathf.Sign(speed);
            dartSpriteRenderer.flipX = sign < 0f;
        }

        dartAnimator.SetFloat(walkParameter, speed != 0f ? 0.1f : 0f);
    }
}
