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
        set
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
        set
        {
            experienceCap = value;
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

    #endregion

    public virtual void Initialize(Player player)
    {
        this.Health = player.Health;
        this.Level = player.Level;
        this.Experience = player.Experience;
        this.ExperienceCap = player.ExperienceCap;
        this.Strength = player.Strength;
        this.Defense = player.Defense;
        this.Speed = player.Speed;
    }

    public virtual void Initialize(int hlth, int lvl, int exp, int expCap, int str, int def, int spd)
    {
        this.Health = hlth;
        this.Level = lvl;
        this.Experience = exp;
        this.ExperienceCap = expCap;
        this.Strength = str;
        this.Defense = def;
        this.Speed = spd;
    }
}
