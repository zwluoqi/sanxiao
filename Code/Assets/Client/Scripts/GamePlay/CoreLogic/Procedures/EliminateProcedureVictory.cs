using UnityEngine;
using System.Collections;
//using UnityEngine.Advertisements;




public class EliminateProcedureVictory:EliminateProcedureBase
{
	private EliminateProcedureManager m_ProcedureManager = null;
	//private float animationTimeDelta = 1f;
    public override EliminateProcedureType GetProcedureType(){
		return EliminateProcedureType.PROCEDURE_VICTORY;
	
	}
    public override bool Init(EliminateProcedureManager manager){
		SystemConfig.Log("PROCEDURE_VICTORY Init");
		m_ProcedureManager = manager;
		
		return true;
	}
	
    public override void OnEnter(){
		SystemConfig.Log("PROCEDURE_VICTORY OnEnter");

		
		//设置当前关卡的评分;todo;		
        int thisResult = MissionManager.Instance.GetResultStart();
        if(thisResult > LocalDataBase.GetCopyStar(LevelData.currentLevel)){
            LocalDataBase.SetCopyStar(LevelData.currentLevel,thisResult);
        }
		//解锁下一关卡;
        if (LocalDataBase.GetCopyStar(LevelData.currentLevel + 1) == -1)
        {
            LocalDataBase.SetCopyStar(LevelData.currentLevel + 1, 0);		
		}

		PageManager.Instance.OpenPage("VictoryController","");

				if(LevelData.currentLevel == 6 && !LocalDataBase.Instance().HadGetChargeGift()){

						PageManager.Instance.OpenPage("libao6","");
				}
//
//				if (Advertisement.IsReady())
//				{
//						Advertisement.Show();
//				}
	}
	
    public override void OnLeave(){
		SystemConfig.Log("PROCEDURE_VICTORY OnLeave");
	}

    public override void Update(float deltaTime)
    {
        m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_END);
	}
}





