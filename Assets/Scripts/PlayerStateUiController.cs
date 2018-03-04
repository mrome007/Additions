using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUiController : MonoBehaviour 
{
    #region Instance

    public static PlayerStateUiController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (PlayerStateUiController)FindObjectOfType(typeof(PlayerStateUiController));
            }
            return instance;
        }
    }

    private static PlayerStateUiController instance = null;

    #endregion

    #region Dart Player Inspector elements

    [SerializeField]
    private Slider healthSlider;

    [SerializeField]
    private Slider shadowSlider;

    [SerializeField]
    private Text levelText;

    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = (PlayerStateUiController)FindObjectOfType(typeof(PlayerStateUiController));
        }
    }

    public void UpdatePlayerHealth(float hpPercentage)
    {
        healthSlider.value = hpPercentage;
    }

    public void UpdateShadowMeter(float shadowPercentage)
    {
        shadowSlider.value = shadowPercentage;
    }

    public void UpdatePlayerLevel(int lvl)
    {
        levelText.text = lvl.ToString();
    }
}
