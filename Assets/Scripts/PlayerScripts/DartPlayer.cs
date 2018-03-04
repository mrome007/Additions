using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartPlayer : Player
{
    #region Overrides

    public override int Health
    {
        get
        {
            return base.Health;
        }
        set
        {
            base.Health = value;
            PlayerStateUiController.Instance.UpdatePlayerHealth((float)health / healthCap);
        }
    }

    public override int Level
    {
        get
        {
            return base.Level;
        }
        protected set
        {
            base.Level = value;
            PlayerStateUiController.Instance.UpdatePlayerLevel(level);
        }
    }

    #endregion
    
    [SerializeField]
    private int expCapIncr = 50;

    [SerializeField]
    private BattlePlayerCreator.Darts dartType;

    public BattlePlayerCreator.Darts DartType { get { return dartType; } }

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
