using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class EliminateProcedureWaitingForInput:EliminateProcedureBase
{
	private EliminateProcedureManager m_ProcedureManager = null;
	private EleUIController uiController = null;
	private EliminatePlayer m_player = null;
	private float timeHintNextMove 					= 10;
	private float processBomTime				= 0;
	
    public override EliminateProcedureType GetProcedureType(){
		return EliminateProcedureType.PROCEDURE_WAITING_INPUT;
	
	}
    public override bool Init(EliminateProcedureManager manager){
		SystemConfig.Log("PROCEDURE_WAITING_INPUT Init");
		m_ProcedureManager = manager;
		m_player = manager.GetEliminatePlayer();
		processBomTime = 0;
		return true;
	}

	void OnPlay (GameObject go)
	{
		ADTouTiao.Instance.RequestRewardVideo (OnPlayDone);
	}

	void OnPlayDone(){
		if (LevelData.type == CopyType.TimeLimit) {
//			MissionManager.Instance.limitAmount += 10;
			EleUIController.Instance.limitAmount+=30;
		} else {
//			MissionManager.Instance.limitAmount += 5;
			EleUIController.Instance.limitAmount+=5;
		}
	}


    public override void OnEnter(){
		SystemConfig.Log("PROCEDURE_WAITING_INPUT OnEnter");
		//
		timeHintNextMove = 10f;
		uiController = EleUIController.Instance;

        if (LevelData.type == CopyType.TimeLimit)
        {
            if (MissionManager.Instance.limitAmount == 10 && !LevelData.lastAnimationShow)
            {
                LevelData.lastAnimationShow = true;
                EleUIController.Instance.PlayLastTip();
				BoxManager.Instance.ShowTimeShareMessage ();
				UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += OnPlay;
            }
        }
        else
        {
            if (MissionManager.Instance.limitAmount == 5 && !LevelData.lastAnimationShow)
            {
                LevelData.lastAnimationShow = true;
                EleUIController.Instance.PlayLastTip();
				BoxManager.Instance.ShowStepShareMessage ();
				UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += OnPlay;
            }
        }

        #region guild


        if (LevelData.needWaitInputGuild)
        {
            string content = "";
            int minrow = int.MaxValue;
            int maxrow = int.MinValue;
            int minCol = int.MaxValue;
            int maxCol = int.MinValue;
            foreach (UIEliminateItemView item in m_player.hintItems)
            {
                if (item.staySquare.m_row < minrow)
                {
                    minrow = item.staySquare.m_row;
                }
                if (item.staySquare.m_row > maxrow)
                {
                    maxrow = item.staySquare.m_row;
                }
                if (item.staySquare.m_col < minCol)
                {
                    minCol = item.staySquare.m_col;
                }
                if (item.staySquare.m_col > maxCol)
                {
                    maxCol = item.staySquare.m_col;
                }
            }
            GameObject newMask = new GameObject("xiaochuMask");
            UIWidget maskwidget = NGUITools.AddWidget<UIWidget>(newMask);
            maskwidget.transform.parent = m_player.hintItems[0].transform.parent;
            maskwidget.transform.localScale = Vector3.one;
            maskwidget.SetDimensions((maxCol - minCol + 1) * UISquareView.BACK_WIDTH, (maxrow - minrow + 1) * UISquareView.BACK_WIDTH);
            maskwidget.transform.localPosition = (Map.Instance.GetSquare(minrow, minCol).transform.localPosition + Map.Instance.GetSquare(maxrow, maxCol).transform.localPosition) / 2;

            UISquareView centerSquare = null;
            if (m_player.diction == SwipeDirection.Up)
            {
                if (LevelData.mode == CopyMode.ElE)
                {
                    content = LanguageManger.GetMe().GetWords("L_xiaochu_tishi1");
                }
                else
                {
                    content = LanguageManger.GetMe().GetWords("L_tfmove_tishi12");
                }
                centerSquare = Map.Instance.GetSquare(m_player.hintItems[0].staySquare.m_row + 1, m_player.hintItems[0].staySquare.m_col);
            }
            else if (m_player.diction == SwipeDirection.Down)
            {
                if (LevelData.mode == CopyMode.ElE)
                {
                    content = LanguageManger.GetMe().GetWords("L_xiaochu_tishi2");
                }
                else
                {
                    content = LanguageManger.GetMe().GetWords("L_tfmove_tishi11");
                }
                centerSquare = Map.Instance.GetSquare(m_player.hintItems[0].staySquare.m_row - 1, m_player.hintItems[0].staySquare.m_col);
            }
            else if (m_player.diction == SwipeDirection.Left)
            {
                content = LanguageManger.GetMe().GetWords("L_xiaochu_tishi3");
                centerSquare = Map.Instance.GetSquare(m_player.hintItems[0].staySquare.m_row, m_player.hintItems[0].staySquare.m_col - 1);
            }
            else if (m_player.diction == SwipeDirection.Right)
            {
                content = LanguageManger.GetMe().GetWords("L_xiaochu_tishi4");
                centerSquare = Map.Instance.GetSquare(m_player.hintItems[0].staySquare.m_row, m_player.hintItems[0].staySquare.m_col + 1);
            }
            List<UIWidget> maskObjs = new List<UIWidget>();
            for (int i = minrow; i <= maxrow; i++)
            {
                for (int j = minCol; j <= maxCol; j++)
                {
                    UISquareView square = Map.Instance.GetSquare(i, j);
                    if (square != centerSquare && !m_player.hintItems.Contains(square.item))
                    {
                        maskObjs.Add(square.GetComponent<UIWidget>());
                    }
                }
            }
            if (m_player.hintItems[0].tab_ele.AffectType == (int)AffectType.WANNENG)//万能使用引导
            {
                content = LanguageManger.GetMe().GetWords("L_xiaochu_tishi5");
            }
            else if ((m_player.hintItems[0].tab_ele.AffectType == (int)AffectType.ROW || m_player.hintItems[0].tab_ele.AffectType == (int)AffectType.COL)
                && m_player.hintItems.Count == 3)//直线使用引导
            {
                content = LanguageManger.GetMe().GetWords("L_xiaochu_tishi10");
            }
            else if (m_player.hintItems[0].tab_ele.AffectType == (int)AffectType.AROUND && m_player.hintItems.Count == 3)//爆炸使用引导
            {
                content = LanguageManger.GetMe().GetWords("L_xiaochu_tishi13");
            }
            else if (m_player.hintItems[0].tab_ele.AffectType != (int)AffectType.NONE && m_player.hintItems[1].tab_ele.AffectType != (int)AffectType.NONE)//两个高亮使用引导
            {
                if (LevelData.mode == CopyMode.ElE)
                {
                    content = LanguageManger.GetMe().GetWords("L_xiaochu_tishi15");
                }
                else
                {
                    if (LevelData.guildWaitInputCount == 0)//塔防高亮第一次提醒
                    {
                        content = LanguageManger.GetMe().GetWords("L_tfmove_tishi13");
                    }
                    else if (LevelData.guildWaitInputCount == 1)//塔防高亮第二次提醒
                    {
                        content = LanguageManger.GetMe().GetWords("L_tfmove_tishi9");
                    }
                }
            }
            else
            {
                if (m_player.hintItems.Count == 4)//直线形成引导
                {
                    content = LanguageManger.GetMe().GetWords("L_xiaochu_tishi9");
                }
                else if (m_player.hintItems.Count == 5 && (maxCol - minCol >= 4|| maxrow - minrow >= 4))//万能形成引导
                {
                    content = LanguageManger.GetMe().GetWords("L_xiaochu_tishi14");
                }
                else if (m_player.hintItems.Count == 5)//爆炸形成引导
                {
                    content = LanguageManger.GetMe().GetWords("L_xiaochu_tishi12");
                }
            }
            GuideManager.Instance.ShowMutipleGuide(maskwidget, content, new Vector3(0,-400,0), maskObjs, m_player.hintItems[0].transform , m_player.diction);
        }
        #endregion
	}
	
    public override void OnLeave(){
		SystemConfig.Log("PROCEDURE_WAITING_INPUT OnLeave");
        GuideManager.Instance.HideGuide();
	}

    private void ResetAnimation()
    {
        foreach (UISquareView square in Map.Instance.squares)
        {
            if (square.item != null)
            {
                if (!square.item.ContainerObj.GetComponent<Animation>().isPlaying)
                {
                    square.item.IdleAnimation();
                }
            }
        }
    }

    public override void Update(float deltaTime)
    {
		CheckHint();
        ResetAnimation();
        uiController.UpdateEquipNumUI();
        if (uiController.selectedItem != null && uiController.targetItem!=null)
        {
            if (uiController.selectedItem.staySquare.CanMoveItemOut() && uiController.targetItem.staySquare.CanMoveItemOut())
            {
                if (GuildForceMove())
                {
                    m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_ELIMINATING);
                }
                else
                {
                    uiController.ResetSelect();
                }
			}
        } 
		
		//时间到,失败;
		if(MissionManager.Instance.limitAmount <= 0){
			m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_PREDEFEATED);
        }

//		if(MissionManager.Instance.limitAmount == 5){
//			m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_PREDEFEATED);
//		}

        #region guild
        if (LocalDataBase.equipGuild)
        {
            //锤子
            if (LocalDataBase.equipWaitInput == 1)
            {
                LocalDataBase.equipWaitInput++;
                GuideManager.Instance.ShowForceGuide(EleUIController.Instance.equipController.equips[1], false, LanguageManger.GetMe().GetWords("L_daoju_tishi7"), Vector3.zero);
            }
            else if (LocalDataBase.equipWaitInput == 3)
            {
                LocalDataBase.equipWaitInput++;
                UISquareView square = Map.Instance.GetSquare(5, 2);
                GuideManager.Instance.ShowForceGuide(square.item.gameObject, false, LanguageManger.GetMe().GetWords("L_daoju_tishi8"), new Vector3(0,-400,0));
            }
            else if (LocalDataBase.equipWaitInput == 5)
            {
                LocalDataBase.equipWaitInput++;
                TipWord tip = new TipWord(LanguageManger.GetMe().GetWords("L_daoju_tishi9"),Vector3.zero);
                Queue<TipWord> tips = new Queue<TipWord>();
                tips.Enqueue(tip);
                GuideManager.Instance.ShowGuildTip(tips);
            }
            else if (LocalDataBase.equipWaitInput == 7)
            {
                LocalDataBase.equipWaitInput++;
                GuideManager.Instance.ShowForceGuide(EleUIController.Instance.equipController.equips[0], false, LanguageManger.GetMe().GetWords("L_daoju_tishi10"), Vector3.zero);
            }
            else if (LocalDataBase.equipWaitInput == 9)
            {
                LocalDataBase.equipWaitInput++;
                UISquareView square = Map.Instance.GetSquare(2, 3);
                GuideManager.Instance.ShowForceGuide(square.item.gameObject, false, LanguageManger.GetMe().GetWords("L_daoju_tishi11"), new Vector3(0,-400,0));
            }
            else if (LocalDataBase.equipWaitInput == 11)
            {
                LocalDataBase.equipWaitInput++;
                TipWord tip = new TipWord(LanguageManger.GetMe().GetWords("L_daoju_tishi12"), Vector3.zero);
                Queue<TipWord> tips = new Queue<TipWord>();
                tips.Enqueue(tip);
                GuideManager.Instance.ShowGuildTip(tips);
            }
            else if (LocalDataBase.equipWaitInput == 13)
            {
                LocalDataBase.equipWaitInput++;
                GuideManager.Instance.ShowForceGuide(EleUIController.Instance.equipController.equips[2], false, LanguageManger.GetMe().GetWords("L_daoju_tishi13"), Vector3.zero);
            }
            else if (LocalDataBase.equipWaitInput == 15)
            {
                LocalDataBase.equipWaitInput++;
                GuideManager.Instance.ShowForceGuide(EleUIController.Instance.equipController.left, false, LanguageManger.GetMe().GetWords("L_daoju_tishi14"), Vector3.zero);
            }
            else if (LocalDataBase.equipWaitInput == 17)
            {
                LocalDataBase.equipWaitInput++;
                UISquareView square = Map.Instance.GetSquare(8, 4);
                GuideManager.Instance.ShowForceGuide(square.item.gameObject, false, LanguageManger.GetMe().GetWords("L_daoju_tishi15"), new Vector3(0,-400,0));
            }
            else if (LocalDataBase.equipWaitInput == 19)
            {
                LocalDataBase.equipGuild = false;
                LocalDataBase.equipWaitInput++;
                TipWord tip = new TipWord(LanguageManger.GetMe().GetWords("L_daoju_tishi16"), Vector3.zero);
                Queue<TipWord> tips = new Queue<TipWord>();
                tips.Enqueue(tip);
                GuideManager.Instance.ShowGuildTip(tips);
            }
        }
        #endregion
    }

    bool GuildForceMove()
    {
        if (LevelData.needWaitInputGuild)
        {
            SwipeDirection realDic = SwipeDirection.None;
            SwipeDirection reverseDic = SwipeDirection.None;
            if (m_player.diction == SwipeDirection.Left)
            {
                reverseDic = SwipeDirection.Right;
            }
            else if (m_player.diction == SwipeDirection.Right)
            {
                reverseDic = SwipeDirection.Left;
            }
            else if (m_player.diction == SwipeDirection.Up)
            {
                reverseDic = SwipeDirection.Down;
            }
            else if (m_player.diction == SwipeDirection.Down)
            {
                reverseDic = SwipeDirection.Up;
            }

            if (uiController.targetItem.staySquare.m_row == uiController.selectedItem.staySquare.m_row - 1)
            {
                realDic = SwipeDirection.Down;
            }
            else if (uiController.targetItem.staySquare.m_row == uiController.selectedItem.staySquare.m_row + 1)
            {
                realDic = SwipeDirection.Up;
            }
            else if (uiController.targetItem.staySquare.m_col == uiController.selectedItem.staySquare.m_col - 1)
            {
                realDic = SwipeDirection.Left;
            }
            else if (uiController.targetItem.staySquare.m_col == uiController.selectedItem.staySquare.m_col + 1)
            {
                realDic = SwipeDirection.Right;
            }


            if (realDic == m_player.diction && uiController.selectedItem == m_player.hintItems[0])
            {
                return true;
            }
            else if (realDic == reverseDic && uiController.targetItem == m_player.hintItems[0])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    void CheckHint()
	{
		//10秒内没有操作;
		
		if(timeHintNextMove >0)
		{
			timeHintNextMove-=Time.deltaTime;
			if(timeHintNextMove <=0)
			{
				//If have hint items, show them
				if( m_player.hintItems.Count > 0)
				{
					//Show next move
					Hint();
					
				}
				
			}
        }
        else
        {
            timeHintNextMove -= Time.deltaTime;
            int tenSecond = (int)(timeHintNextMove) % 10;
            if (tenSecond == 0)
            {
                SoundEffect.Instance.PlaySound(SoundEffect.hitTip);
                timeHintNextMove -= 1;
            }
        }
	}
	//show hint items
	void Hint()
	{
		for(int i=0;i<m_player.hintItems.Count;i++){
			if(m_player.hintItems[i] != null)
				m_player.hintItems[i].HintAnimation();
		}
	}
}





