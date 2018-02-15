using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartPlayer : Player
{
    [SerializeField]
    private int expCapIncr = 50;

    public void IncrementExperiencePoints(int exp)
    {
        experience += exp;
        if(experience >= experienceCap)
        {
            level++;
            experience %= experienceCap;
            experienceCap += (expCapIncr * level);
        }
    }
}
