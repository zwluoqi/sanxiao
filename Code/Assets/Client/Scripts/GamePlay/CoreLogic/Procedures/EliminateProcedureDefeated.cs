using UnityEngine;
using System.Collections;
//using UnityEngine.Advertisements;




public class EliminateProcedureDefeated:EliminateProcedureBase
{
	private EliminateProcedureManager m_ProcedureManager = null;
	//private float animationTimeDelta = 1f;
	
    public override EliminateProcedureType GetProcedureType(){
		return EliminateProcedureType.PROCEDURE_DEFEATED;
	
	}
    public override bool Init(EliminateProcedureManager manager){
		SystemConfig.Log("PROCEDURE_DEFEATED Init");
		m_ProcedureManager = manager;
		
		return true;
	}
	
    public override void OnEnter(){
		SystemConfig.Log("PROCEDURE_DEFEATED OnEnter");

		//animationTimeDelta = 3f;
		//MainUIController.Instance.AnimationGameOver();
		PageManager.Instance.OpenPage("DefeatedController","");

//				if (Advertisement.IsReady())
//				{
//						Advertisement.Show();
//				}
	}
	
    public override void OnLeave(){
		SystemConfig.Log("PROCEDURE_DEFEATED OnLeave");
	}

    public override void Update(float deltaTime)
    {
        m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_END);
	}
}





