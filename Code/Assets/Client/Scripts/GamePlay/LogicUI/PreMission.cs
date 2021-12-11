using UnityEngine;
using System.Collections;
using GCGame.Table;

public class PreMission : MonoBehaviour {
    public GameObject childs;
    public UIGrid grid;
    public UISprite[] missionIcon;
    public UILabel copyLevel;
    public UILabel missionScore;
    public GameObject targetScore;

    [HideInInspector]
    public bool playCompleted = false;

    public void Init()
    {
        int MissionCount = 0;
        string strIconName = "";
        Mission requestMission = null;


        //初始化元素任务图标及类型;
        int allMissionCount = LevelData.GetMissionCount();

        for (int i = 0; i < allMissionCount; i++)
        {
            requestMission = LevelData.GetMissionByIndex(i);
            if (requestMission != null && TableManager.GetMissionByID(requestMission.type).DisplayAtTop == 1)
            {
                MissionCount++;
                strIconName = TableManager.GetMissionByID(requestMission.type).SpriteName;
                if (MissionCount <= missionIcon.Length)
                {
                    missionIcon[MissionCount - 1].gameObject.SetActive(true);
                    missionIcon[MissionCount - 1].spriteName = strIconName;
                }
                else
                {
                    SystemConfig.LogError("do not have space view");
                    break;
                }
            }
        }

        //多余的不显示;
        for (; MissionCount < missionIcon.Length; MissionCount++)
        {
            missionIcon[MissionCount].gameObject.SetActive(false);
        }
        grid.repositionNow = true;

        //目标分数显示;
        if (LevelData.requestScore > 0)
        {
            targetScore.SetActive(true);
            missionScore
                .text = LevelData.requestScore.ToString();
        }
        else
        {
            targetScore.SetActive(false);
        }

        //关卡
        copyLevel.text = string.Format(LanguageManger.GetMe().GetWords("L_1077"), LevelData.currentLevel.ToString());

        
    }

    public void PlayPreMissionAnimation()
    {
        playCompleted = false;
        TweenPosition tp = childs.GetComponent<TweenPosition>();
        tp.SetOnFinished(Onfinished);
        tp.ResetToBeginning();
        tp.PlayForward();
    }

    public void Onfinished()
    {
        playCompleted = true;
    }
}
