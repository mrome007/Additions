﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour 
{
    #region Player Status Inspector Elements

    [SerializeField]
    protected int health;

    protected int healthCap;

    [SerializeField]
    protected int level = 1;

    [SerializeField]
    protected int experience;

    [SerializeField]
    protected int experienceCap;

    [SerializeField]
    protected int strength;

    [SerializeField]
    protected int defense;

    [SerializeField]
    protected int speed;

    [SerializeField]
    protected int shadow;

    protected int shadowCap = 20;

    #endregion

    #region Player Status Properties

    public int HealthCap
    {
        get
        {
            return healthCap;
        }
        private set
        {
            healthCap = value;
        }
    }

    /// <summary>
    /// Gets or sets the health. Be able to set due to 
    /// health being used in Battle Sequences and Overworld.
    /// </summary>
    /// <value>The health.</value>
    public virtual int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public virtual int Level
    {
        get
        {
            return level;
        }
        protected set
        {
            level = value;
        }
    }

    public int Experience
    {
        get
        {
            return experience;
        }
    }

    public int ExperienceCap
    {
        get
        {
            return experienceCap;
        }
    }

    public int Strength
    {
        get
        {
            return strength;
        }
        set
        {
            strength = value;
        }
    }

    public int Defense
    {
        get
        {
            return defense;
        }
        set
        {
            defense = value;
        }
    }

    public int Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    public virtual int Shadow
    {
        get
        {
            return shadow;
        }
        set
        {
            shadow = value;
        }
    }

    public virtual int ShadowCap
    {
        get
        {
            return shadowCap;
        }
    }

    #endregion

    #region base values

    protected int baseHealth;
    protected int baseStrength;
    protected int baseDefense;
    protected int baseSpeed;

    #endregion

    protected virtual void Awake()
    {
        healthCap = health;
    }

    protected virtual void Start()
    {
        baseHealth = health;
        baseStrength = strength;
        baseDefense = defense;
        baseSpeed = speed;
    }

    public virtual void Initialize(Player player)
    {
        this.health = player.Health;
        healthCap = health;
        this.level = player.Level;
        this.experience = player.Experience;
        this.experienceCap = player.ExperienceCap;
        this.strength = player.Strength;
        this.defense = player.Defense;
        this.speed = player.Speed;
        this.shadow = player.Shadow;
    }

    public virtual void Initialize(int hlth, int lvl, int exp, int expCap, int str, int def, int spd, int sdw)
    {
        this.health = hlth;
        healthCap = health;
        this.level = lvl;
        this.experience = exp;
        this.experienceCap = expCap;
        this.strength = str;
        this.defense = def;
        this.speed = spd;
        this.shadow = sdw;
    }
}
