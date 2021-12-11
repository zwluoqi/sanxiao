using UnityEngine;
using System.Collections;




public class EliminateProcedurePreVictory:EliminateProcedureBase
{
	private EliminateProcedureManager m_ProcedureManager = null;

    private float duration = 1f;
    private float startTime = 0;

    private PreVictoryState state;

    public enum PreVictoryState
    {
        None,
        ShowSpec,
        ShowSpecDone,
        ShowMissionComplete,
        ShowStep,
        UseStep,
        End,
    }
    public override EliminateProcedureType GetProcedureType(){
		return EliminateProcedureType.PROCEDURE_PREVICTORY;
	
	}
    public override bool Init(EliminateProcedureManager manager){
		SystemConfig.LogWarning("PROCEDURE_PREVICTORY Init");
		m_ProcedureManager = manager;
		
		return true;
	}
	
    public override void OnEnter(){
		SystemConfig.LogWarning("PROCEDURE_PREVICTORY OnEnter");
		EliminateLogic.Instance.StopTimeTick();
        state = PreVictoryState.None;
        startTime = 0;

        duration = EleUIController.Instance.eleTextEffect.waitDuration * 2;
		
	}
	
    public override void OnLeave(){

        SystemConfig.LogWarning("PROCEDURE_PREVICTORY OnLeave");
	}

    public override void Update(float deltaTime)
    {
        if (state == PreVictoryState.None)
        {
            state = PreVictoryState.ShowSpec;
            Map.Instance.RemoveSpec(OnRemoveSpecDone);
        }
        else if (state == PreVictoryState.ShowSpec)
        {
            ;//todo
        }
        else if (state == PreVictoryState.ShowSpecDone)
        {
            startTime = 0;
            EleUIController.Instance.eleTextEffect.ShowCelerationDirectlyText("renwuwancheng");
            state = PreVictoryState.ShowMissionComplete;
        }
        else if(state == PreVictoryState.ShowMissionComplete)
        {
            startTime += deltaTime;
            if (startTime > duration )
            {
                EleUIController.Instance.eleTextEffect.ShowCelerationDirectlyText("bushujiangli");
                state = PreVictoryState.ShowStep;
                startTime = 0;
            }
        }
        else if (state == PreVictoryState.ShowStep)
        {
            startTime += deltaTime;
            if (startTime > duration)
            {
                state = PreVictoryState.UseStep;
                startTime = 0;
                Map.Instance.UseStep(OnEliminateNotCheck, MissionManager.Instance.limitAmount, m_ProcedureManager.GetEliminatePlayer().flyIns);
            }
        }
        else if (state == PreVictoryState.UseStep)
        {
            //todo
        }
        else if (state == PreVictoryState.End)
        {
            m_ProcedureManager.ChangProcedure(EliminateProcedureType.PROCEDURE_VICTORY);
        }
	}


    public void OnRemoveSpecDone()
    {
        state = PreVictoryState.ShowSpecDone;
    }


	public void OnEliminateNotCheck(){
		Map.Instance.EliminateFromCheck(OnUsedStep);
	}	
	
	public void OnUsedStep(){
        state = PreVictoryState.End;
	}
}





