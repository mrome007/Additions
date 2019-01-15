using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DartBattlePlayer : BattlePlayer
{   
    public Additions CurrentAddition { get { return currentAdditions; }  }

    [SerializeField]
    private List<Additions> additions;

    [SerializeField]
    private PerformAdditions performAdditions;

    private Additions currentAdditions;

    private void Start()
    {
        currentAdditions = additions[0];
    }
    
    public override void PlayerAttack(BattlePlayer target)
    {
        base.PlayerAttack(target);
        performAdditions.AdditionComplete += HandlePerformAdditionsComplete;
        performAdditions.StartPerformAddition(target, currentAdditions, PlayerStats.Strength);
    }

    private void HandlePerformAdditionsComplete(object sender, ActionEventArgs e)
    {
        performAdditions.AdditionComplete -= HandlePerformAdditionsComplete;
        EndAction(e);
    }

    public override void PlayerDefend(BattlePlayer target)
    {
        base.PlayerDefend(target);
        ExecuteDefense(target);
    }

    public override void PlayerHeal(BattlePlayer target)
    {
        base.PlayerHeal(target);
        ExecuteHeal(target);
    }

    public void ChangeAddition(int index)
    {
        if(index >= 0 & index < additions.Count)
        {
            currentAdditions = additions[index];
        }
    }

    private void ExecuteHeal(BattlePlayer target)
    {
        var hp = UnityEngine.Random.Range(1, target.PlayerStats.HealthCap / 4);
        EndAction(currentAction, hp, target);
    }

    private void ExecuteDefense(BattlePlayer target)
    {
        EndAction(currentAction, 1, target);
    }

    //For now pick a random addition to apply boost to.
    public void BoostAdditions(string additionName, float boostValue)
    {
        //List works for now since the additions list are small.
        var additionSelected = additions.Find(add => add.Name == additionName);

        for(int index = 0; index < additionSelected.Addition.Count; index++)
        {
            var addition = additionSelected.Addition[index];
            addition.BoostDamage(boostValue);
        }
    }

    public void EnableAdditions(string additionName, bool enable)
    {
        //List works for now since the additions list are small.
        var additionSelected = additions.Find(add => add.Name == additionName);
        additionSelected.Enabled = enable;
    }

    public Dictionary<string, int> GetAdditionsCount()
    {
        var additionCountContainer = new Dictionary<string, int>();
        for(int index = 0; index < additions.Count; index++)
        {
            var addition = additions[index];
            if(!additionCountContainer.ContainsKey(addition.Name))
            {
                additionCountContainer.Add(addition.Name, addition.Count);
            }
        }

        return additionCountContainer;
    }

    public List<string> GetEnabledAdditions()
    {
        return additions.Where(addi => addi.Enabled).Select(add => add.Name).ToList();
    }
}

[Serializable]
public class Additions
{
    public string Name;
    public bool Enabled;
    public List<Addition> Addition;
    public string FinalAttackTrigger;
    public int Count = 0;
}

[Serializable]
public class Addition
{
    public string AttackTrigger;
    public int Damage;
    public int NumFramesToExecute;
    public int BaseDamage;

    public int ApplyDamage(bool success)
    {
        var damage = success ? Damage : Damage / 2;
        return damage;
    }

    public void BoostDamage(float boostPercentage)
    {
        var perc = boostPercentage / 100f;
        var newDamage = perc * BaseDamage;
        Damage = (int)newDamage;
    }
}
