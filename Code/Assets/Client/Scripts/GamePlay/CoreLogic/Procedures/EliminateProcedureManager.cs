using UnityEngine;
using System.Collections.Generic;


public sealed class EliminateProcedureManager
{
    private EliminatePlayer m_Player = null;
    private EliminateProcedureBase m_CurrentProcedure = null;
    private IDictionary<EliminateProcedureType, EliminateProcedureBase> m_ProcedureList = new Dictionary<EliminateProcedureType, EliminateProcedureBase>();
	
    public EliminateProcedureManager(EliminatePlayer player)
    {
        m_Player = player;
        
		m_ProcedureList[EliminateProcedureType.PROCEDURE_RESOURCE_LOAD] = new EliminateProcedureResourceLoad();
		m_ProcedureList[EliminateProcedureType.PROCEDURE_PRODUCEMISSION] = new EliminateProcedureProduceMission();
		m_ProcedureList[EliminateProcedureType.PROCEDURE_CHECK_HITS] = new EliminateProcedureCheckHit();
		m_ProcedureList[EliminateProcedureType.PROCEDURE_WAITING_INPUT] = new EliminateProcedureWaitingForInput();
		m_ProcedureList[EliminateProcedureType.PROCEDURE_ELIMINATING] = new EliminateProcedureEliminating();
		m_ProcedureList[EliminateProcedureType.PROCEDURE_NOMATCHING] = new EliminateProcedureNoMatch();
		m_ProcedureList[EliminateProcedureType.PROCEDURE_PREVICTORY] = new EliminateProcedurePreVictory();
		m_ProcedureList[EliminateProcedureType.PROCEDURE_VICTORY] = new EliminateProcedureVictory();
		m_ProcedureList[EliminateProcedureType.PROCEDURE_PREDEFEATED] = new EliminateProcedurePreDefeated();
		m_ProcedureList[EliminateProcedureType.PROCEDURE_DEFEATED] = new EliminateProcedureDefeated();
		m_ProcedureList[EliminateProcedureType.PROCEDURE_END] = new EliminateProcedureEnd();
        m_ProcedureList[EliminateProcedureType.EliminateProcedureEquipReset] = new EliminateProcedureEquipReset();
        m_ProcedureList[EliminateProcedureType.EliminateProcedureFontStory] = new EliminateProcedureFontStory();
	}

    public bool Init()
    {
        foreach (KeyValuePair<EliminateProcedureType, EliminateProcedureBase> procedure in m_ProcedureList)
        {
            if (!procedure.Value.Init(this))
            {
                return false;
            }
        }
        m_CurrentProcedure = null;
        return true;
    }

    public void ChangProcedure(EliminateProcedureType type)
    {
        if (m_CurrentProcedure.GetProcedureType() != type)
        {
            m_CurrentProcedure.OnLeave();
            m_CurrentProcedure = m_ProcedureList[type];
            m_CurrentProcedure.OnEnter();
        }
    }

    public EliminateProcedureBase GetActiveProcedure()
    {
        return this.m_CurrentProcedure;
    }

    public void Update(float deltaTime)
    {

        if (m_CurrentProcedure == null)
        {
            m_CurrentProcedure = m_ProcedureList[EliminateProcedureType.PROCEDURE_RESOURCE_LOAD];
            m_CurrentProcedure.OnEnter();
        }
        m_CurrentProcedure.Update(deltaTime);

        
    }

    public EliminatePlayer GetEliminatePlayer()
    {
        return m_Player;
    }
	


}

