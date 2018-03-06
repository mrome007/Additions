using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionMilestones : MonoBehaviour 
{
    //Public for now, til I figure out the entire logic.
    public List<AdditionMilestone> Additions;
}

[Serializable]
public class AdditionMilestone
{
    public string MilestoneName;
    public List<AdditionTarget> Milestones;
}

[Serializable]
public class AdditionTarget
{
    public enum BoostType
    {
        Damage,
        Multiplier,
        Frames
    }

    public BoostType AdditionBoostType;
    public int BoostValue;
    public int Target;
}