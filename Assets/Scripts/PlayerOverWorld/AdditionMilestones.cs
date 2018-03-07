using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionMilestones : MonoBehaviour 
{
    //Public for now, til I figure out the entire logic.
    public List<AdditionMilestone> Additions;

    private Dictionary<string, AdditionMilestone> AdditionMilestonesContainer;

    private void Awake()
    {
        AdditionMilestonesContainer = new Dictionary<string, AdditionMilestone>();
        for(int index = 0; index < Additions.Count; index++)
        {
            var mileStone = Additions[index];
            if(!AdditionMilestonesContainer.ContainsKey(mileStone.MilestoneName))
            {
                AdditionMilestonesContainer.Add(mileStone.MilestoneName, mileStone);
            }
        }
    }

    public void ApplyMilestoneBoosts(DartBattlePlayer dart)
    {
        foreach(var mileStone in AdditionMilestonesContainer)
        {
            for(int mIndex = 0; mIndex < mileStone.Value.Milestones.Count; mIndex++)
            {
                var target = mileStone.Value.Milestones[mIndex];
                if(mileStone.Value.MilestoneCount >= target.Target)
                {
                    dart.BoostAdditions(mileStone.Value.MilestoneName, target.AdditionBoostType, target.BoostValue);
                }
            }
        }
    }

    public void UpdateMilestoneCount(string name, int count)
    {
        if(AdditionMilestonesContainer.ContainsKey(name))
        {
            AdditionMilestonesContainer[name].SetMilestoneCount(count);
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