using UnityEngine;
using System.Collections;

public class EleTextEffect : MonoBehaviour {

    public float waitDuration = 0.5f;
    int currentCount = 0;

    public void ShowPopText(string textSpriteName)
    {
        currentCount++;
        StartCoroutine( _ShowPopText(textSpriteName));
    }

    private IEnumerator _ShowPopText(string textSpriteName)
    {
        if (currentCount > 1)
        {
            yield return new WaitForSeconds(waitDuration * (currentCount - 1));
        }
        else
        {
            ;
        }
        ShowPopTextDirectly(textSpriteName);
    }

    

    private void OnFinishTextEffect(GameObject go)
    {
        WidgetBufferManager.Instance.DestroyWidgetObj("Game/EleTextEffectPref", go);
        currentCount--;
    }


    public void ShowPopTextDirectly(string textSpriteName)
    {
        SoundEffect.Instance.PlaySound("cute_sound");
        GameObject obj = WidgetBufferManager.Instance.loadWidget("Game/EleTextEffectPref", this.transform);
        obj.GetComponent<UISprite>().spriteName = textSpriteName;
        obj.GetComponent<UISprite>().MakePixelPerfect();
        obj.GetComponent<TimeEventObject>().SetOnTimeFinished(OnFinishTextEffect);
        UITweener[] tweens = obj.GetComponents<UITweener>();
        foreach (UITweener tween in tweens)
        {
            tween.ResetToBeginning();
            tween.PlayForward();
        }
    }

    public void ShowCelerationDirectlyText(string textSpriteName)
    {
        SoundEffect.Instance.PlaySound("cute_sound");
        GameObject obj = WidgetBufferManager.Instance.loadWidget("Game/EleCelerationEffectPref", this.transform);
        obj.transform.Find("Icon").GetComponent<UISprite>().spriteName = textSpriteName;
        obj.transform.Find("Icon").GetComponent<UISprite>().MakePixelPerfect();
        obj.GetComponent<TimeEventObject>().SetOnTimeFinished(OnFinishCelerationEffect);
        UITweener[] tweens = obj.GetComponentsInChildren<UITweener>();
        foreach (UITweener tween in tweens)
        {
            tween.ResetToBeginning();
            tween.PlayForward();
        }
    }




    private void OnFinishCelerationEffect(GameObject go)
    {
        WidgetBufferManager.Instance.DestroyWidgetObj("Game/EleCelerationEffectPref", go);
    }


    public void ShowTextEffect(string m_score)
    {
        currentCount++;
        StartCoroutine(_ShowTextEffect(m_score));
    }

    private IEnumerator _ShowTextEffect(string m_score)
    {
        if (currentCount > 1)
        {
            yield return new WaitForSeconds(waitDuration * (currentCount - 1));
        }
        else
        {
            ;
        }
        ShowTextDirectlyEffect(m_score);
    }


    public void ShowTextDirectlyEffect(string m_score)
    {

        SoundEffect.Instance.PlaySound("cute_sound");
        GameObject obj = WidgetBufferManager.Instance.loadWidget("Game/Text", this.transform);
        UILabel scoreLabel = obj.GetComponentInChildren<UILabel>();
        scoreLabel.depth = UISquareView.fly_depth;
        scoreLabel.text = m_score;

        UITweener[] tweens = obj.GetComponentsInChildren<UITweener>();
        foreach (UITweener tween in tweens)
        {
            tween.ResetToBeginning();
            tween.PlayForward();
        }
        obj.GetComponent<TimeEventObject>().SetOnTimeFinished(OnFinishCelerationEffect);
    }

    private void OnFinisTextEffect(GameObject go){
        currentCount--;
        WidgetBufferManager.Instance.DestroyWidgetObj("Game/Text", go);
    }
}
