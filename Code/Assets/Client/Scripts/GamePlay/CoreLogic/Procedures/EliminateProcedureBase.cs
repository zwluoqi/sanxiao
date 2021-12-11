using UnityEngine;
using System.Collections;


    public enum EliminateProcedureType
    {
        PROCEDURE_RESOURCE_LOAD,//资源预加载
		PROCEDURE_PRODUCEMISSION,//生成任务及相关item
		PROCEDURE_CHECK_HITS,//检测可消除项
		PROCEDURE_WAITING_INPUT,//等待玩家操作
		PROCEDURE_ELIMINATING,//进行消除处理
		PROCEDURE_NOMATCHING,//重新生成相关item
		PROCEDURE_VICTORY,//游戏胜利
		PROCEDURE_DEFEATED,//游戏失败
		PROCEDURE_PREDEFEATED,//结束前置判断;
		PROCEDURE_PREVICTORY,//胜利前置动画;
		PROCEDURE_END,//结束游戏
        EliminateProcedureEquipReset,//道具重置元素
        EliminateProcedureFontStory,//前置剧情介绍
	
    }


    public abstract class EliminateProcedureBase
    {
        protected EliminateProcedureBase()
        {
        }

        public abstract EliminateProcedureType GetProcedureType();
        public abstract bool Init(EliminateProcedureManager manager);
        public abstract void OnEnter();
        public abstract void OnLeave();
        public abstract void Update(float deltaTime);
    }





