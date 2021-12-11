using UnityEngine;
using System.Collections;

public class MissionAnimation : MonoBehaviour {
    public UISlider missionIconBar;
    public GameObject thumb;
    public AnimationCurve curveData;
    public float duration = 0.5f;
    public float defaultValue = 0.004f;
    public float startLevelValue = 0.337f;

    [HideInInspector]
    public bool ProMissionAnimationCompleted = false;

    void Awake()
    {
        UIEventListener.Get(thumb).onDrag += OnDragThumb;
        UIEventListener.Get(thumb).onPress += OnPressThumb;
    }

    public void Reset()
    {
        missionIconBar.value = startLevelValue;
    }

    public void PlayMoveTop()
    {
        ProMissionAnimationCompleted = false;
        StartCoroutine(PlayStartAnimation());
    }

    private void OnDragThumb(GameObject go, Vector2 dic)
    {
        if (dic.y > 0)
        {
            SystemConfig.Log("up");

        }
        else if (dic.y < 0)
        {
            SystemConfig.Log("down");

        }
        float barValue = missionIconBar.value - dic.y / missionIconBar.foregroundWidget.height;
        barValue = Mathf.Clamp(barValue, defaultValue, 1f);
        missionIconBar.value = barValue;

    }

    private void OnPressThumb(GameObject go, bool press)
    {
        if (press)
        {
            StopAllCoroutines();
            Debug.Log("reset drag");
        }
        else
        {
            Debug.Log("compelet drag");
            StartCoroutine(MoveTop());
        }
    }

    private IEnumerator PlayStartAnimation()
    {
        float _startValue = startLevelValue;
        float distance = defaultValue - startLevelValue;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            missionIconBar.value = _startValue + distance * curveData.Evaluate(timer / duration);
            yield return null;
        }
        missionIconBar.value = _startValue + distance;
        ProMissionAnimationCompleted = true;
    }

    private IEnumerator MoveTop()
    {
        float _startValue = missionIconBar.value ;
        float distance = defaultValue - missionIconBar.value;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            missionIconBar.value = _startValue + distance * curveData.Evaluate(timer / duration);
            yield return null;
        }
        missionIconBar.value = _startValue + distance;
        ProMissionAnimationCompleted = true;
    }
}
