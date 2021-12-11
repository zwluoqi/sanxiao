using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;

public class CopyBeforeController : Page {
	
	public UILabel level;
    public GameObject startButton;
    public GameObject power;
    private GameObject newPower;

	public List<UIMissionView> m_MisIcon;
	public List<GameObject> selectedEquips;
    public List<UIToggle> stars;
	public GameObject targetScore;
    public GameObject targetReq;
    public GameObject targetTFMove;
    private const int buyPower = 5;
    private bool animtioning = false;

    private bool loadCompleted = false;

    protected override void DoOpen()
    {	
        animtioning = false;
        int currentLevel = LocalDataBase.Instance().GetSelectCopyLevel();
		level.text = currentLevel.ToString();
		
		
		foreach(GameObject obj in selectedEquips){
			obj.GetComponent<UIToggle>().value = false; 
		}
		
		Tab_Copydetail copy = TableManager.GetCopydetailByID(currentLevel);

        if (copy.CopyMode == (int)CopyMode.ElE)
        {
            targetTFMove.SetActive(false);
            targetScore.SetActive(true);
            targetReq.SetActive(true);

            //初始化元素任务图标及类型;
            int MissionCount = 0;
            string strIconName = "";
            int requestMissionid = -1;
            int requestMissionnum = -1;

            for (int i = 0; i < 4; i++)
            {
                requestMissionid = copy.GetRequestMissionIDbyIndex(i);
                requestMissionnum = copy.GetRequestMissionNumbyIndex(i);
                if (requestMissionid != -1)
                {
                    MissionCount++;
                    strIconName = TableManager.GetMissionByID(requestMissionid).SpriteName;
                    if (MissionCount <= m_MisIcon.Count)
                    {
                        m_MisIcon[MissionCount - 1].gameObject.SetActive(true);
                        m_MisIcon[MissionCount - 1].Init(requestMissionid, requestMissionnum);
                        m_MisIcon[MissionCount - 1].m_sprite.spriteName = strIconName; //itemAtlas.spriteList[2].name;
                        m_MisIcon[MissionCount - 1].SetUIForBefore((CopyMode)copy.CopyMode);
                    }
                    else
                    {
                        Debug.LogError("do not have space view");
                        break;
                    }
                }
            }

            //多余的不显示;
            for (; MissionCount < 4; MissionCount++)
            {
                m_MisIcon[MissionCount].gameObject.SetActive(false);
            }


            //目标分数显示;
            if (copy.ScoreRequest > 0)
            {
                targetScore.SetActive(true);
                targetScore.transform.Find("Score").GetComponent<UILabel>()
                    .text = copy.ScoreRequest.ToString();
            }
            else
            {
                targetScore.SetActive(false);
            }

        }
        else
        {
            targetTFMove.SetActive(true);
            targetScore.SetActive(false);
            targetReq.SetActive(false);
            Tab_TourContent tour = TableManager.GetTourContentByID(copy.TFID);
            targetTFMove.transform.Find("icon").GetComponent<UISprite>().spriteName = TableManager.GetSquareByID(tour.TFYinXiongSquareId).SpriteName;
        }

        //显示已经拥有的星级
        int star = LocalDataBase.GetCopyStar(currentLevel);
        for (int i = 0; i < stars.Count; i++)
        {
            if (i < star)
            {
                stars[i].activeSprite.alpha = 1;
            }
            else
            {
                stars[i].activeSprite.alpha = 0;
            }
        }
        StartCoroutine(LoadMapData(copy.MapName,copy.GuildEleData));

        #region guild
        if (PlayerPrefs.GetInt("CopyBeforeController", -1) != 1 || (LocalDataBase.equipGuild && LocalDataBase.equipbuy == 7))
        {
            PlayerPrefs.SetInt("CopyBeforeController", 1);
            LocalDataBase.equipbuy++;
            LocalDataBase.equipWaitInput = 1;
            GuideManager.Instance.ShowForceGuide(startButton, true, "", Vector3.zero);
        }
        #endregion
	}


    private IEnumerator LoadMapData(string mapName,string guildEleData)
    {
        if (!LevelData.copyTextasset.ContainsKey(mapName))
        {
            TextAsset mapText = Resources.Load("Maps/" + mapName) as TextAsset;
            LevelData.copyTextasset.Add(mapName, mapText);
        }
        yield return null;
        if (!LevelData.guildTextasset.ContainsKey(guildEleData) && guildEleData != "None")
        {
            TextAsset guildData = Resources.Load("Maps/" + guildEleData) as TextAsset;
            LevelData.guildTextasset.Add(guildEleData, guildData);
        }
    }
	
	public void OnCloseBtn(){
        if (animtioning)
        {
            return;
        }
        this.Close();
	}
	
	public void OnEnsureBtn(){
        #region guild
        GuideManager.Instance.HideGuide();
        #endregion

        if (animtioning)
        {
            return;
        }
        if (LocalDataBase.Instance().GetDataNum(DataType.power) < LocalDataBase. costPower)
        {
            //if (!LocalDataBase.HasShareToday())
            //{
                BoxManager.Instance.ShowPowerShareMessage();
                UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += ShareBtn;
                UIEventListener.Get(BoxManager.Instance.buttonCancle).onClick += CancleShareBtn;
            //    return;
            //}
            //else
            //{
            //    BoxManager.Instance.ShowBuyPowerMessage();
            //    UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += BuyTimes;
            //    return;
            //}
//            BoxManager.Instance.ShowBuyPowerMessage();
//            UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += BuyTimes;
            return;
		}

        int currentLevel = LocalDataBase.Instance().GetSelectCopyLevel();
        Tab_Copydetail copy = TableManager.GetCopydetailByID(currentLevel);
        if (LocalDataBase.GetAllStars() < copy.OpenedLimited && !LocalDataBase.copyModels[currentLevel-1].buyUnLock)
        {
            //BoxManager.Instance.ShowPopupMessage(string.Format("需要{0}颗星星才能开启此关卡", copy.OpenedLimited));
            PageManager.Instance.OpenPage("EnoughPage", "");
            return;
        }

        LocalDataBase.Instance().DecreaseDataNum(DataType.power, LocalDataBase.costPower);
        animtioning = true;
        GameObject powerIcon = SceneManager.Instance.UI.GetComponentInChildren<InfoController>().power.transform.parent.Find("icon").gameObject;
        newPower = GameObject.Instantiate(powerIcon) as GameObject;
        newPower.transform.parent = powerIcon.transform.parent;
        newPower.SetActive(true);
        newPower.transform.localPosition = powerIcon.transform.localPosition;
        newPower.transform.localScale = powerIcon.transform.localScale;

        newPower.transform.parent = gameObject.transform;

        iTweenHandler.PlayToPos(newPower, powerIcon.transform.position, power.transform.position, 1, true);
        TimeEventObject timeEvent = newPower.AddComponent<TimeEventObject>();
        timeEvent.SetOnTimeFinished(PowerFlyCallBack, 1.1f);
        LoadResource();
    }

    void Update()
    {
        if (!animtioning && loadCompleted)
        {
            animtioning = false;
            loadCompleted = false;
            PageManager.Instance.CloseAllPage();
            SoundEffect.Instance.PlaySound(SoundEffect.tiliHip);
            SceneManager.Instance.UIToGame();
        }
    }

    private void PowerFlyCallBack(GameObject go) {
        Destroy(newPower);
        animtioning = false; 
    }

    private void LoadResource()
    {
        //加载特效缓冲
        EffectManager.Instance.CreateEffectCache();
        EffectManager.Instance.m_EffectCacheList.TryGetValue(EliminateLogic.flyParticleAnimationID, out EliminateLogic.Instance.GetEliminatePlayer().flyIns);
        EffectManager.Instance.m_EffectCacheList.TryGetValue(EliminateLogic.xunzhuanParticleAnimationID, out EliminateLogic.Instance.GetEliminatePlayer().xuanZhuanIns);

        EliminateLogic.Instance.StartCoroutine(EffectManager.Instance.LoadResources());
        EliminateLogic.Instance.StartCoroutine(LoadItemAndSquareWidgets());
        SoundEffect.Instance.PreLoadSoundResource();
    }

    public bool LoadCompleted()
    {
        return EffectManager.Instance.LoadResouceCompleted() && loadCompleted;
    }

    IEnumerator LoadItemAndSquareWidgets()
    {
        loadCompleted = false;
        WidgetBufferManager.Instance.PreLoadWidget(UISquareView.parentPrefabStr, 81, null);
        yield return null;
        WidgetBufferManager.Instance.PreLoadWidget(UISquareView.prefabStr, 196, null);
        yield return null;
        WidgetBufferManager.Instance.PreLoadWidget(UIEliminateItemView.prefabStr, 81, null);
        yield return null;
        WidgetBufferManager.Instance.PreLoadWidget("Game/Score", 20, null);
        yield return null;
        WidgetBufferManager.Instance.PreLoadWidget("Game/EleTextEffectPref", 4, null);
        yield return null;
        WidgetBufferManager.Instance.PreLoadWidget(UIEliminateItemView.lineSpriteName, 4, null);
        yield return null;
        WidgetBufferManager.Instance.PreLoadWidget(UIEliminateItemView.borderSpriteName, 4, null);
        yield return null;
        WidgetBufferManager.Instance.PreLoadWidget("Game/FlyEquipItem", 4, null);
        yield return null;
        WidgetBufferManager.Instance.PreLoadWidget("Game/FlyMissionItem", 10, null);
        yield return null;
        WidgetBufferManager.Instance.PreLoadWidget("SoundObject", 5, null);
        yield return null;
        loadCompleted = true;
    }

	public void BuyTimes(GameObject go){
        pageManager.OpenPage("ShopController", "shopType="+(int)ShopType.Tili);
	}

    public void ShareBtn(GameObject go)
    {
		ADTouTiao.Instance.RequestRewardVideo(OnPlayDone);
    }

	void OnPlayDone ()
	{
		LocalDataBase.Instance().AddDataNum(DataType.power, 3);
	}

    public void CancleShareBtn(GameObject go)
    {
        StartCoroutine(YieldCancleShareBtn());
    }

    IEnumerator YieldCancleShareBtn()
    {
        yield return null;
        BoxManager.Instance.ShowBuyPowerMessage();
        UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += BuyTimes;
    }

	public void BuySpecitalEquip(){

	}

	public void BuyAddThreeStepEquip(){


	}

	public void BuyMiracleEquip(){


	}


}
