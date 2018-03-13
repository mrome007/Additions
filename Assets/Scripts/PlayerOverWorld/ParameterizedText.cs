using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParameterizedText : MonoBehaviour 
{
    [SerializeField]
    private Text text;
    
    [SerializeField]
    private string textFormat;

    public void UpdateText(object [] args)
    {
        text.text = string.Format(textFormat, args);
    }
}
