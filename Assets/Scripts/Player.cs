using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour 
{
    #region Player Status Inspector Elements

    [SerializeField]
    private int health;

    [SerializeField]
    private int level = 1;

    [SerializeField]
    private int experience;

    [SerializeField]
    private int experienceCap;

    [SerializeField]
    private int strength;

    [SerializeField]
    private int defense;

    [SerializeField]
    private int speed;

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
}
