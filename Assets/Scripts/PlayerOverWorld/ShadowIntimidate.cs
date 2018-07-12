using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowIntimidate : MonoBehaviour 
{
    [SerializeField]
    private DartPlayer player;

    [SerializeField]
    private float intimidateIncrement;

    private Vector3 scaleVector;
    private float scaleIncrement;

    private void Awake()
    {
        scaleVector = transform.localScale;
        scaleIncrement = 2f / player.ShadowCap;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var lightCreature = other.GetComponent<LightCreature>();
        if(lightCreature != null)
        {
            lightCreature.FadeLight(intimidateIncrement);
            if(player.Shadow < player.ShadowCap)
            {
                player.Shadow++;
                scaleVector.x += scaleIncrement;
                scaleVector.y += scaleIncrement;
                transform.localScale = scaleVector;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var lightCreature = other.GetComponent<LightCreature>();
        if(lightCreature != null)
        {
            lightCreature.FadeLight(intimidateIncrement);

            if(player.Shadow < player.ShadowCap)
            {
                player.Shadow++;
                scaleVector.x += scaleIncrement;
                scaleVector.y += scaleIncrement;
                transform.localScale = scaleVector;
            }
        }
    }
}
