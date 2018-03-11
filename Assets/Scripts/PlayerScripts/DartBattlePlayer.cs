﻿using System;
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
    private AdditionSuccessBox additionBox;

    [SerializeField]
    private Animator dartPlayerAnimator;
    [SerializeField]
    private Animator dartAdditionAnimator;

    private Additions currentAdditions;
    private const int additionExecuteOffset = 6;
    private WaitForSeconds delayShowAdditionBoxTime;
    private WaitForSeconds additionDelayTime;
    private WaitForSeconds finalAttackDelayTime;
    private WaitForSeconds delayEndAction;
    private Vector3 originalPosition;
    private float enemyPositionOffset = 1.25f;

    private void Start()
    {
        currentAdditions = additions[0];
        delayShowAdditionBoxTime = new WaitForSeconds(0.15f);
        additionDelayTime = new WaitForSeconds(0.5f);
        finalAttackDelayTime = new WaitForSeconds(1.5f);
        delayEndAction = new WaitForSeconds(1.5f);
        originalPosition = transform.position;

    }
    
    public override void PlayerAttack(BattlePlayer target)
    {
        base.PlayerAttack(target);
        additionBox.Reset();
        StartCoroutine(ExecuteAddition(target));
    }

    public override void PlayerDefend(BattlePlayer target)
    {
        base.PlayerDefend(target);
        StartCoroutine(ExecuteDefense(target));
    }

    public override void PlayerHeal(BattlePlayer target)
    {
        base.PlayerHeal(target);
        StartCoroutine(ExecuteHeal(target));
    }

    public void ChangeAddition(int index)
    {
        if(index >= 0 & index < additions.Count)
        {
            currentAdditions = additions[index];
        }
    }

    private IEnumerator ExecuteAddition(BattlePlayer target)
    {
        var damage = 0;
        var index = 0;

        for(; index < currentAdditions.Addition.Count; index++)
        {
            additionBox.ShowAdditionBox(true);
            
            var addition = currentAdditions.Addition[index];
            var numFrames = addition.NumFramesToExecute + additionExecuteOffset;
            var numFrameLowerLimit = addition.NumFramesToExecute - additionExecuteOffset;
            var numFrameUpperLimit = numFrames;
            var additionSuccess = true;
            var frameCount = 0;

            if(index == 0)
            {
                var direction = (target.transform.position - transform.position).normalized;
                var distance = Vector3.Distance(transform.position, target.transform.position) - enemyPositionOffset;
                StartCoroutine(MovePlayerToTarget(numFrameLowerLimit - 1, distance, direction));
            }

            while(frameCount < numFrames)
            {
                if(frameCount == numFrameLowerLimit)
                {
                    //dartAdditionAnimator.SetTrigger(addition.AttackTrigger);
                    dartPlayerAnimator.SetTrigger(addition.AttackTrigger);
                }
                
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    additionSuccess = (frameCount >= numFrameLowerLimit && frameCount < numFrameUpperLimit);
                    break;
                }

                frameCount++;
                additionBox.ScaleAdditionBox(numFrames);
                yield return null;
            }

            if(frameCount >= numFrames)
            {
                additionSuccess = false;
            }

            additionBox.AdditionSuccess(additionSuccess);

            damage += addition.ApplyDamage(additionSuccess);

            if(!additionSuccess)
            {
                while(frameCount < numFrameLowerLimit)
                {
                    if(frameCount == numFrameLowerLimit - 1)
                    {
                        //dartAdditionAnimator.SetTrigger(addition.AttackTrigger);
                        dartPlayerAnimator.SetTrigger(addition.AttackTrigger);
                    }
                    frameCount++;
                    additionBox.ScaleAdditionBox(numFrames);
                    yield return null;
                }
                break;
            }

            yield return delayShowAdditionBoxTime;

            additionBox.Reset();
            additionBox.ShowAdditionBox(false);

            yield return additionDelayTime;
        }

        if(index == currentAdditions.Addition.Count)
        {
            currentAdditions.Count++;
            currentAdditions.Count %= 500;
            dartPlayerAnimator.SetTrigger(currentAdditions.FinalAttackTrigger);
            damage += PlayerStats.Strength;
            yield return finalAttackDelayTime;
        }

        yield return delayEndAction;

        additionBox.ShowAdditionBox(false);
        additionBox.Reset();
        transform.position = originalPosition;

        yield return additionDelayTime;

        EndAction(currentAction, damage, target);
    }

    private IEnumerator ExecuteHeal(BattlePlayer target)
    {
        yield return delayEndAction;
        var hp = UnityEngine.Random.Range(1, target.PlayerStats.HealthCap / 4);
        EndAction(currentAction, hp, target);
    }

    private IEnumerator ExecuteDefense(BattlePlayer target)
    {
        yield return delayEndAction;

        EndAction(currentAction, 1, target);
    }

    private IEnumerator MovePlayerToTarget(int numberOfFrames, float distance, Vector3 direction)
    {
        var rate = distance / numberOfFrames;
        var count = 0;

        while(count < numberOfFrames)
        {
            transform.Translate(direction * rate);
            count++;
            yield return null;
        }
    }

    //For now pick a random addition to apply boost to.
    public void BoostAdditions(string additionName, AdditionTarget.BoostType boost, int boostValue)
    {
        //List works for now since the additions list are small.
        var additionSelected = additions.Find(add => add.Name == additionName);

        var addition = additionSelected.Addition[UnityEngine.Random.Range(0,additionSelected.Addition.Count)];
        switch(boost)
        {
            case AdditionTarget.BoostType.Damage:
                addition.Damage += boostValue;
                break;

            case AdditionTarget.BoostType.Frames:
                addition.NumFramesToExecute += boostValue;
                break;

            case AdditionTarget.BoostType.Multiplier:
                addition.SuccessMultiplier += boostValue;
                break;
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
public struct Addition
{
    public string AttackTrigger;
    public int Damage;
    public int NumFramesToExecute;
    public int SuccessMultiplier;

    public int ApplyDamage(bool success)
    {
        var damage = success ? Damage * SuccessMultiplier : Damage / 2;
        return damage;
    }
}
