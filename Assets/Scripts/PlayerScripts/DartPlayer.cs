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
    private BattlePlayerCreator.Darts dartType;

    [SerializeField]
    private AdditionMilestones additionMileStones;

    public BattlePlayerCreator.Darts DartType { get { return dartType; } }

    public void IncrementExperiencePoints(int exp)
    {
        experience += exp;
        if(experience >= experienceCap)
        {
            LevelUp();
        }
    }

    public void ApplyBoosts(DartBattlePlayer player)
    {
        additionMileStones.ApplyMilestoneBoosts(player);
    }

    public void UpdateAdditionMileStoneCount(string name, int count)
    {
        additionMileStones.UpdateMilestoneCount(name, count);
    }

    private int GetNewLevelUpCap(int lvl)
    {
        return lvl * lvl * lvl;
    }

    private void LevelUp()
    {
        var levelIncrement = experience / experienceCap;
        Level += levelIncrement;
        experience %= experienceCap;
        experienceCap += GetNewLevelUpCap(level);

        healthCap = LevelUpAttributes(level, baseHealth, healthCap, 0);
        strength = LevelUpAttributes(level, baseStrength, strength, Random.Range(2,4));
        defense = LevelUpAttributes(level, baseDefense, defense, 0);
        speed = LevelUpAttributes(level, baseSpeed, speed, Random.Range(1, 4));
    }

    private int LevelUpAttributes(int lvl, int baseValue, int currentValue, int modifier)
    {
        return (baseValue + currentValue / 17) + (lvl * baseValue / 11) + modifier;
    }
}
