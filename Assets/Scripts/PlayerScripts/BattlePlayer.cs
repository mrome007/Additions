using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BattlePlayer : MonoBehaviour 
{
    public Player PlayerStats
    {
        get
        {
            return player;
        }
        private set
        {
            player = value;
        }
    }
    public bool Alive { get{ return PlayerStats.Health > 0; } }

    public event EventHandler<ActionEventArgs> ActionStart;
    public event EventHandler<ActionEventArgs> ActionEnd;
    protected ActionType currentAction = ActionType.Idle;

    private Player player;

    protected virtual void Awake()
    {
        PlayerStats = GetComponent<Player>();

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
        var damagePercent = (float)(PlayerStats.Strength) / (PlayerStats.Strength + target.PlayerStats.Defense);
        var baseDamage = (float)(damage + PlayerStats.Strength);
        var totalDamage = (int)(baseDamage * damagePercent);

        if(totalDamage <= 0)
        {
            totalDamage = PlayerStats.Strength > 1 ? PlayerStats.Strength / 2 : 1;
        }

        target.TakeDamage(totalDamage);
    }

    protected virtual void TakeDamage(int damage)
    {
        player.Health -= damage;
        Debug.Log(gameObject.name + ": " + player.Health);
        CheckDeath();
    }

    protected virtual void CheckDeath()
    {
        if(player.Health <= 0)
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
