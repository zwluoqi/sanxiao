using UnityEngine;
using System.Collections;




public class EliminateProcedurePreDefeated:EliminateProcedureBase
{
		private EliminateProcedureManager m_ProcedureManager = null;
		private EliminatePlayer m_player = null;

		private bool show = false;

		public override EliminateProcedureType GetProcedureType ()
		{
				return EliminateProcedureType.PROCEDURE_PREDEFEATED;
	
		}

		public override bool Init (EliminateProcedureManager manager)
		{
				SystemConfig.Log ("PROCEDURE_PREDEFEATED Init");
				m_ProcedureManager = manager;
				m_player = manager.GetEliminatePlayer ();
				return true;
		}

		public override void OnEnter ()
		{
				SystemConfig.Log ("PROCEDURE_PREDEFEATED OnEnter");
				EliminateLogic.Instance.StopTimeTick ();
				show = false;
		}

		public override void OnLeave ()
		{
				show = false;
				SystemConfig.Log ("PROCEDURE_PREDEFEATED OnLeave");
		}

		public override void Update (float deltaTime)
		{
				if (!show) {
						show = true;
//            PageManager.Instance.OpenPage("BeforeDefeatedController","");
						m_ProcedureManager.ChangProcedure (EliminateProcedureType.PROCEDURE_DEFEATED);
				}
		}
}





