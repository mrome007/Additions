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

    #region Defense Buff

    private int baseDefense;
    private bool defenseBuffed = false;

    #endregion

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
        switch(currentAction)
        {
            //ABLE TO STACK BUFFS. For now able to stack defense buffs, so if the previous action is not a buff
            //then reset the buffs.
            case ActionType.Attack:
            case ActionType.Heal:
                ResetBuffs();
                break;

            default:
                break;
        }

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

    protected virtual void IncreaseHealth(int hp)
    {
        var hpDifference = player.HealthCap - player.Health;
        if(hp > hpDifference)
        {
            hp = hpDifference;
        }
        player.Health += hp;
    }

    protected virtual void ApplyHealth(int hp, BattlePlayer target)
    {
        target.IncreaseHealth(hp);
    }

    protected virtual void BuffDefense(int points)
    {
        if(!defenseBuffed)
        {
            defenseBuffed = true;
            baseDefense = player.Defense;
        }

        player.Defense += points;
    }

    protected virtual void ApplyDefense(int points, BattlePlayer target)
    {
        target.BuffDefense(points);
    }

    protected virtual void ResetBuffs()
    {
        if(defenseBuffed)
        {
            defenseBuffed = false;
            player.Defense = baseDefense;
        }

        //TODO strength and speed buffs in the future.
    }
        
    #region EventHandlers

    protected virtual void HandleBattlePlayerActionEnded(object sender, ActionEventArgs actArgs)
    {
        switch(actArgs.ActionType)
        {
            case ActionType.Attack:
                ApplyDamage(actArgs.DataPoints, actArgs.Target);
                break;

            case ActionType.Defend:
                ApplyDefense(actArgs.DataPoints, actArgs.Target);
                break;

            case ActionType.Heal:
                ApplyHealth(actArgs.DataPoints, actArgs.Target);
                break;
            
            case ActionType.Idle:
                break;

            default:
                break;
        }
    }

    #endregion
}
