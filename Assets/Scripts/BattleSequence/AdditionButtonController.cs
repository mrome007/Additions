using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionButtonController : MonoBehaviour 
{
    [SerializeField]
    private List<AdditionButton> additionButtons;

    private bool initialized = false;
    private List<AdditionButton> enabledAdditionButtons;
    private int currentButtonIndex;

    private void Awake()
    {
        initialized = false;
        enabledAdditionButtons = new List<AdditionButton>();
        currentButtonIndex = 0;
    }

    public void ShowAdditionButtons(bool enable, List<string> additions = null)
    {
        if(!initialized)
        {
            InitializeButtons(additions);
        }

        additionButtons.ForEach(button => button.gameObject.SetActive(false));
        enabledAdditionButtons.ForEach(button => button.gameObject.SetActive(enable));
    }

    private void InitializeButtons(List<string> additions)
    {
        if(additions == null)
        {
            return;
        }
        
        initialized = true;
        for(int index = 0; index < additions.Count; index++)
        {
            var buttonName = additions[index];
            var button = additionButtons.Find(addB => addB.AdditionName == buttonName);
            if(button != null)
            {
                enabledAdditionButtons.Add(button);
            }
        }
    }

    public AdditionButton GetCurrentAdditionButton()
    {
        if(currentButtonIndex >= 0 && currentButtonIndex < enabledAdditionButtons.Count)
        {
            return enabledAdditionButtons[currentButtonIndex];
        }

        return null;
    }

    public AdditionButton GetNextAdditionButton()
    {
        if(currentButtonIndex >= 0 && currentButtonIndex < enabledAdditionButtons.Count)
        {
            currentButtonIndex++;
            currentButtonIndex %= enabledAdditionButtons.Count;
        }

        return enabledAdditionButtons[currentButtonIndex];
    }

    public AdditionButton GetPreviousAdditionButton()
    {
        if(currentButtonIndex >= 0 && currentButtonIndex < enabledAdditionButtons.Count)
        {
            currentButtonIndex--;
            if(currentButtonIndex < 0)
            {
                currentButtonIndex += enabledAdditionButtons.Count;
            }
        }

        return enabledAdditionButtons[currentButtonIndex];
    }
}
