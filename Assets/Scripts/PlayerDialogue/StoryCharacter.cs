using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class StoryCharacter : MonoBehaviour 
{
    public bool Activated { get; private set; }

    [SerializeField]
    private List<StoryDialogue> dialogues;

    private int maxLength = 140;

    private void Awake()
    {
        if(dialogues != null)
        {
            var moreThanMaxLength = dialogues.Find(dialogue => dialogue.StoryText.Length > maxLength);
            if(moreThanMaxLength != null)
            {
                Debug.LogError("One of the texts is/are " + maxLength + " characters long. Text has to be shorter.");
            }
        }

        Activated = false;
    }

    public void ShowDialogue()
    {
        Activated = true;
        StoryDialoguePresentation.Instance.ShowStoryDialogue(dialogues);
    }
}