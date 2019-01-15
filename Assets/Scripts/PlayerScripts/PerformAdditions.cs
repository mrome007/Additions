using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformAdditions : MonoBehaviour 
{
    public event EventHandler<ActionEventArgs> AdditionComplete;

    [SerializeField]
    private AdditionSuccessBox additionBox;

    [SerializeField]
    private Animator dartPlayerAnimator;

    [SerializeField]
    private Camera BattleCamera;

    private const int additionUpperExecuteOffset = 2;
    private const int additionLowerExecuteOffset = 4;
    private WaitForSeconds delayShowAdditionBoxTime;
    private WaitForSeconds additionDelayTime;
    private WaitForSeconds finalAttackDelayTime;
    private WaitForSeconds delayEndAction;
    private Vector3 originalPosition;
    private float enemyPositionOffset = 1.25f;

    private Vector3 cameraMovementVector;

    private void Start()
    {
        delayShowAdditionBoxTime = new WaitForSeconds(0.15f);
        additionDelayTime = new WaitForSeconds(0.5f);
        finalAttackDelayTime = new WaitForSeconds(1.5f);
        delayEndAction = new WaitForSeconds(1.5f);
        originalPosition = transform.position;
        cameraMovementVector = Vector3.zero;
    }

    public void StartPerformAddition(BattlePlayer target, Additions currentAddition, int baseDamage)
    {
        StartCoroutine(ExecuteAddition(target, currentAddition, baseDamage));
    }

    private IEnumerator ExecuteAddition(BattlePlayer target, Additions currentAdditions, int baseDamage)
    {
        var damage = 0;
        var index = 0;

        for(; index < currentAdditions.Addition.Count; index++)
        {
            additionBox.ShowAdditionBox(true);

            var currentAddition = currentAdditions.Addition[index];
            var numFrames = currentAddition.NumFramesToExecute - additionUpperExecuteOffset;
            var numFrameLowerLimit = currentAddition.NumFramesToExecute - additionLowerExecuteOffset;
            var numFrameUpperLimit = numFrames;
            var additionSuccess = true;
            var frameCount = 0;

            if(index == 0)
            {
                var direction = (target.transform.position - transform.position).normalized;
                var distance = Vector3.Distance(transform.position, target.transform.position) - enemyPositionOffset;
                StartCoroutine(MovePlayerToTarget(numFrameLowerLimit - 1, distance, direction));
            }

            while(frameCount < currentAddition.NumFramesToExecute)
            {
                if(frameCount == numFrameLowerLimit)
                {
                    //dartAdditionAnimator.SetTrigger(addition.AttackTrigger);
                    dartPlayerAnimator.SetTrigger(currentAddition.AttackTrigger);
                }

                if(Input.GetKeyDown(KeyCode.Space))
                {
                    additionSuccess = (frameCount >= numFrameLowerLimit && frameCount <= numFrameUpperLimit);
                    break;
                }

                frameCount++;
                additionBox.ScaleAdditionBox(currentAddition.NumFramesToExecute);
                yield return null;
            }

            if(frameCount >= numFrames)
            {
                additionSuccess = false;
            }

            additionBox.AdditionSuccess(additionSuccess);

            damage += currentAddition.ApplyDamage(additionSuccess);

            if(!additionSuccess)
            {
                while(frameCount < numFrameLowerLimit)
                {
                    if(frameCount == numFrameLowerLimit - 1)
                    {
                        //dartAdditionAnimator.SetTrigger(addition.AttackTrigger);
                        dartPlayerAnimator.SetTrigger(currentAddition.AttackTrigger);
                    }
                    frameCount++;
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
            damage += baseDamage;
            yield return finalAttackDelayTime;
        }
        else
        {
            yield return delayEndAction;
        }

        additionBox.Reset();
        additionBox.ShowAdditionBox(false);
        transform.position = originalPosition;
        BattleCamera.orthographicSize = 5f;
        cameraMovementVector.x = 4f;
        cameraMovementVector.y = 2f;
        BattleCamera.transform.localPosition = cameraMovementVector;

        yield return additionDelayTime;

        PostAdditionComplete(damage, target);
    }

    private IEnumerator MovePlayerToTarget(int numberOfFrames, float distance, Vector3 direction)
    {
        var rate = distance / numberOfFrames;
        var count = 0;

        var xCamRate = 3f / numberOfFrames;
        var yCamRate = 1f / numberOfFrames;
        var orthRate = 2.5f / numberOfFrames;
        cameraMovementVector = BattleCamera.transform.localPosition;

        while(count < numberOfFrames)
        {
            transform.Translate(direction * rate);
            count++;
            BattleCamera.orthographicSize -= orthRate;
            cameraMovementVector.x -= xCamRate;
            cameraMovementVector.y -= yCamRate;

            cameraMovementVector.x = Mathf.Clamp(cameraMovementVector.x, 1f, 4f);
            cameraMovementVector.y = Mathf.Clamp(cameraMovementVector.y, 1f, 2f);

            BattleCamera.transform.localPosition = cameraMovementVector;

            yield return null;
        }
    }

    private void PostAdditionComplete(int damage, BattlePlayer target)
    {
        var handler = AdditionComplete;
        if(handler != null)
        {
            handler(this, new ActionEventArgs(ActionType.Attack, damage, target));
        }
    }
}
