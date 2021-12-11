using UnityEngine;
using System.Collections;

public class UIFlyItem  {
    public Vector3 m_objWordPos;
    public Vector3 m_targetIconWorldPos;
    public string m_spriteName;
    public string m_prefabName;
    public AnimationCurve animationCurve;

    private GameObject obj;

    public UIFlyItem(Vector3 objWorldPos, Vector3 targetIconWorldPos, string spriteName, string prefabName)
    {
        this.m_objWordPos = objWorldPos;
        this.m_targetIconWorldPos = targetIconWorldPos;
        this.m_spriteName = spriteName;
        this.m_prefabName = prefabName;
    }

    public void Play()
    {
        if (m_targetIconWorldPos != Vector3.zero)
        {
            //在此基础上创建小对象,小对象向任务图标移动,并在到达后消失;
            obj = WidgetBufferManager.Instance.loadWidget(m_prefabName, EleUIController.Instance.missionAnimation.transform);
            obj.transform.position = m_objWordPos;
            UISprite sprite = obj.GetComponent<UISprite>();
            sprite.spriteName = m_spriteName;
            sprite.depth = UISquareView.fly_depth;

            FlyItemAnmationCurve flyCurve = obj.GetComponent<FlyItemAnmationCurve>();
            flyCurve.Play(AnimationCurveType.QUXIAN, obj.transform.parent.InverseTransformPoint(m_targetIconWorldPos));
            float duration = flyCurve.curves[(int)AnimationCurveType.QUXIAN].duration;
            EventDelayManger.Instance.CreateEvent(PlayCompleted, duration);
        }
        Play0();
    }


    public virtual void Play0()
    {

        SystemConfig.Log("flying" + Time.realtimeSinceStartup);
    }

    public void PlayCompleted()
    {
        PlayCompleted0();
        FlyItemAnmationCurve flyCurve = obj.GetComponent<FlyItemAnmationCurve>();
        flyCurve.Stop();
        WidgetBufferManager.Instance.DestroyWidgetObj(m_prefabName, obj);
        obj = null;
    }

    public virtual void PlayCompleted0()
    {
        SystemConfig.Log("flyed");
    }
}
