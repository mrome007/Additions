using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DartPlayer : Player
{
    [SerializeField]
    private List<Additions> additions;

    private bool ready = false;

    private void Update()
    {
        if(!ready)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ready = false;
            EndAction();
        }
    }

    public override void PlayerAttack(Player target)
    {
        ready = true;
        base.PlayerAttack(target);
    }
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
