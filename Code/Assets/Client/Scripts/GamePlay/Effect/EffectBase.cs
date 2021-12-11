using UnityEngine;
using System.Collections;


public enum BattleEffectType
{
    /// <summary>
    /// 序列帧
    /// </summary>
    E_BATTLE_EFFECT_TYPE_SEQUENCE   = 0,
    /// <summary>
    /// 粒子特效
    /// </summary>
    E_BATTLE_EFFECT_TYPE_PARTICLE   = 1,
}

public enum EffectState
{
    None,
    DelayStart,
    Effecting,
    End,
}

//特效基类
public class EffectBase
{
    protected BattleEffectType m_EffectType;	//特效类型	序列帧 和 粒子
    public GameObject m_EffectOwner;			//特效所有者
    protected string m_EffectName;				//特效名称
    protected string m_LibName;					//特效库
    protected GameObject m_TargetObject;			//目标对象

    protected GameObject m_EffectObj;
    protected Transform m_StartPos;
    public Transform StartPos { set { m_StartPos = value; } }//特效开始位置;
    protected Transform m_EndPos;
    public Transform EndPos { set { m_EndPos = value; } }//特效结束位置;
    protected Vector3 m_DirectionPoint;
    public Vector3 DirectionPoint { set { m_DirectionPoint = value; } }//特效方向;

    protected bool m_MoveSpeed = false;
    public bool MoveSpeed { set { m_MoveSpeed = value; } }

    public float durationTime = 0;
    public float delaydestroyTime = 0;
    public float delayStartTime = 0;

    public delegate void EffectStartEventHandler(EffectBase effect, GameObject target);
    public delegate void EffectEndEventHandler(EffectBase effect, GameObject target, float total_time);
    public delegate void EffectUpdateEventHandler(EffectBase effect, GameObject target, float total_time, float cur_time);

    public EffectStartEventHandler EffectStart;
    public EffectEndEventHandler EffectEnd;
    public EffectUpdateEventHandler EffectUpdate;

    public EffectState state;
    protected virtual void OnEffectStart(EffectBase effect) { }
    protected virtual void OnEffectEnd(EffectBase effect, float total_time) { }
    protected virtual void OnEffectUpdate(EffectBase effect, float cur_time) { }

    public virtual bool Init() { return false; }
    public virtual bool LoadResource() { return false; }

    public virtual bool Play(GameObject target) { return false; }
    public virtual void Stop() {  }
    public virtual void SetVisible(bool visible) { }

    public virtual EffectBase Duplicate() { return null; }
    public virtual void Update() { }
    public virtual void Destroy() { }
}


