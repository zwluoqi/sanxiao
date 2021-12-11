using UnityEngine;
using System.Collections;




public class EliminateProcedureNoMatch:EliminateProcedureBase
{
	private EliminateProcedureManager m_ProcedureManager = null;
	private const float deltaTime = 0.5f;
	private float animationTimeDelta = deltaTime;
	private EliminatePlayer m_player = null;
	
	public override EliminateProcedureType GetProcedureType(){
		return EliminateProcedureType.PROCEDURE_NOMATCHING;
	
	}
    public override bool Init(EliminateProcedureManager manager){
		SystemConfig.Log("PROCEDURE_NOMATCHING Init");
		m_ProcedureManager = manager;
		m_player = manager.GetEliminatePlayer();
		return true;
	}
	
    public override void OnEnter(){
		SystemConfig.Log("PROCEDURE_NOMATCHING OnEnter");
		//Display no more match
		animationTimeDelta = deltaTime;
		EleUIController.Instance.AnimationNoMatch(deltaTime+1);
	}
	
    public override void OnLeave(){
		SystemConfig.Log("PROCEDURE_NOMATCHING OnLeave");
	}

    public override void Update(float deltaTime)
    {
		if(animationTimeDelta > 0){
            animationTimeDelta -= deltaTime;
			if(animationTimeDelta <=0)
			{
                m_player.StartGeneMap();
				Map.Instance.ReGenItems();
			}
        }
        else
        {
            if (m_player.produceMapCompleted)
            {
                SystemConfig.Log("OnNoMatchproduceMissionCompleted");
                m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_CHECK_HITS);
            }
        }
	}
	
}





