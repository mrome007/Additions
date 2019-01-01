using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSequenceState : MonoBehaviour 
{
    public Action EnteredState;
    public Action ExitedState;
    
    [SerializeField]
    protected BattleSequenceState defaultNextState;

    protected BattleSequenceState nextState;

    public virtual void EnterState(BattleSequenceStateArgs enterArgs = null)
	{
        PostEnteredState();
        
        nextState = defaultNextState;
	    RegisterEvents();	
	}

    public virtual void ExitState(BattleSequenceStateArgs exitArgs = null)
	{
	    UnRegisterEvents();
        nextState.EnterState();

        PostExitedState();
	}

	public virtual void RegisterEvents()
	{
	}

	public virtual void UnRegisterEvents()
    {
	}

    protected virtual void PostEnteredState()
    {
        EnteredState.Invoke();
    }

    protected virtual void PostExitedState()
    {
        ExitedState.Invoke();
    }
}
