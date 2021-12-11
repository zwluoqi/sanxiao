using UnityEngine;
using System.Collections;




public class EliminateProcedureEnd:EliminateProcedureBase
{
	private EliminateProcedureManager m_ProcedureManager = null;
	private bool process = false;
    public override EliminateProcedureType GetProcedureType(){
		return EliminateProcedureType.PROCEDURE_END;
	
	}
    public override bool Init(EliminateProcedureManager manager){
		SystemConfig.Log("PROCEDURE_END Init");
		m_ProcedureManager = manager;
		
		return true;
	}
	
    public override void OnEnter(){
		SystemConfig.Log("PROCEDURE_END OnEnter");
	}
	
    public override void OnLeave(){
		SystemConfig.Log("PROCEDURE_END OnLeave");
	}

    public override void Update(float deltaTime)
    {
		if(!process){
			process = true;
            //清空地图数据;
            Map.Instance.MapOver();

            WidgetBufferManager.Instance.ClearObjs();

		}
	}
}





