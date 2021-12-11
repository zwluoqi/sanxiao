using UnityEngine;
using System.Collections;
//using sanxiao.net;
//using xjgame.message;
using GCGame.Table;
using System.Collections.Generic;
using System;
using XZXD;
using Com.Communication;


public class LoginController : MonoBehaviour {

    public GameObject loginButton;
	public GameObject quickButton;
	public UISlider sceneLoading;
	private MapController m_mapController;

    public UITexture[] texturesOfPage;
    public UIAtlas[] atlasOgPage;
    void Awake()
    {
//        if (SystemConfig.Instance.platformID == ComPlarmID.AppleStore)
//        {
//            loginButton.SetActive(false);
//        }
//        else
//        {
//            loginButton.SetActive(true);
//        }
		loginButton.SetActive(true);
		NativeCaller.loadDeviceInfo ();
    }


    void Start()
    {
		if (PlayerPrefs.GetInt("first",0)==0) {
			PlayerPrefs.SetInt ("first", 1);
			LanguageManger.GetMe ().ExchangeLanguage (LanguageType.LANGUAGE_CHINESE);
		}
        m_mapController = SceneManager.Instance.UI.GetComponent<UIController>().map;
        quickButton.SetActive(true);
        sceneLoading.gameObject.SetActive(false);

        DateTime current = DateTime.Now;
        Debug.Log(current);
        DateTime ss;
        DateTime.TryParse(current.ToString(), out ss);
        Debug.Log(ss);
		ADTouTiao.Instance.LoadFullScreenVideoAd ();
//		NetManager.ResetZoneUrl ("http://114.67.88.112:8080/xzxdweb/");
		NetManager.ResetZoneUrl ("http://118.190.209.198:8080/xzxdweb/");
		NetCheck ();
		//		"http://118.190.209.198:8080/xzxdweb/";
    }

    void Update()
    {
        if (!m_mapController.isDone)
        {
            sceneLoading.value = m_mapController.progress;
        }
    }

	public void OnStartBtn(){
        quickButton.SetActive(false);
        loginButton.SetActive(false);
        sceneLoading.gameObject.SetActive(true);
        SceneManager.Instance.StartCoroutine(_OnStart());
    }

    public void OnLoginBtn()
    {
        quickButton.SetActive(false);
        loginButton.SetActive(false);
        sceneLoading.gameObject.SetActive(true);
        SceneManager.Instance.StartCoroutine(_OnStart());
        SDKObjecty.Init();

    }

    public void OnUnLoadAssets()
    {
        foreach (UITexture ut in texturesOfPage)
        {
            Resources.UnloadAsset(ut.mainTexture);
        }
        foreach (UIAtlas ua in atlasOgPage)
        {
            Resources.UnloadAsset(ua.spriteMaterial.mainTexture);
        }
    }


    IEnumerator _OnStart()
    {

        LocalDataBase.InitCopyDatas();

        yield return null;
        //请求网络数据;
        //SendBeginGame(BeginGameCallBack,false);
        SceneManager.Instance.StartCoroutine(LoadResource());
        yield return null;
        //加载音乐缓冲
        AudioManger.Instance.PreLoadAudioResource();
        yield return null;
        //加载音效缓冲
        //SoundEffect.Instance.PreLoadSoundResource();
        //yield return null;
        //加载特效缓冲
        //EffectManager.Instance.CreateEffectCache();
        //yield return null;
        //加载对象缓冲;
        //SceneManager.Instance.StartCoroutine(LoadItemAndSquareWidgets());
        //yield return null;

    }

//    public static void SendBeginGame(UIListener.OnReceive funReceive,bool isblock)
//    {
//        CSBeginGame msg = (CSBeginGame)PacketDistributed.CreatePacket(MessageID.CSBeginGame);
//        msg.DeviceKey = SystemInfo.deviceUniqueIdentifier;
//		msg.PlatformId = (int)SystemConfig.Instance.platformId;
//        if (!string.IsNullOrEmpty(LocalDataBase.plarmformID))
//        {
//            msg.PlatformUserId = long.Parse(LocalDataBase.plarmformID);
//        }
//        msg.ZhuanshuNum = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi);
//        msg.JinbiNum = LocalDataBase.Instance().GetDataNum(DataType.jinbi);
//        msg.PowerNum= LocalDataBase.Instance().GetDataNum(DataType.power);
//        msg.ChannelId = 1;
//        foreach(CopyDataModel copyModel in LocalDataBase.copyModels){
//            CopyData copydata = new CopyData();
//            copydata.Copyid = copyModel.copyID;
//            copydata.Star = copyModel.star;
//            msg.AddCopys(copydata);
//        }
//        List<EquipData> equips = LocalDataBase.Instance().GetLocalEquipData();
//        foreach (EquipData equip in equips)
//        {
//            msg.AddEquips(equip);
//        }
//        NetworkSender.Instance().send(funReceive, msg, isblock);
//    }
//
//    //正常登陆流程
//    public void BeginGameCallBack(bool success, PacketDistributed packet)
//    {
//        if (success)
//        {
//            CancelInvoke("FailedBeginGame");
//            SCBeginGame msg = (SCBeginGame)packet;
//            Debug.Log("Login success:" + msg.Pid);
//            LocalDataBase.pid = msg.Pid;
//            LocalDataBase.Instance().SetDataNum(DataType.zhuanshi, msg.ZhuanshuNum);
//            LocalDataBase.Instance().SetDataNum(DataType.jinbi, msg.JinbiNum);
//            LocalDataBase.Instance().SetDataNum(DataType.power, msg.PowerNum);
//            List<CopyData> copys = new List<CopyData>();
//            foreach (CopyData copy in msg.copysList)
//            {
//                copys.Add(copy);
//            }
//            List<EquipData> equips = new List<EquipData>();
//            foreach (EquipData equip in msg.equipsList)
//            {
//                equips.Add(equip);
//            }
//            LocalDataBase.InitCopyDatas(copys);
//            LocalDataBase.InitEquipData(equips);
//            SceneManager.Instance.StartCoroutine(LoadResource());
//
//        }
//    }


    private void FailedBeginGame()
    {
        Debug.Log("Login failed:");
        SceneManager.Instance.StartCoroutine(LoadResource());
    }


    private IEnumerator LoadResource()
    {
        //加载地图;
        yield return SceneManager.Instance.StartCoroutine(m_mapController.GeneroCopyItem2());

        SceneManager.Instance.LoginToUI();

        //音效控制初始化
        InitAudio();

        #region guild
        if (PlayerPrefs.GetInt("initData_1", -1) != 1)
        {
            PlayerPrefs.SetInt("initData_1", 1);
            PageManager.Instance.OpenPage("StartGiftPage", "");
            //GA.SetUserInfo(SystemInfo.deviceUniqueIdentifier, GA.Gender.Unknown, 0, SystemConfig.Instance.platformID.ToString());
        }
        else if(LocalDataBase.copyModels[14].star >= 0)
        {

            int randomGift = UnityEngine.Random.Range(1, 25);
            if (randomGift >= 1 && randomGift <= 5)
            {
                PageManager.Instance.OpenPage("libao" + randomGift, "");
            }
        }
        #endregion
    }

    IEnumerator LoadItemAndSquareWidgets()
    {
        WidgetBufferManager.Instance.PreLoadWidget(UISquareView.parentPrefabStr,81, null);
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
        WidgetBufferManager.Instance.PreLoadWidget("SignRewardItem", 31, null);
        yield return null;
        WidgetBufferManager.Instance.PreLoadWidget("SoundObject", 5, null);
        yield return null;
    }


    public void InitAudio()
    {
        if (LocalDataBase.Instance().GetSysState(SystemLock.Music))
        {
            AudioManger.Instance.Open();
        }
        else
        {
            AudioManger.Instance.Close();
        }

        if (LocalDataBase.Instance().GetSysState(SystemLock.MusicP))
        {
            SoundEffect.Instance.TurnVolume(1);
        }
        else
        {
            SoundEffect.Instance.TurnVolume(0);
        }
    }

//
//    /// <summary>
//    /// 强制登陆数据同步;
//    /// </summary>
//    /// <param name="success"></param>
//    /// <param name="packet"></param>
//    public static void ReLoginSyncData(bool success,PacketDistributed packet)
//    {
//        if (success)
//        {
//            SCBeginGame msg = (SCBeginGame)packet;
//            Debug.Log("ReLogin success:" + msg.Pid);
//            LocalDataBase.pid = msg.Pid;
//            LocalDataBase.Instance().SetDataNum(DataType.zhuanshi, msg.ZhuanshuNum);
//            LocalDataBase.Instance().SetDataNum(DataType.jinbi, msg.JinbiNum);
//            LocalDataBase.Instance().SetDataNum(DataType.power, msg.PowerNum);
//            List<CopyData> copys = new List<CopyData>();
//            foreach (CopyData copy in msg.copysList)
//            {
//                copys.Add(copy);
//            }
//            List<EquipData> equips = new List<EquipData>();
//            foreach (EquipData equip in msg.equipsList)
//            {
//                equips.Add(equip);
//            }
//            LocalDataBase.InitCopyDatas(copys);
//            LocalDataBase.InitEquipData(equips);
//        }
//    }

    //public void SendBuyPower(UIListener.OnReceive funReceive)
    //{
    //    CSBuyPower msg = (CSBuyPower)PacketDistributed.CreatePacket(MessageID.CSBuyPower);
    //    NetworkSender.Instance().send(funReceive, msg, true);
    //}

	void NetCheck ()
	{

		SendCheckVersion ();
	}


	public void SendCheckVersion(){
		Com.Communication.CSCheckVersion csmsg = new Com.Communication.CSCheckVersion ();
		csmsg.appVersion = VersionTool.appVersion;
		csmsg.funVersion = VersionTool.funVersion;
		csmsg.channelId = XZXD.NativeCaller.getChannelId ();
		csmsg.libcode = "";
		csmsg.deviceUniqueIdentifier = NativeCaller.GetDeviceId ();
		NetManager.Instance.SendHttp (OpDefine.CSCheckVersion, csmsg, delegate(NetWork.Layer.Packet receiveData, bool bSuccess) {
			if(bSuccess){
				SCCheckVersion scmsg = receiveData.kBody as SCCheckVersion;

				RunCoroutine.Run(HaHa(scmsg));

			}else{
//				BoxManager.Instance.CreatOneButtonBox ("确定", "连接超时,请检查网络环境",delegate(bool bo) {
//					SendCheckVersion();
//				});
			}
		});
	}

	public static Dictionary<string ,PBConfigData> globalConfigDatas = new Dictionary<string, PBConfigData> ();

	IEnumerator HaHa (SCCheckVersion scmsg)
	{
		yield return 1;
		for (int i = 0; i < scmsg.configData.Count; i++) {
			globalConfigDatas [scmsg.configData [i].name] = scmsg.configData [i];
		}
		if(scmsg.actionMethod == "update_app"){
//			Goto(LoginStateEnum.UpdateApp,scmsg.actionParam1);
			BoxManager.Instance.CreatOneButtonBox ("确定", "有新版本可供更新，前往更新哦",delegate(GameObject bo) {
				Application.OpenURL(scmsg.actionParam1);
			});
		}
		else if(scmsg.actionMethod == "select_update_app"){
//			Goto(LoginStateEnum.SelectUpdateApp,scmsg.actionParam1);
		}
		else{

			CSLogin msg = new CSLogin ();
			msg.activeCode = "0";
			msg.authCode = NativeCaller.GetDeviceId ();
			msg.authPass = NativeCaller.GetDeviceId ();
			msg.loginType = 0;
			msg.platformUid = NativeCaller.getChannelId ();
			msg.zoneID = 999;

			msg.deviceModel = SystemInfo.deviceModel;
			msg.deviceName = SystemInfo.deviceName;
			msg.deviceType = SystemInfo.deviceType.ToString ();
			msg.operatingSystemFamily = SystemInfo.operatingSystemFamily.ToString ();
			msg.operatingSystem = SystemInfo.operatingSystem;
			msg.deviceUniqueIdentifier = NativeCaller.GetDeviceId ();


			NetManager.Instance.SendHttp (OpDefine.CSLogin, msg, delegate(NetWork.Layer.Packet receiveData, bool bSuccess) {
				if (bSuccess) {
					SCLogin scMsg = receiveData.kBody as SCLogin;
					//
					PacketBundle.m_sAccountLoginKey = scMsg.accountKey.ToString ();
					PacketBundle.m_nServerID = scMsg.zoneID;
					ServerTimerTool.CorrectTime (scMsg.serverTimeNow);
					var time = ServerTimerTool.Java2CSTime(scMsg.serverTimeNow);
					Debug.LogError(time.ToString());
//
//					GrowFun.Ins.SetAccount(scMsg.zoneID,scMsg.instAccount);
//
//					CacheData.SetLoginId (scMsg.instAccount.accountName);
//					CacheData.SetLoginPassward (scMsg.instAccount.loginPassawrd);
//					CacheData.SetLeatestLoginServerZoneId(scMsg.zoneID.ToString());
//					CacheData.AddLeatestLoginServerZoneS(scMsg.zoneID.ToString());
//					XZXDDebug.LogWarning ("登录成功");
//
//
//
//					StartBeginGame ();
				} else {
//					cacheDatas.Clear();
				}
			});

		}
	}
}
