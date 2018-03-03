using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour 
{
    #region Player Status Inspector Elements

    [SerializeField]
    protected int health;

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

    #endregion

    #region Player Status Properties

    /// <summary>
    /// Gets or sets the health. Be able to set due to 
    /// health being used in Battle Sequences and Overworld.
    /// </summary>
    /// <value>The health.</value>
    public int Health
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

    public int Level
    {
        get
        {
            return level;
        }
    }

    public int Experience
    {
        get
        {
            return experience;
        }
        set
        {
            experience = value;
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
    }

    public int Defense
    {
        get
        {
            return defense;
        }
    }

    public int Speed
    {
        get
        {
            return speed;
        }
    }

    #endregion

    public virtual void Initialize(Player player)
    {
        this.health = player.Health;
        this.level = player.Level;
        this.experience = player.Experience;
        this.experienceCap = player.ExperienceCap;
        this.strength = player.Strength;
        this.defense = player.Defense;
        this.speed = player.Speed;
    }

    public virtual void Initialize(int hlth, int lvl, int exp, int expCap, int str, int def, int spd)
    {
        this.health = hlth;
        this.level = lvl;
        this.experience = exp;
        this.experienceCap = expCap;
        this.strength = str;
        this.defense = def;
        this.speed = spd;
    }
}
