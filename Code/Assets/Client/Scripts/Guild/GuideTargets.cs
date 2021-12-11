using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideTargets : MonoBehaviour {

    public GameObject LeftTop;
    public GameObject RightTop;
    public GameObject LeftBottom;
    public GameObject RightBottom;

    public float x;
    public float y;
    public float w;
    public float h;

    public GameObject TargetSign;

    public GameObject Mask;

    public void ShowTarget(Vector3 pos)
    {
        TargetSign.transform.localScale = Vector3.one;
        TargetSign.transform.localPosition = pos;
        TargetSign.SetActive(true);
    }
    public void OnClick()
    {
        GuideManager.Instance.HideGuide();
    }



    public void Hide()
    {
        transform.parent = GuideManager.Instance.transform;
        gameObject.SetActive(false);
    }
	
    public void ShowForceWithoutHand(Transform target,bool isTransformToParent, Rect rect, string word, Vector3 wordPos, bool isRotate)
    {
        x = rect.x; y = rect.y; w = rect.width; h = rect.height;
        LeftTop.transform.localPosition = new Vector3(x + w, y, 0.0f);
        RightTop.transform.localPosition = new Vector3(x + w, y - h, 0.0f);
        LeftBottom.transform.localPosition = new Vector3(x, y, 0.0f);
        RightBottom.transform.localPosition = new Vector3(x, y - h, 0.0f);

        Mask.GetComponent<UISprite>().SetDimensions((int)w + 1, (int)h + 1);
        Mask.transform.localPosition = new Vector3(x, y + 0.5f, 0);

        gameObject.transform.parent = target;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localScale = Vector3.one;
        if (!isTransformToParent)
        {
            gameObject.transform.parent = GuideManager.Instance.transform;
        }

        gameObject.SetActive(false);
        gameObject.SetActive(true);

        Vector3 centerpos = new Vector3((LeftTop.transform.localPosition.x - LeftBottom.transform.localPosition.x) / 2 + LeftBottom.transform.localPosition.x, LeftBottom.transform.localPosition.y + (RightTop.transform.localPosition.y - LeftTop.transform.localPosition.y) / 2, 0);
        ShowTarget(centerpos);
        if (word == "")
        {
            GuideManager.Instance.GuideSayWord.SetActive(false);
        }
        else
        {
            GuideManager.Instance.GuideSayWord.SetActive(true);
            GuideManager.Instance.GuideSayWord.GetComponent<GuideWord>().touchGuild.SetActive(true);
            GuideManager.Instance.GuideSayWord.GetComponent<GuideWord>().Show(word, isRotate, wordPos, true);

        }

        LeftTop.SetActive(false);
        RightTop.SetActive(false);
        LeftBottom.SetActive(false);
        RightBottom.SetActive(false);
        LeftTop.SetActive(true);
        RightTop.SetActive(true);
        LeftBottom.SetActive(true);
        RightBottom.SetActive(true);
        Mask.SetActive(true);

    }
}
