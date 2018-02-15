using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BattlePlayer : MonoBehaviour 
{
    public int TurnPoints { get; private set; }
    public event EventHandler<ActionEventArgs> ActionStart;
    public event EventHandler<ActionEventArgs> ActionEnd;
    protected ActionType currentAction = ActionType.Idle;

    protected virtual void Awake()
    {
        var player = GetComponent<Player>();
        TurnPoints = player != null ? player.Speed : 1;
    }

    protected virtual void StartAction()
    {
        var handler = ActionStart;
        if(handler != null)
        {
            handler(this, null);
        }
    }

    protected virtual void EndAction(ActionType act, float hp)
    {
        var handler = ActionEnd;
        if(handler != null)
        {
            handler(this, new ActionEventArgs(act, hp));
        }
    }

    public virtual void PlayerAttack(BattlePlayer target)
    {
        currentAction = ActionType.Attack;
        StartAction();
    }

    public virtual void PlayerDefend(BattlePlayer target)
    {
        currentAction = ActionType.Defend;
        StartAction();
    }

    public virtual void PlayerHeal(BattlePlayer target)
    {
        currentAction = ActionType.Heal;
        StartAction();
    }
}
