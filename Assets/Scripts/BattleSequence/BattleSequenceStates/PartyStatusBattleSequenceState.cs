using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyStatusBattleSequenceState : BattleSequenceState
{
    public override void EnterState(BattleSequenceStateArgs enterArgs)
    {
        base.EnterState(enterArgs);

        stateArgs = enterArgs;

        if(!stateArgs.PlayerParty.IsPartyAlive() || !stateArgs.EnemyParty.IsPartyAlive())
        {
            if(enterArgs.PlayerParty.IsPartyAlive())
            {
                var exp = enterArgs.EnemyParty.GetPartyExperiencePoints();
                BattleSequenceTransition.Instance.MainPlayer.IncrementExperiencePoints(exp);
                BattleSequenceTransition.Instance.MainPlayer.Health = stateArgs.PlayerParty.GetNextPlayer().PlayerStats.Health;
                BattleSequenceTransition.Instance.MainPlayer.Shadow = stateArgs.PlayerParty.GetNextPlayer().PlayerStats.Shadow;
                BattleSequenceTransition.Instance.UpdateAdditionsInformation(((DartBattlePlayer)stateArgs.PlayerParty.GetNextPlayer()).GetAdditionsCount());
            }

            BattleSequenceTransition.Instance.UnloadBattleSequence(stateArgs.PlayerParty.IsPartyAlive());
            stateArgs.PlayerParty.ClearPlayersFromParty();
            stateArgs.EnemyParty.ClearPlayersFromParty();
        }
        else
        {
            ExitState(stateArgs);
        }
    }
}
