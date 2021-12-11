using UnityEngine;
using System.Collections;




public class EliminateProcedureResourceLoad:EliminateProcedureBase
{
	private EliminateProcedureManager m_ProcedureManager = null;
    private EliminatePlayer m_player;

	public override EliminateProcedureType GetProcedureType(){
		return EliminateProcedureType.PROCEDURE_RESOURCE_LOAD;
	
	}
    public override bool Init(EliminateProcedureManager manager){
		SystemConfig.Log("PROCEDURE_RESOURCE_LOAD Init");
		m_ProcedureManager = manager;
        m_player = m_ProcedureManager.GetEliminatePlayer();
		return true;
	}
	
    public override void OnEnter(){
		SystemConfig.Log("PROCEDURE_RESOURCE_LOAD OnEnter");
    }
	
    public override void OnLeave(){
		SystemConfig.Log("PROCEDURE_RESOURCE_LOAD OnLeave");
	}

    public override void Update(float deltaTime)
    {
         m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_PRODUCEMISSION);
	}



}





