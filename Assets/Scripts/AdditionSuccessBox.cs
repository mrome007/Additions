using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionSuccessBox : MonoBehaviour 
{
    [SerializeField]
    private SpriteRenderer successBox;

    [SerializeField]
    private float originalScale;

    private Vector3 scaleVector;
    private float boxScale;

    private void Awake()
    {
        scaleVector = new Vector3(originalScale, originalScale, 1f);
        transform.localScale = scaleVector;
        boxScale = originalScale;
        ShowAdditionBox(false);
    }

    public void ShowAdditionBox(bool show)
    {
        successBox.gameObject.SetActive(show);
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
        successBox.color = Color.white;
        transform.localScale = scaleVector;
    }
}
