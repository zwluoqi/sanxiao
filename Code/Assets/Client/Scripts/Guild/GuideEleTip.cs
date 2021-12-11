using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuideEleTip : MonoBehaviour
{

    public GameObject LeftTop;
    public GameObject RightTop;
    public GameObject LeftBottom;
    public GameObject RightBottom;

    public float x;
    public float y;
    public float w;
    public float h;

    public GameObject TargetSign;
    public Animation swardSignAnimation;

    public GameObject MaskPref;
    private List<GameObject> maskList = new List<GameObject>();

    public void ShowTarget(Vector3 pos,SwipeDirection dic)
    {
        TargetSign.transform.localScale = Vector3.one;
        TargetSign.transform.position = pos;
        TargetSign.SetActive(true);
        swardSignAnimation.wrapMode = WrapMode.PingPong;
        if (dic == SwipeDirection.Left )
        {
            swardSignAnimation.Play("guildhenzuo");
        }
        else if (dic == SwipeDirection.Right)
        {
            swardSignAnimation.Play("guildhenyou");
        }
        else if (dic == SwipeDirection.Up)
        {
            swardSignAnimation.Play("guildshushang");
        }
        else if (dic == SwipeDirection.Down)
        {
            swardSignAnimation.Play("guildshuxia");
        }
        else
        {
            swardSignAnimation.Play("guildshushang");
        }
    }

    public void Hide()
    {
        transform.parent = GuideManager.Instance.transform;
        foreach (GameObject mask in maskList)
        {
            Destroy(mask);
        }
        maskList.Clear();
        gameObject.SetActive(false);
    }

    public void ShowForceEleTip(Transform masktarget, Rect rect, string word, Vector3 wordPos, List<UIWidget> maskWidgets,Transform target, bool isRotate,SwipeDirection dic)
    {
        x = rect.x; y = rect.y; w = rect.width; h = rect.height;
        LeftTop.transform.localPosition = new Vector3(x + w, y, 0.0f);
        RightTop.transform.localPosition = new Vector3(x + w, y - h, 0.0f);
        LeftBottom.transform.localPosition = new Vector3(x, y, 0.0f);
        RightBottom.transform.localPosition = new Vector3(x, y - h, 0.0f);

        //Mask.GetComponent<UISprite>().SetDimensions((int)w + 1, (int)h + 1);
        //Mask.transform.localPosition = new Vector3(x, y + 0.5f, 0);

        gameObject.transform.parent = masktarget.parent;
        gameObject.transform.localPosition = masktarget.transform.localPosition;
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.parent = GuideManager.Instance.transform;
        foreach (UIWidget widget in maskWidgets)
        {
            GameObject maskObj = GameObject.Instantiate(MaskPref) as GameObject;
            maskObj.transform.parent = widget.transform.parent;
            maskObj.GetComponent<UISprite>().SetDimensions(widget.width + 1, widget.height + 1);
            maskObj.transform.localPosition = widget.transform.localPosition;
            maskObj.transform.localScale = Vector3.one;
            maskObj.transform.parent = transform;
            maskObj.SetActive(true);
            maskList.Add(maskObj);
        }

        gameObject.SetActive(false);
        gameObject.SetActive(true);

        Vector3 centerpos = target.position;
        ShowTarget(centerpos, dic);
        if (word == "")
        {
            GuideManager.Instance.GuideSayWord.SetActive(false);
        }
        else
        {
            GuideManager.Instance.GuideSayWord.SetActive(true);
            GuideManager.Instance.GuideSayWord.GetComponent<GuideWord>().touchGuild.SetActive(false);
            GuideManager.Instance.GuideSayWord.GetComponent<GuideWord>().Show(word, isRotate, wordPos, true);
            GuideManager.Instance.GuideSayWord.transform.localPosition += new Vector3(0, -120, 0);

        }

        LeftTop.SetActive(false);
        RightTop.SetActive(false);
        LeftBottom.SetActive(false);
        RightBottom.SetActive(false);
        LeftTop.SetActive(true);
        RightTop.SetActive(true);
        LeftBottom.SetActive(true);
        RightBottom.SetActive(true);
        //Mask.SetActive(true);

    }
}
