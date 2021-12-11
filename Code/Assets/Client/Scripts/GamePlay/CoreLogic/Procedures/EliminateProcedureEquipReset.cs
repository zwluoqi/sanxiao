using UnityEngine;
using System.Collections;




public class EliminateProcedureEquipReset : EliminateProcedureBase
{
	private EliminateProcedureManager m_ProcedureManager = null;
	private const float deltaTime = 0.5f;
	private float animationTimeDelta = deltaTime;
	private EliminatePlayer m_player = null;
	
	public override EliminateProcedureType GetProcedureType(){
        return EliminateProcedureType.EliminateProcedureEquipReset;
	
	}
    public override bool Init(EliminateProcedureManager manager){
        SystemConfig.Log("EliminateProcedureEquipReset Init");
		m_ProcedureManager = manager;
		m_player = manager.GetEliminatePlayer();
		return true;
	}
	
    public override void OnEnter(){
        SystemConfig.Log("EliminateProcedureEquipReset OnEnter");
        m_player.StartGeneMap();
        Map.Instance.ReGenItems();
	}
	
    public override void OnLeave(){
        SystemConfig.Log("EliminateProcedureEquipReset OnLeave");
	}

    public override void Update(float deltaTime)
    {
        if (m_player.produceMapCompleted)
        {
            SystemConfig.Log("OnReGenproduceMissionCompleted");
            m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_CHECK_HITS);
        }
	}
	
}





