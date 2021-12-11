using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;


public sealed class EliminatePlayer
{
    private EliminateLogic m_EliminateLogic = null;
	private EliminateProcedureManager m_ProcedureManager = null;
	
	//LevelData
	public List<UIEliminateItemView> hintItems = null;
    public SwipeDirection diction = SwipeDirection.Up;
	
	//equipData
	public EquipEffectType useEquipType = EquipEffectType.Invalid;
	public UIEliminateItemView useEquipSelectItem = null;
	public UIEliminateItemView useEquipSelectOtherItem = null;
	//buydata
	public bool buy = false;
	
	//time data
	public bool starttimeTick = false;
	public float gameTime = 0;
	
	public int getGoldNum = 0;
	public int getZhuanshiNum = 0;
	public int getPower = 0;

    //飞行特效;
    public EffectBase flyIns;
    public EffectBase xuanZhuanIns;

    public bool produceMapCompleted = false;
    public int eliminatingCount;

    public bool gaming = false;
    public EliminatePlayer(EliminateLogic logic)
    {
        m_EliminateLogic = logic;
		m_ProcedureManager = new EliminateProcedureManager(this);
        EventManager.Instance.RegisterEventHandler(EventDefine.Hammer, UseHammer);
        EventManager.Instance.RegisterEventHandler(EventDefine.Step, UseStep);
        EventManager.Instance.RegisterEventHandler(EventDefine.desStep, UsedesStep);
        EventManager.Instance.RegisterEventHandler(EventDefine.add_time_30s, Useadd_time_30s);
        EventManager.Instance.RegisterEventHandler(EventDefine.Bomb_SameCor, UseBomb_SameCor);
        EventManager.Instance.RegisterEventHandler(EventDefine.ResetItem, UseResetItem);
        EventManager.Instance.RegisterEventHandler(EventDefine.BombRow, UseBombRow);
        EventManager.Instance.RegisterEventHandler(EventDefine.BombCol, UseBombCol);
        EventManager.Instance.RegisterEventHandler(EventDefine.Exchange, ExchangeEle);
        EventManager.Instance.RegisterEventHandler(EventDefine.BombEffect, UseBombEffect);
    }

    public bool Init()
    {
        if (!m_ProcedureManager.Init())
        {
            Debug.LogError("消除流程初始化错误！");
            return false;
        }

        EffectManager.Instance.Init(this);
        InitPlayer();
        EleUIController.Instance.EnterGame();
        return true;
    }
    
    public void Update(float deltaTime)
    {
        EventManager.Instance.Update(deltaTime);
        EventDelayManger.GetInstance().OrderedUpdate(deltaTime);
        m_ProcedureManager.Update(deltaTime);
    }
	

	public void InitPlayer(){
		int currentLevel = LocalDataBase.Instance().GetSelectCopyLevel();
		SystemConfig.LogWarning ("current level:"+currentLevel);
        //GA.StartLevel("level" + currentLevel);

		buy = false;
		hintItems = null;
		
		//equipData
		useEquipType = EquipEffectType.Invalid;
		useEquipSelectItem = null;
		useEquipSelectOtherItem = null;

		//buydata
		buy = false;
		
		//time data
		starttimeTick = false;
		gameTime = 0;
		
		getGoldNum = 0;
		getZhuanshiNum = 0;
		getPower = 0;

        produceMapCompleted = false;


        //if (LocalDataBase.copyModels[14].star >= 0)
        //{
        //    EleUIController.Instance.equipController.gameObject.SetActive(true);
        //    EleUIController.Instance.InitEquipView((CopyType)TableManager.GetCopydetailByID(currentLevel).CopyType);
        //}
        //else
        //{
        //    EleUIController.Instance.equipController.gameObject.SetActive(false);
        //}
        EleUIController.Instance.InitEquipView((CopyType)TableManager.GetCopydetailByID(currentLevel).CopyType);

        EventDelayManger.GetInstance().Init();

        gaming = true;
	}
	

	
    public void OnDestroy()
    {
        gaming = false;
        EventDelayManger.GetInstance().Close();
    }
	
	public EliminateProcedureManager GetEliminateProcedureManager(){
		return m_ProcedureManager;	
	}
	
	
	//触发事件;
	private void UseHammer(EventDefine type, System.Object[] args){
		useEquipType = EquipEffectType.Hammer;
		useEquipSelectItem = args[0] as UIEliminateItemView;
		m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_ELIMINATING);
	}
	
	private void UseStep(EventDefine type, System.Object[] args){
		EleUIController.Instance.limitAmount += 5;

	}
	
	private void UsedesStep(EventDefine type, System.Object[] args){
		EleUIController.Instance.limitAmount -= 5;

	}
	
	private void Useadd_time_30s(EventDefine type, System.Object[] args){
		EleUIController.Instance.limitAmount += 30;

	}
	
	private void UseBomb_SameCor(EventDefine type, System.Object[] args){
		useEquipType = EquipEffectType.Bomb_SameCor;
		useEquipSelectItem = args[0] as UIEliminateItemView;
		m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_ELIMINATING);
		
	}
	
	private void UseResetItem(EventDefine type, System.Object[] args){

		m_ProcedureManager.ChangProcedure(EliminateProcedureType.EliminateProcedureEquipReset);

	}
	
	private void UseBombRow(EventDefine type, System.Object[] args){
		useEquipType = EquipEffectType.BombRow;
		useEquipSelectItem = args[0] as UIEliminateItemView;
		m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_ELIMINATING);

	}	
	
	private void UseBombCol(EventDefine type, System.Object[] args){
		useEquipType = EquipEffectType.BombCol;
		useEquipSelectItem = args[0] as UIEliminateItemView;
		m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_ELIMINATING);

	}		
	
	private void ExchangeEle(EventDefine type, System.Object[] args){
		useEquipType = EquipEffectType.Exchange;
		useEquipSelectItem = args[0] as UIEliminateItemView;
		useEquipSelectOtherItem = args[1] as UIEliminateItemView;
		m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_ELIMINATING);
	}

    private void UseBombEffect(EventDefine type, System.Object[] args)
    {
        useEquipType = EquipEffectType.BomEffect;
        useEquipSelectItem = args[0] as UIEliminateItemView;
        m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_ELIMINATING);
    }

    public void StartGeneMap()
    {
        produceMapCompleted = false;
    }

    public void EndGeneMap()
    {
        produceMapCompleted = true;
    }

	public void ReturnToMain()
    {
        #region guild
        if (LocalDataBase.equipGuild)
        {
            LocalDataBase.equipGuild = false;
        }
        #endregion
        //清空地图数据;
        Map.Instance.MapOver();

        //预制缓存
        WidgetBufferManager.Instance.ClearObjs();

        //特效缓存
        EffectManager.Instance.DestroyEffect();

        //音效缓存
        SoundEffect.Instance.ClearSounds();

        Resources.UnloadUnusedAssets();

        //清空游戏临时道具;
        LocalDataBase.Instance().ClearTmpData();
        //UI
        EleUIController.Instance.ExitGame();
		//进入主界面;
		SceneManager.Instance.GameToUI();
	}
	
	public void ReStartLevel(){
        LocalDataBase.Instance().DecreaseDataNum(DataType.zhuanshi, 5);
        //清空地图数据;
        Map.Instance.MapOver();
        //清空游戏临时道具;
        LocalDataBase.Instance().ClearTmpData();

        Init();
	}

	public void CheckWin(){
        if (LevelData.type == CopyType.MoveLimit)
        {
			if(MissionManager.Instance.limitAmount <= 0 && !MissionManager.Instance.IsWin())
			{
				m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_PREDEFEATED);
			}
			else if(MissionManager.Instance.limitAmount >=0 && MissionManager.Instance.IsWin())
			{
				m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_PREVICTORY);
			}
			else
			{
				SystemConfig.LogWarning("Goon the Game");
				m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_CHECK_HITS);
			}
		}else{
			if(MissionManager.Instance.limitAmount <=0){
				if(MissionManager.Instance.IsWin()){
					m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_PREVICTORY);
				}else{
					m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_PREDEFEATED);
				}
			}else{
				SystemConfig.LogWarning("Goon the Game");
				m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_CHECK_HITS);			}
		}
	}
}

