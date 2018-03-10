using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionButtonController : MonoBehaviour 
{
    [SerializeField]
    private List<PlayerActionButton> actionButtons;

    private int currentActionButtonIndex;

    private void Awake()
    {
        currentActionButtonIndex = 0;
    }

    public void ShowActionButtons(bool show)
    {
        actionButtons.ForEach(button => button.gameObject.SetActive(show));
    }

    public PlayerActionButton GetNextActionButton()
    {
        currentActionButtonIndex++;
        currentActionButtonIndex %= actionButtons.Count;
        return actionButtons[currentActionButtonIndex];
    }

    public PlayerActionButton GetPreviousActionButton()
    {
        currentActionButtonIndex--;
        if(currentActionButtonIndex < 0)
        {
            currentActionButtonIndex += actionButtons.Count;
        }
        return actionButtons[currentActionButtonIndex];
    }

    public PlayerActionButton GetCurrentActionButton()
    {
        return actionButtons[currentActionButtonIndex];
    }
}
