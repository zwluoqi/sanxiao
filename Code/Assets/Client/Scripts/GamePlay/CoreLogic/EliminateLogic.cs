using UnityEngine;
using System.Collections;

public class EliminateLogic : MonoBehaviour {
    public float moveinDeltaTime = 0.2f;//移动交换元素的时间;
    public float dropDeltaTime = 0.2f;//元素掉落的时间;
    public float createDeltaTime = 1.0f;//元素创建由小变大的时间;
    //public float flyParticleDeltaTime = 0.4f;//飞行特效飞行的时间;
    public float dropedSquareDeltaTime = 1.5f;//掉落的方块消失延迟时间;
    //public float xuanzhuanDeltaTime = 3f;//万能元素旋转时间
    public float FlyDeltaTime = 0.1f;//飞行元素之间的间隔;
    public float hammerDeltaTime = 0.3f;//勺子动画播放时间
    public float localTipMoveDeltaTime = 0.7f;

    public const int flyParticleAnimationID = 6;//飞行特效ID
    public const int xunzhuanParticleAnimationID = 7;//旋转特效ID


	private static EliminateLogic m_Instance;
	public static EliminateLogic Instance{get{return m_Instance;}}

    private EliminatePlayer m_Player;
    public EliminatePlayer GetEliminatePlayer() { return m_Player; }
	
	void Awake(){
		m_Instance = this;
        m_Player = new EliminatePlayer(this);
	}

	public void StartEleminate(){
		SystemConfig.LogWarning("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! EliminateLogic.StartEleminate()!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        if (!m_Player.Init())
        {
            Debug.LogError("Init Failed！");
        }
	}
	
	void Update(){
        if (m_Player.gaming)
        {
            m_Player.Update(Time.deltaTime);
		}
	}

    public void EndEleminate()
    {
        m_Player.OnDestroy();
        CancelInvoke();
        SystemConfig.LogWarning("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! EliminateLogic.EndEleminate()!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }
	
	void TimeTick(){
		//如果任务模式为时间模式,则减少;
        if (LevelData.type == CopyType.TimeLimit)
        {
			EleUIController.Instance.limitAmount--;
			if(EleUIController.Instance.limitAmount == 0){
                m_Player.CheckWin();
			}
		}

	}		
	
	public void StartTimeTick(){
		//开启沙漏;
		m_Player.starttimeTick = true;
		EliminateLogic.Instance.InvokeRepeating("TimeTick",1,1);		
	}
	
	public void StopTimeTick(){
		//取消沙漏;
		m_Player.starttimeTick = false;
		EliminateLogic.Instance.CancelInvoke("TimeTick");
	}

    public void StartEleCheckHits()
    {
        StartTimeTick();
        m_Player.GetEliminateProcedureManager().ChangProcedure(EliminateProcedureType.PROCEDURE_CHECK_HITS);
    }
}
