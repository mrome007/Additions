using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionMilestones : MonoBehaviour 
{
    public int MilestonesCount { get { return Additions.Count; } }

    [SerializeField]
    private List<AdditionMilestone> Additions;

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

    public AdditionMilestone GetMileStone(int index)
    {
        if(index >= 0 && index < Additions.Count)
        {
            return Additions[index];
        }

        return null;
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

    public AdditionTarget GetNextTarget()
    {
        AdditionTarget result = null;
        for(int mIndex = 0; mIndex < Milestones.Count; mIndex++)
        {
            var target = Milestones[mIndex];
            if(MilestoneCount >= target.Target)
            {
                result = target;
                break;
            }
        }

        return result;
    }

    public AdditionTarget GetCurrentTarget()
    {
        AdditionTarget result = null;
        for(int mIndex = 0; mIndex < Milestones.Count; mIndex++)
        {
            var target = Milestones[mIndex];
            if(MilestoneCount < target.Target)
            {
                result = target;
                break;
            }
        }

        return result;
    }

    public int GetLevel()
    {
        var count = 1;
        for(int mIndex = 0; mIndex < Milestones.Count; mIndex++)
        {
            var target = Milestones[mIndex];
            if(MilestoneCount >= target.Target)
            {
                count++;
            }
        }

        return count;
    }
}

[Serializable]
public class AdditionTarget
{
    public float DamagePercent;
    public int Target;
}