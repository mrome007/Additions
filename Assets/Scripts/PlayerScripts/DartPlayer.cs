﻿using System.Collections;
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

    public override int Shadow
    {
        get
        {
            return base.Shadow;
        }
        set
        {
            base.Shadow = value;
            PlayerStateUiController.Instance.UpdateShadowMeter((float)shadow / shadowCap);
        }
    }

    #endregion

    [SerializeField]
    private BattlePlayerCreator.Darts dartType;

    [SerializeField]
    private AdditionMilestones additionMileStones;

    public BattlePlayerCreator.Darts DartType { get { return dartType; } }

    public override void Initialize(Player player)
    {
        this.health = player.Health;
        this.level = player.Level;
        this.experience = player.Experience;
        this.experienceCap = player.ExperienceCap;
        this.strength = player.Strength;
        this.defense = player.Defense;
        this.speed = player.Speed;
        this.shadow = player.Shadow;
    }

    public void IncrementExperiencePoints(int exp)
    {
        experience += exp;
        if(experience >= experienceCap)
        {
            LevelUp();
            additionMileStones.UpdateEnabledAdditions(level);
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
