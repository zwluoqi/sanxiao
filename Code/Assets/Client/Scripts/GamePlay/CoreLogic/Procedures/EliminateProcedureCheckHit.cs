using UnityEngine;
using System.Collections;




public class EliminateProcedureCheckHit:EliminateProcedureBase
{
	private EliminateProcedureManager m_ProcedureManager = null;
	private EliminatePlayer m_player = null;
	
    public override EliminateProcedureType GetProcedureType(){
		return EliminateProcedureType.PROCEDURE_CHECK_HITS;
	
	}
    public override bool Init(EliminateProcedureManager manager){
		SystemConfig.Log("PROCEDURE_CHECK_HITS Init");
		m_ProcedureManager = manager;
		m_player = manager.GetEliminatePlayer();
		return true;
	}
	
    public override void OnEnter(){
		SystemConfig.Log("PROCEDURE_CHECK_HITS OnEnter");
		//
	}
	
    public override void OnLeave(){
		SystemConfig.Log("PROCEDURE_CHECK_HITS OnLeave");
	}

    public override void Update(float deltaTime)
    {
		FindHintItems();
	}
	
	public void FindHintItems()
	{
        m_player.hintItems = Map.Instance.FindNextMove(out m_player.diction);
		
		//没有匹配项,重新生成item;
		if(m_player.hintItems.Count == 0)
		{
			m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_NOMATCHING);
		}
		//存在匹配项,项waiting转换;
		else
		{
			m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_WAITING_INPUT);
		}
	}	
}





