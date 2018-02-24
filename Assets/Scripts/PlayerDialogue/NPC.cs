using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//TODO Ensure texts are only 140 characters long.
public class NPC : MonoBehaviour 
{
    [TextArea]
    [SerializeField]
    private string npcInspectorText;

    private StringBuilder uiText;

    [SerializeField]
    private GameObject textBoxObject;

    [SerializeField]
    private Text textBoxText;

    private Coroutine textShowCoroutine = null;
    private int maxLength = 140;

    private void Awake()
    {
        if(npcInspectorText.Length > maxLength)
        {
            Debug.LogError("Text can only be " + maxLength + " characters long.");
        }
        
        uiText = new StringBuilder(maxLength, maxLength);
        for(int index = 0; index < uiText.Capacity; index++)
        {
            uiText.Append(' ');
        }
    }

    private void Start()
    {
        ShowText(false);
    }

    public void ShowText(bool show)
    {
        textBoxObject.SetActive(show);

        if(show)
        {
            if(textShowCoroutine == null)
            {
                textShowCoroutine = StartCoroutine(GoThroughText());
            }
        }
        else
        {
            if(textShowCoroutine != null)
            {
                StopCoroutine(textShowCoroutine);
                textShowCoroutine = null;
            }
            textBoxText.text = string.Empty;
            for(int index = 0; index < uiText.Length; index++)
            {
                uiText[index] = ' ';
            }
        }
    }

    private IEnumerator GoThroughText()
    {
        for(int index = 0; index < npcInspectorText.Length; index++)
        {
            var character = npcInspectorText[index];
            uiText[index] = character;
            textBoxText.text = uiText.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
