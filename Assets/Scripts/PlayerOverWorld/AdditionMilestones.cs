using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionMilestones : MonoBehaviour 
{
    //Public for now, til I figure out the entire logic.
    public List<AdditionMilestone> Additions;

    public void ApplyMilestoneBoosts(DartBattlePlayer dart)
    {
        for(int index = 0; index < Additions.Count; index++)
        {
            var mileStone = Additions[index];
            for(int mIndex = 0; mIndex < mileStone.Milestones.Count; mIndex++)
            {
                var target = mileStone.Milestones[mIndex];
                if(mileStone.MilestoneCount >= target.Target)
                {
                    dart.BoostAdditions(mileStone.MilestoneName, target.AdditionBoostType, target.BoostValue);
                }
            }
        }
    }
}

[Serializable]
public class AdditionMilestone
{
    public string MilestoneName;
    public int MilestoneCount { get; private set; }
    public List<AdditionTarget> Milestones;

    public void SetMilestoneCount(int count)
    {
        MilestoneCount = count;
    }
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