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
    protected BattleSequenceStateArgs stateArgs;

    protected virtual void Awake()
    {
        stateArgs = null;
    }

    public virtual void EnterState(BattleSequenceStateArgs enterArgs = null)
	{
        PostEnteredState();
        
        nextState = defaultNextState;
	    RegisterEvents();	
	}

    public virtual void ExitState(BattleSequenceStateArgs exitArgs = null)
	{
	    UnRegisterEvents();
        nextState.EnterState(exitArgs);

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
        if(EnteredState != null)
        {
            EnteredState.Invoke();
        }
    }

    protected virtual void PostExitedState()
    {
        if(ExitedState != null)
        {
            ExitedState.Invoke();
        }
    }
}
