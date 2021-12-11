using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;


public class EliminateProcedureProduceMission:EliminateProcedureBase
{
	private EliminateProcedureManager m_ProcedureManager = null;
	private EliminatePlayer m_player = null;
    private bool loadingMap = false;
    private float timer = 0;
    private bool startTimer = false;
    private bool preMissionGuild = false;

    private AnimationState state;
    public enum AnimationState
    {
        None,
        LoadResource,
        PreMissionShow,
        MissionShow,
        MieSheepShow,
        Ready,
        End,
    }


    public override EliminateProcedureType GetProcedureType(){
		return EliminateProcedureType.PROCEDURE_PRODUCEMISSION;
	
	}
    public override bool Init(EliminateProcedureManager manager){
		SystemConfig.Log("PROCEDURE_PRODUCEMISSION Init");
		m_ProcedureManager = manager;
		m_player = manager.GetEliminatePlayer();
		return true;
	}
	
    public override void OnEnter(){
		SystemConfig.Log("PROCEDURE_PRODUCEMISSION OnEnter");
		LoadMissionData();
		LoadMissionIcon();
        loadingMap = false;
        timer = 0;
        startTimer = false;
        state = AnimationState.None;
        preMissionGuild = false;
    }
	
    public override void OnLeave(){
        loadingMap = false;
		SystemConfig.Log("PROCEDURE_PRODUCEMISSION OnLeave");
	}
	
    public override void Update(float deltaTime){

        switch (state)
        {
            case AnimationState.None:
                m_player.StartGeneMap();
                Map.Instance.LoadMap();
                //EleUIController.Instance.LoadObj.SetActive(false);
                state = AnimationState.LoadResource;
                break;
            case AnimationState.LoadResource:
                if (m_player.produceMapCompleted)
                {
                    EleUIController.Instance.PlayPreMissionAnimation();
                    state = AnimationState.PreMissionShow;
                }
                break;

            case AnimationState.PreMissionShow:
                if (EleUIController.Instance.preMissionAniamtion.playCompleted)
                {
                    EleUIController.Instance.PlayMissionAnimation();
                    EleUIController.Instance.PlayMieSheepAnimation();
                    state = AnimationState.MieSheepShow;
                }
                break;
            //case AnimationState.MissionShow:
            //    if (EleUIController.Instance.missionAnimation.ProMissionAnimationCompleted)
            //    {
            //        EleUIController.Instance.PlayMieSheepAnimation();
            //        state = AnimationState.MieSheepShow;
            //    }
            //    break;

            case AnimationState.MieSheepShow:
                if (EleUIController.Instance.mieSheepAnimation.playCompleted)
                {
                    state = AnimationState.Ready;
                }
                break;
            case AnimationState.Ready:
                #region guild
                if (!preMissionGuild)
                {
                    if (LevelData.guildLevel == 1)
                    {
                        if (LevelData.needGuild)
                        {
                            preMissionGuild = true;
                            UIMissionView firstMission = EleUIController.Instance.m_MisIcon[0];
                            Mission requestMission = LevelData.GetMissionByID(firstMission.m_ntype);
                            Mission completedMission = MissionManager.Instance.GetMissionByID(firstMission.m_ntype);
                            int remianedReq = requestMission.amount - completedMission.amount;

                            GuideManager.Instance.ShowGuideTarget(firstMission.GetComponent<UIWidget>(), false, string.Format(LanguageManger.GetMe().GetWords("L_mission"), remianedReq, requestMission.missionName), new Vector3(0,-400,0));
                            UIEventListener.Get(GuideManager.Instance.m_GuildTarget.gameObject).onClick = GuildMission;
                        }
                    }
                }
                #endregion
                state = AnimationState.End;
                break;
            case AnimationState.End:
                if (!preMissionGuild)
                {
                    m_ProcedureManager.ChangProcedure(EliminateProcedureType.EliminateProcedureFontStory);
                }
                break;
        }

	}

    void GuildMission(GameObject go)
    {
        preMissionGuild = false;
    }

	//加载任务数据
	void LoadMissionData()
	{
        int level = LocalDataBase.Instance().GetSelectCopyLevel();
        LevelData.LoadDataFromLocal(level);
        MissionManager.Instance.InitScore(LevelData.requestMissions, LevelData.limitAmount);
		
	}	
	
  //加载任务图标到任务栏
   void LoadMissionIcon()
   {

		List<UIMissionView> m_MisIcon = EleUIController.Instance.m_MisIcon;
		int MissionCount = 0;
		string strIconName = "";
		Mission requestMission = null;
		


		//初始化元素任务图标及类型;
		int allMissionCount = LevelData.GetMissionCount();
		
		for(int i=0;i<allMissionCount;i++){
			requestMission = LevelData.GetMissionByIndex(i);
			if(requestMission != null && TableManager.GetMissionByID(requestMission.type).DisplayAtTop == 1){
				MissionCount++;
				strIconName = TableManager.GetMissionByID(requestMission.type).SpriteName;
				if(MissionCount <= m_MisIcon.Count){
					m_MisIcon[MissionCount-1].gameObject.SetActive(true);
		           m_MisIcon[MissionCount-1].Init(requestMission.type, requestMission.amount);
		           m_MisIcon[MissionCount-1].m_sprite.spriteName = strIconName;
				}else{
					SystemConfig.LogError("do not have space view");
					break;
		        }
			}
		}
		
		//多余的不显示;
        for (; MissionCount < m_MisIcon.Count; MissionCount++)
        {
            m_MisIcon[MissionCount].Init(-1, 0);
			m_MisIcon[MissionCount].gameObject.SetActive(false);
		}
		
		//目标分数显示;
		if(LevelData.requestScore >0){
			EleUIController.Instance.targetScore.SetActive(true);
			EleUIController.Instance.targetScore.transform.Find("Score").GetComponent<UILabel>()
				.text = LevelData.requestScore.ToString();
		}else{
			EleUIController.Instance.targetScore.SetActive(false);
		}

        //关卡
        EleUIController.Instance.copyLevel.text = string.Format(LanguageManger.GetMe().GetWords("L_1077"), LevelData.currentLevel.ToString());

       //星星分布
        float width = EleUIController.Instance.scoreSlider.backgroundWidget.width;
        for (int i = 0; i < 3; i++)
        {
            Transform star1 = EleUIController.Instance.scoreSlider.transform.Find("star"+(i+1));
            star1.localPosition = new Vector3((LevelData.currenCopyDetail.GetStarbyIndex(i) * 1.0f / LevelData.maxScore - 0.5f) * width, 0, 0);
        }
        
		//副本类型;
        if (LevelData.type == CopyType.MoveLimit)
        {
			EleUIController.Instance.copyType.text = LanguageManger.GetMe().GetWords("L_1040");
		}else{
			EleUIController.Instance.copyType.text = LanguageManger.GetMe().GetWords("L_1039");
		}



		EleUIController.Instance.limitAmount = MissionManager.Instance.limitAmount;
		EleUIController.Instance.UpdateScoreUI();
		EleUIController.Instance.UpdateMoveStepUI();
        EleUIController.Instance.UpdateEquipNumUI();
        EleUIController.Instance.UpdateMissionUI();
        EleUIController.Instance.UpdateDataUI();
        
	}

}





