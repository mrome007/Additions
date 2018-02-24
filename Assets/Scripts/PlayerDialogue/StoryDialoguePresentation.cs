using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StoryDialoguePresentation : MonoBehaviour 
{
    #region Instance

    public static StoryDialoguePresentation Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (StoryDialoguePresentation)FindObjectOfType(typeof(StoryDialoguePresentation));
            }
            return instance;
        }
    }

    private static StoryDialoguePresentation instance = null;

    #endregion

    public event EventHandler DialogueEnded;

    [SerializeField]
    private List<Sprite> characterSprites;
    
    [SerializeField]
    private GameObject storyDialoguePresentationContainer;

    [SerializeField]
    private GameObject characterPresentationContainer;

    [SerializeField]
    private Image leftCharacter;

    [SerializeField]
    private Image rightCharacter;

    [SerializeField]
    private Text textBoxText;
    
    private StringBuilder uiTextContainer;
    private int maxLength = 140;
    private Coroutine storyDialogueCoroutine = null;
    private WaitForSeconds dialogueDelay;

    private void Awake()
    {
        if(instance == null)
        {
            instance = (StoryDialoguePresentation)FindObjectOfType(typeof(StoryDialoguePresentation));
        }
        dialogueDelay = new WaitForSeconds(0.5f);
        ResetUiText();

        ShowStoryPresentation(false);
    }

    public void ShowStoryDialogue(List<StoryDialogue> dialogues)
    {
        storyDialogueCoroutine = StartCoroutine(ShowStoryDialogueRoutine(dialogues));
    }

    public void StopStoryDialogue()
    {
        if(storyDialogueCoroutine != null)
        {
            StopCoroutine(storyDialogueCoroutine);
        }

        ShowStoryPresentation(false);
    }

    public void ShowStoryPresentation(bool show)
    {
        storyDialoguePresentationContainer.SetActive(show);
    }

    private IEnumerator ShowStoryDialogueRoutine(List<StoryDialogue> dialogues)
    {
        ShowStoryPresentation(true);

        for(int index = 0; index < dialogues.Count; index++)
        {
            yield return StartCoroutine(GoThroughText(dialogues[index].StoryText));
            yield return dialogueDelay;
        }

        ShowStoryPresentation(false);

        var handler = DialogueEnded;
        if(handler != null)
        {
            handler(this, null);
        }
    }

    private void ResetUiText()
    {
        if(uiTextContainer == null)
        {
            uiTextContainer = new StringBuilder(maxLength, maxLength);
            for(int count = 0; count < maxLength; count++)
            {
                uiTextContainer.Append(' ');
            }
        }
        else
        {
            for(int index = 0; index < maxLength; index++)
            {
                uiTextContainer[index] = ' ';
            }
        }
    }

    private IEnumerator GoThroughText(string dialogueText)
    {
        ResetUiText();
        textBoxText.text = "";
        
        for(int index = 0; index < dialogueText.Length; index++)
        {
            var character = dialogueText[index];
            uiTextContainer[index] = character;
            textBoxText.text = uiTextContainer.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
