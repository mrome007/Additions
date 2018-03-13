using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsMenuController : MonoBehaviour 
{
    [SerializeField]
    private DartPlayer player;

    [SerializeField]
    private AdditionMilestones playerAdditions;

    #region Player Stats Text

    [SerializeField]
    private Text levelNumberText;

    [SerializeField]
    private Text strengthNumberText;

    [SerializeField]
    private Text defenseNumberText;

    [SerializeField]
    private Text speedNumberText;

    [SerializeField]
    private ParameterizedText healthNumberText;

    [SerializeField]
    private ParameterizedText shadowNumberText;

    [SerializeField]
    private ParameterizedText experienceNumberText;

    #endregion

    #region Additions Text

    [SerializeField]
    private List<ParameterizedText> boostPercentTexts;

    [SerializeField]
    private List<ParameterizedText> additionTargetTexts;

    #endregion

    private void OnEnable()
    {
        UpdateStats();
    }

    private void Start()
    {
        UpdateStats();
    }

    private void UpdateStats()
    {
        levelNumberText.text = player.Level.ToString();
        strengthNumberText.text = player.Strength.ToString();
        defenseNumberText.text = player.Defense.ToString();
        speedNumberText.text = player.Speed.ToString();

        healthNumberText.UpdateText(new object[]{ player.Health, player.HealthCap });
        //TODO shadow.
        experienceNumberText.UpdateText(new object[]{player.Experience, player.ExperienceCap});

        for(int index = 0; index < playerAdditions.MilestonesCount; index++)
        {
            var additionMilestone = playerAdditions.GetMileStone(index);
            var currentTarget = additionMilestone.GetCurrentTarget();
            var nextTarget = additionMilestone.GetNextTarget();

            if(currentTarget != null)
            {
                additionTargetTexts[index].UpdateText(new object[] {
                    additionMilestone.MilestoneCount,
                    currentTarget.Target
                });
            }
            else
            {
                additionTargetTexts[index].UpdateText(new object[] {
                    additionMilestone.MilestoneCount,
                    nextTarget.Target
                });
            }

            if(nextTarget != null)
            {
                boostPercentTexts[index].UpdateText(new object[]{ currentTarget != null ? currentTarget.DamagePercent : nextTarget.DamagePercent });
            }
            else
            {
                boostPercentTexts[index].UpdateText(new object[]{ 100 });
            }
        }
    }
}
