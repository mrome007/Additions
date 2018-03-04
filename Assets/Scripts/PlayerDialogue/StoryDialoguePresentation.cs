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
    private List<DialogueSprites> characterSprites;
    
    [SerializeField]
    private GameObject storyDialoguePresentationContainer;

    [SerializeField]
    private Image shadowCharacter;

    [SerializeField]
    private Image leftCharacter;

    [SerializeField]
    private Image rightCharacter;

    [SerializeField]
    private Text textBoxText;

    [SerializeField]
    private Text characterSpeakingText;
    
    private StringBuilder uiTextContainer;
    private int maxLength = 140;
    private Coroutine storyDialogueCoroutine = null;
    private WaitForSeconds dialogueDelay;
    private WaitForSeconds perCharDelay;

    private void Awake()
    {
        if(instance == null)
        {
            instance = (StoryDialoguePresentation)FindObjectOfType(typeof(StoryDialoguePresentation));
        }
        dialogueDelay = new WaitForSeconds(1f);
        perCharDelay = new WaitForSeconds(0.1f);
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
        HideAllCharacters();

        for(int index = 0; index < dialogues.Count; index++)
        {
            var dialogue = dialogues[index];
            characterSpeakingText.text = dialogue.CharacterSpeaking.ToString();
            ShowCharacters(dialogue.CharacterSpeaking);
            yield return StartCoroutine(GoThroughText(dialogue.StoryText));
            yield return dialogueDelay;
        }

        yield return dialogueDelay;

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
            yield return perCharDelay;
        }
    }

    private void ShowCharacters(StoryDialogue.Characters character)
    {
        switch(character)
        {
            case StoryDialogue.Characters.Dart:
                leftCharacter.gameObject.SetActive(true);
                shadowCharacter.gameObject.SetActive(false);
                break;

            //TODO make this more generic, as all secondary characters will do this.
            case StoryDialogue.Characters.DarkTree:
                rightCharacter.sprite = characterSprites.Find(spr => spr.Character == character).CharacterSprite;
                rightCharacter.gameObject.SetActive(true);
                break;

            case StoryDialogue.Characters.Shadow:
                shadowCharacter.gameObject.SetActive(true);
                leftCharacter.gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }

    private void HideAllCharacters()
    {
        shadowCharacter.gameObject.SetActive(false);
        leftCharacter.gameObject.SetActive(false);
        rightCharacter.gameObject.SetActive(false);
    }
}

[Serializable]
public class StoryDialogue
{
    public enum Characters
    {
        Dart,
        Shadow,
        DarkTree
    }

    public Characters CharacterSpeaking;

    [TextArea]
    public string StoryText;
}

[Serializable]
public class DialogueSprites
{
    public StoryDialogue.Characters Character;
    public Sprite CharacterSprite;
}
