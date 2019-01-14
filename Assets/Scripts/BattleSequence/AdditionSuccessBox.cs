using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdditionSuccessBox : MonoBehaviour 
{
    [SerializeField]
    private Transform additionBox;

    [SerializeField]
    private Transform additionLineTop;
    [SerializeField]
    private Transform additionLineBottom;
    [SerializeField]
    private Transform additionLineLeft;
    [SerializeField]
    private Transform additionLineRight;

    private readonly Vector2 originalTopLeftPoint = new Vector2(-3f, 3f);
    private readonly Vector2 originalBottomRightPoint = new Vector2(3f, -3f);
    private const float originalScale = 3f;
    private const float scaleFactor = 25f;

    private Vector2 topLeftPoint;
    private Vector2 bottomRightPoint;

    private void Awake()
    {
        Reset();
        ShowAdditionBox(false);
    }

    public void ShowAdditionBox(bool show)
    {
        additionBox.gameObject.SetActive(show);
        additionLineTop.gameObject.SetActive(show);
        additionLineBottom.gameObject.SetActive(show);
        additionLineLeft.gameObject.SetActive(show);
        additionLineRight.gameObject.SetActive(show);
    }

    public void ScaleAdditionBox(int numFrames)
    {
        var scaleFactor = originalScale / numFrames;
        topLeftPoint -= new Vector2(-scaleFactor, scaleFactor);
        bottomRightPoint -= new Vector2(scaleFactor, -scaleFactor);
        ScaleAdditionBox();
    }

    public void AdditionSuccess(bool success)
    {
    }

    public void Reset()
    {
        topLeftPoint = originalTopLeftPoint;
        bottomRightPoint = originalBottomRightPoint;
        ScaleAdditionBox();
    }

    private void ScaleAdditionBox()
    {
        var dummyPoint = -1f * topLeftPoint;
        var difference = topLeftPoint - dummyPoint;
        var scale = Mathf.Abs(difference.x) * scaleFactor;

        var verticalScale = new Vector3(1f, scale, 1f);
        var horizontalScale = new Vector3(scale, 1f, 1f);
        var leftPosition = new Vector3(topLeftPoint.x, 0f, 0f);
        var rightPosition = new Vector3(bottomRightPoint.x, 0f, 0f);
        var topPosition = new Vector3(0f, topLeftPoint.y, 0f);
        var bottomPosition = new Vector3(0f, bottomRightPoint.y, 0f);

        additionLineTop.localPosition = topPosition;
        additionLineTop.localScale = horizontalScale;

        additionLineBottom.localPosition = bottomPosition;
        additionLineBottom.localScale = horizontalScale;

        additionLineLeft.localPosition = leftPosition;
        additionLineLeft.localScale = verticalScale;

        additionLineRight.localPosition = rightPosition;
        additionLineRight.localScale = verticalScale;
    }
}
