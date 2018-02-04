using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionSuccessBox : MonoBehaviour 
{
    //This may change in the future depending on how I indicate success.
    [SerializeField]
    private SpriteRenderer successBox;

    [SerializeField]
    private float originalScale;

    [SerializeField]
    private Selectable keyIndicator;

    private Vector3 scaleVector;
    private float boxScale;
    private Color originalColor;

    private void Awake()
    {
        scaleVector = new Vector3(originalScale, originalScale, 1f);
        transform.localScale = scaleVector;
        boxScale = originalScale;
        ShowAdditionBox(false);
        var origColor = 100f / 255f; 
        originalColor = new Color(origColor, origColor, origColor, 30f / 100f);
    }

    public void ShowAdditionBox(bool show)
    {
        keyIndicator.gameObject.SetActive(show);
        keyIndicator.interactable = show;
        successBox.gameObject.SetActive(show);
    }

    public void ShowAdditionExecuted(bool show)
    {
        keyIndicator.interactable = show;
    }

    public void ScaleAdditionBox(int numFrames)
    {
        var scaleRate = (originalScale - 1f) / numFrames;
        boxScale -= scaleRate;
        scaleVector.x = boxScale;
        scaleVector.y = boxScale;

        transform.localScale = scaleVector;
    }

    public void AdditionSuccess(bool success)
    {
        successBox.color = success ? Color.green : Color.red;
    }

    public void Reset()
    {
        scaleVector.x = originalScale;
        scaleVector.y = originalScale;
        boxScale = originalScale;
        successBox.color = originalColor;
        transform.localScale = scaleVector;
    }
}
