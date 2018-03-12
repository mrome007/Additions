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
            dart.EnableAdditions(mileStone.Key, mileStone.Value.Enabled);
            if(mileStone.Value.Enabled)
            {
                for(int mIndex = 0; mIndex < mileStone.Value.Milestones.Count; mIndex++)
                {
                    var target = mileStone.Value.Milestones[mIndex];
                    if(mileStone.Value.MilestoneCount >= target.Target)
                    {
                        dart.BoostAdditions(mileStone.Key, target.DamagePercent);
                    }
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

    public void UpdateEnabledAdditions(int lvl)
    {
        foreach(var mileStone in AdditionMilestonesContainer)
        {
            mileStone.Value.Enabled = mileStone.Value.LevelEnabled >= lvl;
        }
    }
}

[Serializable]
public class AdditionMilestone
{
    public string MilestoneName;
    public bool Enabled;
    public int LevelEnabled;
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
    public float DamagePercent;
    public int Target;
}