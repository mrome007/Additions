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

    private Vector3 scaleVector;
    private float boxScale;
    private Color originalColor;

    private void Awake()
    {
        scaleVector = new Vector3(originalScale, originalScale, 1f);
        transform.localScale = scaleVector;
        boxScale = originalScale;
        ShowAdditionBox(true);
        originalColor = successBox.color;
    }

    public void ShowAdditionBox(bool show)
    {
        successBox.gameObject.SetActive(show);
    }

    public void ScaleAdditionBox(int numFrames)
    {
        var scaleRate = (originalScale - 1f) / numFrames;
        if(boxScale > 1f)
        {
            boxScale -= scaleRate;
        }
        else
        {
            boxScale = 1f;
        }
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
