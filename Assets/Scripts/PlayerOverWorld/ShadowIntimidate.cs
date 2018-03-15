using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowIntimidate : MonoBehaviour 
{
    [SerializeField]
    private DartPlayer player;
    private Vector3 scaleVector;
    private float scaleIncrement;

    private void Awake()
    {
        scaleVector = transform.localScale;
        scaleIncrement = 1f / player.ShadowCap;
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
                if(player.Shadow < player.ShadowCap)
                {
                    player.Shadow++;
                    scaleVector.x += scaleIncrement;
                    scaleVector.y += scaleIncrement;
                    transform.localScale = scaleVector;
                }
            }
            else
            {
                if(player.Shadow > 0)
                {
                    player.Shadow--;
                    scaleVector.x -= scaleIncrement;
                    scaleVector.y -= scaleIncrement;
                    transform.localScale = scaleVector;
                }
            }
        }
    }
}
