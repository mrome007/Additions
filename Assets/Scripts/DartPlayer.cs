using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DartPlayer : Player
{
    [SerializeField]
    private List<Additions> additions;

    private Additions currentAdditions;
    private const int additionExecuteOffset = 20;

    private void Start()
    {
        //TODO temporary and will be able to cycle different additions attack.
        currentAdditions = additions[0];
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
                        //Debug.Log("Success " + index);
                        break;
                    }
                    else
                    {
                        additionSuccess = false;
                        Debug.Log("X Success " + index);
                        break;
                    }
                }
                Debug.Log("Frame Count " + frameCount);
                frameCount++;
                yield return null;
            }

            if(frameCount >= numFrames)
            {
                additionSuccess = false;
                Debug.Log("X O Success " + index);
            }
            
            damage += addition.ApplyDamage(additionSuccess);

            if(!additionSuccess)
            {
                break;
            }
            yield return null;
        }

        if(index == currentAdditions.Addition.Count)
        {
            //Do final attack here.
        }

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
