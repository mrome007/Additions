using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCreature : MonoBehaviour 
{
    [SerializeField]
    private SpriteRenderer lightSprite;

    private const float lightColor = 255f;
    private const float darkColor = 25f;

    private Color currentColor;

    public void FadeLight(float incr)
    {
        var colorMultiplier = lightSprite.color.r;
        var currentColorVal = colorMultiplier * lightColor;
        currentColor = lightSprite.color;
        currentColorVal -= incr * 70f * Time.deltaTime;
        if(currentColorVal <= darkColor)
        {
            currentColorVal = darkColor;
            //For now set object off. In the future return it to an object pool.
            gameObject.SetActive(false);
        }

        colorMultiplier = currentColorVal / lightColor;
        currentColor.r = colorMultiplier;
        currentColor.g = colorMultiplier;
        currentColor.b = colorMultiplier;

        lightSprite.color = currentColor;
    }
}