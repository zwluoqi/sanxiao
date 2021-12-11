using UnityEngine;
using System.Collections;
using GCGame.Table;

using System.Collections.Generic;
public class VictoryController : Page {
	
	//public UILabel star;
	public GameObject[] stars;
	public UILabel score;
	public UILabel jinbiValue;
	public UILabel powerValue;
	public UILabel level;
    public GameObject nextLevelBtn;
    private EffectBase particle;
    private int currentPat;
    private bool equipGuide = false;

    void Awake()
    {
        particle = new EffectParticle(gameObject, "xingxingtexiao", "");
        particle.durationTime = 0.5f;
        particle.delaydestroyTime = 0.51f;
        particle.delayStartTime = 0;
        
    }
	

	public void OnNextBtn()
    {
        #region guild
        GuideManager.Instance.HideGuide();
        #endregion
        this.Close();
        //设置选择下一关;
        LocalDataBase.Instance().SetSelectCopyLevel(LevelData.currentLevel + 1);
        //Umeng.GA.SetUserLevel((LevelData.currentLevel + 1).ToString());
		EliminateLogic.Instance.GetEliminatePlayer().ReturnToMain();

        Tab_Copydetail nextCopy = TableManager.GetCopydetailByID(LevelData.currentLevel + 1);
        if (equipGuide)
        {
            #region guild
            LocalDataBase.equipGuild = true;
            LocalDataBase.equipbuy = 1;
            LocalDataBase.equipWaitInput = 1;
			LevelData.AutoOpenNextLevel = false;
            #endregion
        }
        else{
			LevelData.AutoOpenNextLevel = true;
		}
	}

    public void OnHomeBtn()
    {
        this.Close();
        LocalDataBase.Instance().SetSelectCopyLevel(LevelData.currentLevel + 1);
        EliminateLogic.Instance.GetEliminatePlayer().ReturnToMain();
        Tab_Copydetail nextCopy = TableManager.GetCopydetailByID(LevelData.currentLevel + 1);
        if (equipGuide)
        {
            #region guild
            LocalDataBase.equipGuild = true;
            LocalDataBase.equipbuy = 1;
            LocalDataBase.equipWaitInput = 1;
            #endregion
        }
        LevelData.AutoOpenNextLevel = false;
    }

    public void OnContinueBtn()
    {
        int power = LocalDataBase.Instance().GetDataNum(DataType.power);
        if (power > LocalDataBase.costPower)
        {
            this.Close();
            LocalDataBase.Instance().DecreaseDataNum(DataType.power, LocalDataBase.costPower);
            EliminateLogic.Instance.GetEliminatePlayer().ReStartLevel();
        }
        else
        {
            BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_1052"));
        }
    }

    protected override void DoOpen()
    {	
				particle.LoadResource();
        currentPat = 0;
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].transform.Find("hover").GetComponent<TweenScale>().ResetToBeginning();
            stars[i].transform.Find("hover").gameObject.transform.localScale = Vector3.zero;
            stars[i].transform.Find("hover").gameObject.SetActive(false);
            stars[i].transform.Find("normal").gameObject.SetActive(true);
        }
        int star = MissionManager.Instance.GetResultStart();
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < star)
            {
                stars[i].transform.Find("hover").gameObject.SetActive(true);
                if (i == 0)
                {
                    stars[i].transform.Find("hover").GetComponent<TweenScale>().PlayForward();
                }
            }
            else
            {
                stars[i].transform.Find("hover").gameObject.SetActive(false);
            }
        }

        //结算数据;
        int thisResult = MissionManager.Instance.GetResultStart();
        var dict = new Dictionary<string, string>();
        dict ["lelve"] = LocalDataBase.Instance().GetSelectCopyLevel().ToString();
        dict ["star"] = thisResult.ToString();
        //dict ["id"] = SystemInfo.deviceUniqueIdentifier;
        //GA.Event("xinji", dict);
        int gold = EliminateLogic.Instance.GetEliminatePlayer().getGoldNum + MissionManager.Instance.GetResultGoldNum();
        int power = EliminateLogic.Instance.GetEliminatePlayer().getPower + thisResult;
        int baoshi = EliminateLogic.Instance.GetEliminatePlayer().getZhuanshiNum;
        LocalDataBase.Instance().AddDataNum(DataType.jinbi, gold);
        LocalDataBase.Instance().AddDataNum(DataType.power, power);
        LocalDataBase.Instance().AddDataNum(DataType.zhuanshi, baoshi);

        level.text = LocalDataBase.Instance().GetSelectCopyLevel().ToString();

        score.text = MissionManager.Instance.completedScore.ToString();

        jinbiValue.text = gold.ToString();
        powerValue.text = power.ToString();

		Tab_Copydetail nextCopy = TableManager.GetCopydetailByID(LevelData.currentLevel + 1);

		if (nextCopy.GuildLevel == 6 && PlayerPrefs.GetInt("EquipGuild", -1) != 1)
		{
			#region guild
            PlayerPrefs.SetInt("EquipGuild", 1);
            equipGuide = true;
			if (LocalDataBase.Instance().GetDataNum(DataType.zhuanshi) < 40)
			{
				LocalDataBase.Instance().AddDataNum(DataType.zhuanshi, 40);
			}
			if (LocalDataBase.Instance().GetEquipNum(EquipEnumID.BomEffect) <= 0)
			{
				LocalDataBase.Instance().AddEquipNum(EquipEnumID.BomEffect, 1);
			}
			if (LocalDataBase.Instance().GetEquipNum(EquipEnumID.Hammer) <= 0)
			{
				LocalDataBase.Instance().AddEquipNum(EquipEnumID.Hammer, 1);
			}
			if (LocalDataBase.Instance().GetEquipNum(EquipEnumID.RowColEliminate) <= 0)
			{
				LocalDataBase.Instance().AddEquipNum(EquipEnumID.RowColEliminate, 1);
			}
			#endregion
        }
        else
        {
            equipGuide = false;
        }

        //Umeng.GA.FinishLevel("level" + LevelData.currentLevel);
	}

    public void OnFinishStayStar(GameObject go)
    {
        go.transform.parent.Find("normal").gameObject.SetActive(false);
        EffectBase m_particle = particle.Duplicate();
        m_particle.StartPos = go.transform;

        m_particle.EffectEnd = EffectEnd;

        SoundEffect.Instance.PlaySound(SoundEffect.missionCompletedEffect);
        m_particle.Play(go);
    }

    private void EffectEnd(EffectBase effect, GameObject target, float total_time)
    {
        currentPat++;
        if (currentPat >= stars.Length)
        {
            #region guild
            if (PlayerPrefs.GetInt("CopyEnd", -1) != 1)
            {
                PlayerPrefs.SetInt("CopyEnd", 1);
                GuideManager.Instance.ShowForceGuide(nextLevelBtn, false, "", Vector3.zero);
            }
            #endregion
        }
    }

    protected override void DoClose()
    {
        LevelData.WinGameToUI = true;
    }

	public void ShareBtn()
    {
		ADTouTiao.Instance.RequestRewardVideo(OnPlayDone);
    }

	void OnPlayDone ()
	{
		LocalDataBase.Instance().AddDataNum(DataType.power, 1);
	}
}
