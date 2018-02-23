using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowIntimidate : MonoBehaviour 
{
    //TODO do this for now so I can see it on the inspector. In the future create a meter GUI for it.
    [SerializeField]
    private int intimidatePoints;

    private int intimidatePointsCap = 100;

    private Vector3 scaleVector;
    private float scaleIncrement = 0.015f;

    private void Awake()
    {
        scaleVector = transform.localScale;
        intimidatePoints = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var lightCreature = other.GetComponent<LightCreature>();
        if(lightCreature != null)
        {
            //TODO temporary. Will probably make a pool of these in the future.
            if(lightCreature.CurrentState == LightCreatureState.Oblivious)
            {
                Destroy(other.gameObject);
                intimidatePoints++;
                if(intimidatePoints < intimidatePointsCap)
                {
                    scaleVector.x += scaleIncrement;
                    scaleVector.y += scaleIncrement;
                    transform.localScale = scaleVector;
                }
                intimidatePoints %= intimidatePointsCap;
            }
            else
            {
                if(intimidatePoints > 0)
                {
                    intimidatePoints--;
                    scaleVector.x -= scaleIncrement;
                    scaleVector.y -= scaleIncrement;
                    transform.localScale = scaleVector;
                }
            }
        }
    }
}
