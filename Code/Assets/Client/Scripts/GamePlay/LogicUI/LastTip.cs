using UnityEngine;
using System.Collections;

public class LastTip : MonoBehaviour {

    public UILabel num;
    public UILabel mode;
    public GameObject zhu;

    public void Reset()
    {
        if (zhu.GetComponent<TweenPosition>() != null)
        {
            zhu.transform.localPosition = zhu.GetComponent<TweenPosition>().from;
        }
    }

    public void PlayAnimation(int numValue)
    {
        if (LevelData.type == CopyType.MoveLimit)
        {
            mode.text = LanguageManger.GetMe().GetWords("L_1047");
        }else{
            mode.text = LanguageManger.GetMe().GetWords("L_1048");
        }
        num.text = numValue.ToString();

        UITweener[] zhuut = zhu.GetComponentsInChildren<UITweener>();

        zhuut[0].SetOnFinished(FinishedShow);
        foreach (UITweener ut in zhuut)
        {
            ut.PlayForward();
        }
    }

    public void FinishedShow()
    {
        Invoke("StartHide", 2);
    }

    public void StartHide()
    {
        UITweener[] zhuut = zhu.GetComponentsInChildren<UITweener>();
        if (zhuut != null && zhuut.Length > 0)
        {
            zhuut[0].onFinished.Clear();

            foreach (UITweener ut in zhuut)
            {
                ut.PlayReverse();
            }

        }
    }
}
