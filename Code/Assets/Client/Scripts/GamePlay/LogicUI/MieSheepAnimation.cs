using UnityEngine;
using System.Collections;

public class MieSheepAnimation : MonoBehaviour {

    public TweenScale tsSheep;
    public TweenPosition tpSheep;
    public TweenScale tpReady;
    public TweenScale tpGo;

    [HideInInspector]
    public bool playCompleted = false;


    public void Init()
    {
        tpReady.transform.localScale = Vector3.zero;
        tpGo.transform.localScale = Vector3.zero;
    }


    public void PlayAnimation()
    {
        playCompleted = false;
        tpSheep.SetOnFinished(PlaySheepFinished);
        tpSheep.PlayForward();
        tsSheep.PlayForward();
    }

    public void PlaySheepFinished()
    {
//        SoundEffect.Instance.PlaySound("ready");
        tpReady.ResetToBeginning();
        tpReady.PlayForward();
    }

    public void PlayReadyFinished()
    {
        tpReady.transform.localScale = Vector3.zero;
        SoundEffect.Instance.PlaySound("go");
        tpGo.ResetToBeginning();
        tpGo.PlayForward();
    }

    public void PlayGoFinished()
    {
        tpSheep.SetOnFinished(SheepFinished);
        tpSheep.PlayReverse();
        tsSheep.PlayReverse();
    }

    public void SheepFinished()
    {
        playCompleted = true;
    }

}
