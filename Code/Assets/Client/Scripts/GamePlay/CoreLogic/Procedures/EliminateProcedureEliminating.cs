using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class EliminateProcedureEliminating:EliminateProcedureBase
{
	private EliminateProcedureManager m_ProcedureManager = null;
	private EleUIController uiController = null;
	private EliminatePlayer m_player = null;
	
	private bool eliminate = false;
	private bool normalEliminate = false;

    private List<EventDelayManger.EventDelay> tfyxMoveDelayList = new List<EventDelayManger.EventDelay>();
    private List<EventDelayManger.EventDelay> tfdrMoveDelayList = new List<EventDelayManger.EventDelay>();

    public enum TFYXState{
        NONE,
        CANMOVE,
        ENDPOINT,
        DEAD,
    }

    private TFYXState tfyxState = TFYXState.NONE;

	private UIEliminateItemView one;
	private UIEliminateItemView other;

    public override EliminateProcedureType GetProcedureType(){
		return EliminateProcedureType.PROCEDURE_ELIMINATING;
	
	}
	
	public enum SwapType{
		Normal,
		Miracle2Miracle,
		Miracle2Other,
		Specital2Specital,
	}
	
	
	
    public override bool Init(EliminateProcedureManager manager){
		SystemConfig.Log("PROCEDURE_ELIMINATING Init");
		m_ProcedureManager = manager;
		
		m_player = manager.GetEliminatePlayer();
		return true;
	}
	
    public override void OnEnter(){
		SystemConfig.Log("PROCEDURE_ELIMINATING OnEnter");
		//
		uiController = EleUIController.Instance;
		eliminate = false;
        tfyxState = TFYXState.NONE;
		Map.Instance.removedSquareIDList = new List<int>();
        Map.Instance.removedELeIDList = new List<int>();
        Map.Instance.removedMissionIDList = new Dictionary<int, int>();
        Map.Instance.removedMissionCombineTimes = 0;
		UnHint();
		if(m_player.useEquipType != EquipEffectType.Invalid){
			one = m_player.useEquipSelectItem;
			other = m_player.useEquipSelectOtherItem;
		}else{
			one = uiController.selectedItem;
			other = uiController.targetItem;
		}
        one.OnSelected(false);
        m_player.eliminatingCount = 0;
        tfyxMoveDelayList.Clear();
        tfdrMoveDelayList.Clear();
	}
	
    public override void OnLeave(){
		SystemConfig.Log("PROCEDURE_ELIMINATING OnLeave");
        uiController.ResetSelect();
		m_player.useEquipType = EquipEffectType.Invalid;
		m_player.useEquipSelectItem = null;
		m_player.useEquipSelectOtherItem = null;
        eliminate = false;
        tfyxState = TFYXState.NONE;
        foreach (EventDelayManger.EventDelay delay in tfyxMoveDelayList)
        {
            EventDelayManger.Instance.Delete(delay);
        }
        tfyxMoveDelayList.Clear();
        foreach (EventDelayManger.EventDelay delay in tfdrMoveDelayList)
        {
            EventDelayManger.Instance.Delete(delay);
        }
        tfdrMoveDelayList.Clear();
	}

    public override void Update(float deltaTime)
    {
		if(!eliminate){
			eliminate = true;
			if(m_player.useEquipType == EquipEffectType.Invalid){
				SystemConfig.Log(string.Format("{0} swap {1}",one.name,other.name));
				one.SwapItem(other
					,OnSwapCallBack);
				normalEliminate = true;
            }
            else if (m_player.useEquipType != EquipEffectType.Invalid && one != null)
            {
                #region guild
                if (LocalDataBase.equipGuild)
                {
                    LocalDataBase.equipWaitInput++;
                }
                #endregion

				if(m_player.useEquipType == EquipEffectType.Hammer){
                    //todo
                    float effectDuration = EquipManager.Instance.PlayAnimation(m_player.useEquipType, one.transform);
                    EventDelayManger.Instance.CreateEvent(() => { Map.Instance.RemoveOneSelectedItem(one, OnEliminateNotCheck); }, effectDuration);
				}else  if(m_player.useEquipType == EquipEffectType.Bomb_SameCor){
                    Map.Instance.RemoveAllItemsBySameColor(one.tab_id, OnEliminateNotCheck);
				}else if(m_player.useEquipType == EquipEffectType.BombRow){
                    float effectDuration = EquipManager.Instance.PlayAnimation(m_player.useEquipType, one.transform);
                    EventDelayManger.Instance.CreateEvent(() => { Map.Instance.RemoveRowOrColItems(one, OnEliminateNotCheck, m_player.useEquipType); }, effectDuration);
				}else if(m_player.useEquipType == EquipEffectType.BombCol){
                    float effectDuration = EquipManager.Instance.PlayAnimation(m_player.useEquipType, one.transform);
                    EventDelayManger.Instance.CreateEvent(() => { Map.Instance.RemoveRowOrColItems(one, OnEliminateNotCheck, m_player.useEquipType); }, effectDuration);
				}else if(m_player.useEquipType == EquipEffectType.Exchange){
					one.SwapItem(other
					             ,OnSwapCallBack);
				}
                else if (m_player.useEquipType == EquipEffectType.BomEffect)
                {
                    float effectDuration = EquipManager.Instance.PlayAnimation(m_player.useEquipType, one.transform);
                    EventDelayManger.Instance.CreateEvent(() => { Map.Instance.RemoveRowOrColItems(one, OnEliminateNotCheck, m_player.useEquipType); }, effectDuration);
                }
                else {
					SystemConfig.LogError("use equip error!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				}
				normalEliminate = false;
			}else{
				SystemConfig.LogError("eliminating error!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			}
		}
	}
	
	//第一次交换回调;
	public void OnSwapCallBack(){
		SystemConfig.Log("first swap back");
		
		//AffectType
		//元素消除对周围的影响（-1，不影响，0周围爆炸影响。1横排影响。2纵排影响。3.交换消除同色）
		
		UIEliminateItemView miracleSwap = null;
		UIEliminateItemView otherSwap = null;
		SwapType swapType = SwapType.Normal;
		if(one.tab_ele.AffectType == 3
		   && other.tab_ele.AffectType != 3){
			swapType = SwapType.Miracle2Other;
			
			miracleSwap = one;
			otherSwap = other;
			
		}else if(other.tab_ele.AffectType == 3
		         && one.tab_ele.AffectType != 3){
			swapType = SwapType.Miracle2Other;
			
			miracleSwap = other;
			otherSwap = one;
		}else if(one.tab_ele.AffectType == 3
		         && other.tab_ele.AffectType == 3){
			swapType = SwapType.Miracle2Miracle;		
		}else if((one.tab_ele.AffectType >= 0 && other.tab_ele.AffectType <= 2)
		         && (other.tab_ele.AffectType >= 0 && other.tab_ele.AffectType <= 2)){
			swapType = SwapType.Specital2Specital;
		}else{
			swapType = SwapType.Normal;
		}

        bool canCombine = false;
		switch(swapType){
			case SwapType.Normal:
                if (Map.Instance.GetCheckedCombines().Count > 0)
                {
                    canCombine = true;
					Map.Instance.EliminateFromCheck(OnEliminateCombineCallBack);
				}else{
					if(m_player.useEquipType == EquipEffectType.Exchange){//强制交换道具
						OnSwapBackCallBack();
					}else{
						one.SwapItem(other
							,OnSwapBackCallBack);
					}
				}
				break;
			case SwapType.Miracle2Miracle:
                canCombine = true;
				Map.Instance.RemoveAllItems(OnEliminateNotCheck);
				break;
			case SwapType.Specital2Specital:
                canCombine = true;
				Map.Instance.RemoveItemsBySpecital(OnEliminateNotCheck,one,other);
				break;
			case SwapType.Miracle2Other:
                canCombine = true;
				Map.Instance.ChangedToSameTypeAndRemove(OnEliminateNotCheck,miracleSwap,otherSwap);
				break;
			
		}
        if (normalEliminate && canCombine)
        {
            if (LevelData.type == CopyType.MoveLimit)
            {
                EleUIController.Instance.limitAmount--;
                LevelData.guildWaitInputCount++;
            }
        }
		
	}
	
	
	//非检测消除回调;
	public void OnEliminateNotCheck(){
		 Map.Instance.EliminateFromCheck(OnEliminateCombineCallBack);
	}

    
	//检测消除回调,塔防英雄开始移动;
	public void OnEliminateCombineCallBack(){


        Map.Instance.CalculateCurrentEleAndSqu();
        Map.Instance.AddSquareWithSameId();
        Map.Instance.MoveSquareToOtherPos();
        if (LevelData.mode == CopyMode.TAFANG)
        {
            TFYXMove();
        }
        else
        {
            EliminateAgain();
        }

	}

    public void TFYXMove()
    {
        int TFYXMoveCount = Map.Instance.TFYXCanMove();
        if (TFYXMoveCount > 0)
        {
            tfyxState = TFYXState.CANMOVE;
            int i = 0;
            for (; i < TFYXMoveCount; i++)
            {
                EventDelayManger.EventDelay tfyxMove = EventDelayManger.GetInstance().CreateEvent(TFYXMove0, EliminateLogic.Instance.moveinDeltaTime * 2 * i);
                tfyxMoveDelayList.Add(tfyxMove);
            }
            EventDelayManger.GetInstance().CreateEvent(TFDRMove, EliminateLogic.Instance.moveinDeltaTime * 2 * i);
        }
        else
        {
            TFDRMove();
        }
    }

    public void TFYXMove0()
    {
        if (tfyxState != TFYXState.ENDPOINT)
        {
            bool end = Map.Instance.TFYXMove();
            if (end)
            {
                tfyxState = TFYXState.ENDPOINT;
            }
        }
    }

    /// <summary>
    /// 敌人攻击模式开启
    /// </summary>
    public void TFDRMove()
    {
        if(tfyxState == TFYXState.ENDPOINT){
            EliminateAgain();
        }else{
            List<AttackModel> drAttacker;
            Map.Instance.TFDRmove(out drAttacker);
            if (drAttacker.Count > 0)
            {
                UISquareView yx = Map.Instance.GetTFYX();

                //塔防英雄被干死;
                if (yx.square_hp <= drAttacker.Count)
                {
                    tfyxState = TFYXState.DEAD;
                }

                int i = 0;
                foreach (AttackModel dr in drAttacker)
                {
                    EventDelayManger.EventDelay tfdrMove = EventDelayManger.GetInstance().CreateEvent(dr.Cast0, i * EliminateLogic.Instance.moveinDeltaTime * 2);
                    tfdrMoveDelayList.Add(tfdrMove);
                    i++;
                }
                EventDelayManger.GetInstance().CreateEvent(EliminateAgain, (i) * EliminateLogic.Instance.moveinDeltaTime * 2);
            }
            else
            {
                EliminateAgain();
            }
        }
    }

    /// <summary>
    /// 敌人攻击完毕
    /// </summary>
    public void EliminateAgain()
    {
        Map.Instance.EliminateFromCheck(OnCombineCallBack);
    }

    //塔防英雄,敌人动态变化后再次检测后回调
    public void OnCombineCallBack()
    {
        CheckWin();
    }
	
	//第二次交换回调;
	public void OnSwapBackCallBack(){
		SystemConfig.Log("second swap back");
		m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_WAITING_INPUT);
	}
	
	
	/// <summary>
	/// un-hint.
	/// </summary>
	public void UnHint()
	{
		//Remove effect of hint items
		for(int i=0;i<m_player.hintItems.Count;i++){
			if(m_player.hintItems[i] != null)
				m_player.hintItems[i].IdleAnimation();
		}
	}	
	
	/// <summary>
	/// Check win or loose or goon
	/// </summary>
	public void CheckWin()
    {
        #region guild
        if(LevelData.mode == CopyMode.TAFANG){
            if (tfyxState == TFYXState.CANMOVE )
            {
                if (LevelData.needTFGuild)
                {
                    List<UISquareView> tfyxItem = Map.Instance.GetSquares(LevelData.tfData.tfyxID);
                    UISquareView tfyx = tfyxItem[0];

                    bool hasGuild = false;
                    if (LevelData.currentLevel == 1)
                    {
                        if (LevelData.guildTFCount == 1)
                        {
                            GuideManager.Instance.ShowGuideTarget(tfyx.GetComponent<UIWidget>(),false, LanguageManger.GetMe().GetWords("L_tfmove_tishi5"), new Vector3(0,-400,0));
                            UIEventListener.Get(GuideManager.Instance.m_GuildTarget.gameObject).onClick = OnCommonYinDaoCallBack;
                        }
                        else
                        {
                            GuideManager.Instance.ShowGuideTarget(tfyx.GetComponent<UIWidget>(), false, string.Format(LanguageManger.GetMe().GetWords("L_tfmove_tishi6"), Map.Instance.tafangPath.Count), new Vector3(0,-400,0));
                            UIEventListener.Get(GuideManager.Instance.m_GuildTarget.gameObject).onClick = OnCommonYinDaoCallBack;
                        }
                        hasGuild = true;
                    }
                    else if (LevelData.currentLevel >= 3 && LevelData.currentLevel <= 4)
                    {
                        if (LevelData.guildTFCount == 2)
                        {
                            Queue<TipWord> words = new Queue<TipWord>();
                            words.Enqueue(new TipWord(LanguageManger.GetMe().GetWords("L_xiaochu_tishi11"), new Vector3(0,-400,0)));
                            GuideManager.Instance.ShowGuildTip(words);
                            GuideManager.Instance.Guildtip.GetComponent<TipGuide>().lastWordTipOnClick = EliStateChange;
                            hasGuild = true;
                        }
                        else
                        {
                            hasGuild = false;
                        }
                    }
                    else if (LevelData.currentLevel == 5)
                    {
                        if (LevelData.guildTFCount == 2)
                        {
                            Queue<TipWord> words = new Queue<TipWord>();
                            words.Enqueue(new TipWord(LanguageManger.GetMe().GetWords("L_xiaochu_tishi11"), new Vector3(0,-400,0)));
                            words.Enqueue(new TipWord(LanguageManger.GetMe().GetWords("L_xiaochu_tishi8"), new Vector3(0, 0, 0)));
                            GuideManager.Instance.ShowGuildTip(words);
                            GuideManager.Instance.Guildtip.GetComponent<TipGuide>().lastWordTipOnClick = EliStateChange;
                            hasGuild = true;
                        }
                        else
                        {
                            hasGuild = false;
                        }
                    }
                    else
                    {
                        if (LevelData.guildTFCount == 2)
                        {
                            GuideManager.Instance.ShowGuideTarget(tfyx.GetComponent<UIWidget>(),false, string.Format(LanguageManger.GetMe().GetWords("L_tfmove_tishi6"), Map.Instance.tafangPath.Count), new Vector3(0,-400,0));
                            UIEventListener.Get(GuideManager.Instance.m_GuildTarget.gameObject).onClick = OnCommonYinDaoCallBack;
                            hasGuild = true;
                        }
                        else
                        {
                            hasGuild = false;
                        }
                    }

                    if (hasGuild)
                    {
                        return;
                    }
                }
            }
        }else{
            if (LevelData.lastMissionGuild)
            {
                if (LevelData.currentLevel == 1)
                {
                    UIMissionView firstMission = EleUIController.Instance.m_MisIcon[0];
                    Mission requestMission = LevelData.GetMissionByID(firstMission.m_ntype);
                    Mission completedMission = MissionManager.Instance.GetMissionByID(firstMission.m_ntype);
                    int remianedReq = requestMission.amount - completedMission.amount;

                    GuideManager.Instance.ShowGuideTarget(firstMission.GetComponent<UIWidget>(),false, string.Format(LanguageManger.GetMe().GetWords("L_tfmove_tishi7"), remianedReq, requestMission.missionName), new Vector3(0,-400,0));
                    UIEventListener.Get(GuideManager.Instance.m_GuildTarget.gameObject).onClick = OnCommonYinDaoCallBack;
                }
                else if(LevelData.currentLevel == 5)
                {
                    Queue<TipWord> words = new Queue<TipWord>();
                    words.Enqueue(new TipWord(LanguageManger.GetMe().GetWords("L_xiaochu_tishi11"),new Vector3(0,-400,0)));
                    words.Enqueue(new TipWord(LanguageManger.GetMe().GetWords("L_xiaochu_tishi8"), new Vector3(0, 0, 0)));
                    GuideManager.Instance.ShowGuildTip(words);
                    GuideManager.Instance.Guildtip.GetComponent<TipGuide>().lastWordTipOnClick = EliStateChange;
                }
                else
                {
                    Queue<TipWord> words = new Queue<TipWord>();
                    words.Enqueue(new TipWord(LanguageManger.GetMe().GetWords("L_xiaochu_tishi11"), new Vector3(0,-400,0)));
                    GuideManager.Instance.ShowGuildTip(words);
                    GuideManager.Instance.Guildtip.GetComponent<TipGuide>().lastWordTipOnClick = EliStateChange;
                }

                return;
            }
        }


        #endregion

        EliStateChange();

	}

    private void OnCommonYinDaoCallBack(GameObject go)
    {
        EliStateChange();
    }

    //private void OnYinDaoCompletedCallBack(GameObject go)
    //{
    //    GuideManager.Instance.ShowTipWord(LanguageManger.GetMe().GetWords("yingdao1"), new Vector3(0,-400,0));
    //    UIEventListener.Get(GuideManager.Instance.Guildtip).onClick = OnCommonYinDaoCallBack;
    //}

    private void EliStateChange()
    {
        if (tfyxState == TFYXState.ENDPOINT)
        {
            m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_PREVICTORY);
        }
        else if (tfyxState == TFYXState.DEAD)
        {
            m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_DEFEATED);
        }
        else
        {
            m_player.CheckWin();
        }
    }

}





