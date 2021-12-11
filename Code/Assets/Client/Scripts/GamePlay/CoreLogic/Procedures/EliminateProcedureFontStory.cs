using UnityEngine;
using System.Collections;




public class EliminateProcedureFontStory : EliminateProcedureBase
{
	private EliminateProcedureManager m_ProcedureManager = null;
    
    public override EliminateProcedureType GetProcedureType(){
		return EliminateProcedureType.PROCEDURE_PREVICTORY;
	
	}
    public override bool Init(EliminateProcedureManager manager){
        SystemConfig.LogWarning("EliminateProcedureFontStory Init");
		m_ProcedureManager = manager;
		
		return true;
	}
	
    public override void OnEnter(){
        SystemConfig.LogWarning("EliminateProcedureFontStory OnEnter");
        if (LevelData.font_storys.Count > 0)
        {
            ShowFontText();
        }
        else
        {
            EliminateLogic.Instance.StartEleCheckHits();
        }
	}
	
    public override void OnLeave(){

        SystemConfig.LogWarning("EliminateProcedureFontStory OnLeave");
	}

    public override void Update(float deltaTime)
    {
		
	}


    private void ShowFontText()
    {
        EleUIController.Instance.ShowStory(LevelData.font_storys);
    }
}





