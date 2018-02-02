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

    private Additions currentAdditions;
    private const int additionExecuteOffset = 10;
    private WaitForSeconds delayShowAdditionBoxTime;
    private WaitForSeconds additionDelayTime;

    private void Start()
    {
        //TODO temporary and will be able to cycle different additions attack.
        currentAdditions = additions[0];
        delayShowAdditionBoxTime = new WaitForSeconds(0.15f);
        additionDelayTime = new WaitForSeconds(0.1f);
    }
    
    public override void PlayerAttack(Player target)
    {
        base.PlayerAttack(target);
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
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    if(frameCount >= numFrameLowerLimit && frameCount < numFrameUpperLimit)
                    {
                        additionSuccess = true;
                        break;
                    }
                    else
                    {
                        additionSuccess = false;
                        break;
                    }
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
                break;
            }

            yield return delayShowAdditionBoxTime;

            additionBox.ShowAdditionBox(false);
            additionBox.Reset();

            yield return additionDelayTime;
        }

        if(index == currentAdditions.Addition.Count)
        {
            //Do final attack here.
        }

        yield return delayShowAdditionBoxTime;
            
        additionBox.ShowAdditionBox(false);
        additionBox.Reset();

        EndAction(currentAction, damage);
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
    public float Damage;
    public int NumFramesToExecute;

    public float ApplyDamage(bool success)
    {
        var damage = success ? Damage : Damage / 2f;
        return damage;
    }
}
