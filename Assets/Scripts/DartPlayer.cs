using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DartPlayer : Player
{
    [SerializeField]
    private List<Additions> additions;
}

[Serializable]
public class Additions
{
    public string Name;
    public List<Addition> Addition;
}

[Serializable]
public struct Addition
{
    [SerializeField]
    private float damage;

    public Addition(float dam)
    {
        damage = dam;
    }

    public float ApplyDamage(bool success)
    {
        damage = success ? damage : damage / 2f;
        return damage;
    }
}
