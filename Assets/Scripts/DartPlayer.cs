﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DartPlayer : Player
{
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
        additionDelayTime = new WaitForSeconds(0.25f);
        finalAttackDelayTime = new WaitForSeconds(1.15f);
        delayEndAction = new WaitForSeconds(1f);
        originalPosition = transform.position;

    }
    
    public override void PlayerAttack(Player target)
    {
        base.PlayerAttack(target);
        additionBox.Reset();
        StartCoroutine(ExecuteAddition(target));
    }

    public void ChangeAddition(int index)
    {
        if(index >= 0 & index < additions.Count)
        {
            currentAdditions = additions[index];
        }
    }

    private IEnumerator ExecuteAddition(Player target)
    {
        var damage = 0f;
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
                    dartAdditionAnimator.SetTrigger(addition.AttackTrigger);
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
                        dartAdditionAnimator.SetTrigger(addition.AttackTrigger);
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

            yield return additionDelayTime;
        }

        if(index == currentAdditions.Addition.Count)
        {
            dartPlayerAnimator.SetTrigger(currentAdditions.FinalAttackTrigger);
            dartAdditionAnimator.SetTrigger(currentAdditions.FinalAttackTrigger);
            yield return finalAttackDelayTime;
        }
        else
        {
            dartPlayerAnimator.Play("Idle");
            dartAdditionAnimator.Play("Idle");
        }

        yield return delayEndAction;

        additionBox.ShowAdditionBox(false);
        additionBox.Reset();
        transform.position = originalPosition;

        yield return additionDelayTime;

        EndAction(currentAction, damage);
    }

    private IEnumerator MovePlayerToTarget(int numberOfFrames, float distance, Vector3 direction)
    {
        var rate = distance / numberOfFrames;
        var count = 0;
        dartPlayerAnimator.SetTrigger("skill");
        dartAdditionAnimator.SetTrigger("skill");
        while(count < numberOfFrames)
        {
            transform.Translate(direction * rate);
            count++;
            yield return null;
        }
    }
}

[Serializable]
public class Additions
{
    public string Name;
    public List<Addition> Addition;
    public string FinalAttackTrigger;
}

[Serializable]
public struct Addition
{
    public string AttackTrigger;
    public float Damage;
    public int NumFramesToExecute;

    public float ApplyDamage(bool success)
    {
        var damage = success ? Damage : Damage / 2f;
        return damage;
    }
}
