using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BattlePlayer : MonoBehaviour 
{
    public int TurnPoints { get; private set; }
    public bool Alive { get{ return health > 0; } }

    public event EventHandler<ActionEventArgs> ActionStart;
    public event EventHandler<ActionEventArgs> ActionEnd;
    protected ActionType currentAction = ActionType.Idle;

    private int health;

    protected virtual void Awake()
    {
        var player = GetComponent<Player>();
        TurnPoints = player != null ? player.Speed : 1;
        health = player.Health;

        //Subscriptions
        ActionEnd += HandleBattlePlayerActionEnded;
    }

    protected virtual void OnDestroy()
    {
        ActionEnd -= HandleBattlePlayerActionEnded;
    }

    protected virtual void StartAction()
    {
        var handler = ActionStart;
        if(handler != null)
        {
            handler(this, null);
        }
    }

    protected virtual void EndAction(ActionType act, int hp, BattlePlayer targ)
    {
        var handler = ActionEnd;
        if(handler != null)
        {
            handler(this, new ActionEventArgs(act, hp, targ));
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

    protected virtual void ApplyDamage(int damage, BattlePlayer target)
    {
        target.TakeDamage(damage);
    }

    protected virtual void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + ": " + health);
        CheckDeath();
    }

    protected virtual void CheckDeath()
    {
        if(health <= 0)
        {
            //TODO temporary. do something else when players die.
            gameObject.SetActive(false);
        }
    }

    #region EventHandlers

    protected virtual void HandleBattlePlayerActionEnded(object sender, ActionEventArgs actArgs)
    {
        switch(actArgs.ActionType)
        {
            case ActionType.Attack:
                ApplyDamage(actArgs.HitPoints, actArgs.Target);
                break;

            case ActionType.Defend:
                break;

            case ActionType.Heal:
                break;
            
            case ActionType.Idle:
                break;

            default:
                break;
        }
    }

    #endregion
}
