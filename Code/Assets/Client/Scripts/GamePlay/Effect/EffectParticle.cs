using UnityEngine;
using System.Collections;


public class EffectParticle : EffectBase
{
    protected GameObject m_EffectLib;
    private EventDelayManger delayManager;

    private EventDelayManger.EventDelay durationDelayEvent;
    private EventDelayManger.EventDelay destroyDelayEvent;

    public EffectParticle(GameObject owner, string name, string lib)
    {
        m_EffectOwner = owner;
        m_EffectName = name;
        m_LibName = lib;
        m_EffectType = BattleEffectType.E_BATTLE_EFFECT_TYPE_PARTICLE;
    }

    /*
    public override bool Init()
    {
        delayManager = EventDelayManger.GetInstance();
        return true;
    }
     * */

    public override bool LoadResource()
    {
		if(EffectManager.Instance.m_EffectGameObjectList.ContainsKey(m_EffectName)){
        	m_EffectLib = EffectManager.Instance.m_EffectGameObjectList[m_EffectName];
        }
        else
        {
            m_EffectLib = EffectManager.Instance.CreateGameObjectLib(m_EffectName);
        }
        delayManager = EventDelayManger.GetInstance();
        return true;
    }

    public override bool Play(GameObject target)
    {
        m_TargetObject = target;

        if (m_EffectLib == null)
        {
            Debug.LogError("Can't play effect! " + m_EffectName);
            if (EffectEnd != null)
            {
                state = EffectState.End;
                EffectEnd(this, m_TargetObject, 0f);
            }
            return false;
        }

        if (this.delayStartTime <= 0)
        {
            Play0();
        }
        else
        {
            //延迟播放;
            delayManager.CreateEvent(Play0, this.delayStartTime);
            state = EffectState.DelayStart;
        }
        return true;

    }

    public void Play0()
    {
        state = EffectState.Effecting;
        GameObject go = GameObject.Instantiate(m_EffectLib) as GameObject;
        ParticleSystem[] pss = go.GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem ps in pss)
        {
            foreach (Material material in ps.GetComponent<Renderer>().sharedMaterials)
            {
                if (material != null)
                {
                    material.renderQueue = 3200;
                }
            }
            ps.Play();
        }
        go.transform.parent = m_StartPos.parent;
        go.transform.localPosition = m_StartPos.transform.localPosition;
        go.transform.localScale = Vector3.one;
        go.transform.localEulerAngles = Vector3.zero;
  

        //如果速度不为0，则特效需要飞行，添加iTween事件
        if (m_MoveSpeed)
        {
            iTweenHandler.PlayToPos(go, m_StartPos.position,
                m_EndPos.position,
                durationTime,
                true);
            //计算旋转方向
            Vector3 direct = m_DirectionPoint - m_StartPos.transform.position;
            if (direct.x != 0f || direct.y != 0f)
            {
                direct.z = 0f;
                go.transform.up = direct;
            }
        }

        //动画播放完成;
        durationDelayEvent = delayManager.CreateEvent(AnimationCompleteHandler, this.durationTime);

        //删除对象;
        destroyDelayEvent = delayManager.CreateEvent(OnDesrtoyDuration, this.delaydestroyTime);

        //DestroyParticle dp = go.AddComponent<DestroyParticle>();
        //dp.particleCompleteDelegate += OnDesrtoyDuration;
        //dp.DestroyTime =  this.delaydestroyTime * 1000;

        m_EffectObj = go;
    }

    private void AnimationCompleteHandler()
    {
        if (durationDelayEvent != null)
        {
            delayManager.Delete(durationDelayEvent);
        }
        if (EffectEnd != null)
        {
            state = EffectState.End;
			EffectEnd(this, m_TargetObject, this.durationTime);
        }
    }


	private void OnDesrtoyDuration(){
        if (destroyDelayEvent != null)
        {
            delayManager.Delete(destroyDelayEvent);
        }
        if (m_EffectObj != null)
        {
            GameObject.Destroy(m_EffectObj);
		}
	}


    public override void Stop()
    {
        base.Stop();
    }

    public override void SetVisible(bool visible)
    {
        if (m_EffectObj != null)
        {
            m_EffectObj.SetActive(visible);
        }
    }

    public override EffectBase Duplicate()
    {
        EffectParticle effect = (EffectParticle)this.MemberwiseClone();
        return effect;
    }

    public override void Destroy()
    {
        if (m_EffectObj != null)
        {
            m_EffectObj.SetActive(false);
            GameObject.Destroy(m_EffectObj);
        }
    }
}