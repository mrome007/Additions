using System.Collections;
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

    private void Start()
    {
        //TODO temporary and will be able to cycle different additions attack.
        currentAdditions = additions[0];
        delayShowAdditionBoxTime = new WaitForSeconds(0.15f);
        additionDelayTime = new WaitForSeconds(0.25f);
    }
    
    public override void PlayerAttack(Player target)
    {
        base.PlayerAttack(target);
        additionBox.Reset();
        StartCoroutine(ExecuteAddition());
    }

    private IEnumerator ExecuteAddition()
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

            while(frameCount < numFrames)
            {
                if(frameCount == numFrameLowerLimit)
                {
                    dartPlayerAnimator.SetTrigger(addition.AttackTrigger);
                    dartAdditionAnimator.SetTrigger(addition.AttackTrigger);
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
                while(frameCount < numFrames)
                {
                    if(frameCount == numFrameLowerLimit)
                    {
                        dartPlayerAnimator.SetTrigger(addition.AttackTrigger);
                        dartAdditionAnimator.SetTrigger(addition.AttackTrigger);
                    }
                    frameCount++;
                    additionBox.ScaleAdditionBox(numFrames);
                    yield return null;
                }
                break;
            }

            yield return delayShowAdditionBoxTime;

            additionBox.ShowAdditionBox(false);
            additionBox.Reset();

            yield return additionDelayTime;
        }

        if(index == currentAdditions.Addition.Count)
        {
            dartPlayerAnimator.SetTrigger(currentAdditions.FinalAttackTrigger);
            dartAdditionAnimator.SetTrigger(currentAdditions.FinalAttackTrigger);
            yield return new WaitForSeconds(1f);
        }

        yield return delayShowAdditionBoxTime;

        additionBox.ShowAdditionBox(false);
        additionBox.Reset();

        EndAction(currentAction, damage);
    }

    private void MovePlayerToTarget()
    {

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
