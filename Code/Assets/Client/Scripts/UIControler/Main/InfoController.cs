using UnityEngine;
using System.Collections;
using System;

public class InfoController : MonoBehaviour {

    public TweenPosition topTween;
    public TweenPosition bottomTween;
    public GameObject shopBtn;
    public GameObject bagBtn;
	public UILabel power;
	public UILabel star;
	public UILabel zhuanshi;
	
	public UILabel remainedTime;

    [HideInInspector]
    public TimeSpan remainedTimeSpan;

	public static DateTime lastAddTime ;

	private MapController mapController;
    public GameObject resetObj;
    public GameObject unlockObj;

	public GameObject guanggaoBtn;

    private static int counter;
    public void OnClickBg()
    {
//        counter++;
//        if (counter > 10)
        {
			resetObj.SetActive(false);
			unlockObj.SetActive(false);
        }
    }

	void Awake(){
        mapController = SceneManager.Instance.UI.GetComponentInChildren<MapController>();
		lastAddTime = LocalDataBase.Instance().GetLastDateTime();
		//InvokeRepeating("UpdateTime",0,1);
	}
	
	void Update(){
		power.text = LocalDataBase.Instance().GetDataNum(DataType.power).ToString();
        star.text = LocalDataBase.GetAllStars().ToString();
		zhuanshi.text = LocalDataBase.Instance().GetDataNum(DataType.zhuanshi).ToString();
		if(LocalDataBase.Instance().GetDataNum(DataType.power) >= LocalDataBase.maxPower){
			remainedTime.text = "";
            lastAddTime = DateTime.Now;
			LocalDataBase.Instance().SetCurrentTime(DateTime.Now);
            remainedTimeSpan = TimeSpan.Zero;
		}else{
            int remainedSecond = UpdateTime();
            remainedTimeSpan = new TimeSpan(0, 0, remainedSecond);
            remainedTime.text = string.Format(LanguageManger.GetMe().GetWords("L_1003"), remainedTimeSpan.Minutes, remainedTimeSpan.Seconds);
        }

        #region guild
        if (LocalDataBase.equipGuild)
        {
            if (LocalDataBase.equipbuy == 1)
            {
                LocalDataBase.equipbuy++;
                GuideManager.Instance.ShowForceGuide(shopBtn,
                    true, LanguageManger.GetMe().GetWords("L_daoju_tishi1"), Vector3.zero);
            }
            else if (LocalDataBase.equipbuy == 5)
            {
                LocalDataBase.equipbuy++;
                GuideManager.Instance.ShowForceGuide(bagBtn,
                    true, LanguageManger.GetMe().GetWords("L_daoju_tishi5"), Vector3.zero);
            }
        }
        #endregion
    }
	
	int UpdateTime(){
		DateTime now = DateTime.Now;
		
		TimeSpan span = now - lastAddTime;
        if (span.TotalSeconds > LocalDataBase.coolDownSecond)
        {
            if (LocalDataBase.Instance().GetDataNum(DataType.power)  < LocalDataBase.maxPower)
            {
                if (LocalDataBase.Instance().GetDataNum(DataType.power) + span.TotalSeconds / LocalDataBase.coolDownSecond < LocalDataBase.maxPower)
                {
                    LocalDataBase.Instance().AddDataNum(DataType.power, (int)span.TotalSeconds / LocalDataBase.coolDownSecond);
                }
                else
                {
                    LocalDataBase.Instance().SetDataNum(DataType.power, LocalDataBase.maxPower);
                }

            }
			lastAddTime = now;
			LocalDataBase.Instance().SetCurrentTime(lastAddTime);
		}
        return LocalDataBase.coolDownSecond - (int)span.TotalSeconds;
	}


	public void UnLockedcopy(){
		mapController.UnlockedCopy();
	}

	public void ResetCopy(){
		mapController.ResetCopy();
	}

	public void OnTiliBtn(){
        PageManager.Instance.OpenPage("ShopController", "shopType=" + (int)ShopType.Tili);

	}

	public void OnZhuanshiBtn(){
//        PageManager.Instance.OpenPage("ShopController", "shopType=" + (int)ShopType.Zhuanshi);

    }

	public void OnShopBtn(){
        GuideManager.Instance.HideGuide();
        PageManager.Instance.OpenPage("ShopController", "shopType=" + (int)ShopType.Equip);

        //if(LocalDataBase.copyModels[14].star >= 0){
        //    PageManager.Instance.OpenPage("ShopController", "shopType="+(int)ShopType.Equip);
        //}
        //else
        //{
        //    BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords( "L_Module_Level_Limited1"));
        //}
    }

	public void OnBagBtn(){
        GuideManager.Instance.HideGuide();
        PageManager.Instance.OpenPage("BagListController", "");

        //if (LocalDataBase.copyModels[14].star >= 0)
        //{
        //    PageManager.Instance.OpenPage("BagListController", "");
        //}
        //else
        //{
        //    BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_Module_Level_Limited5"));
        //}
	}

    public void OnLibaoBtn()
    {
        PageManager.Instance.OpenPage("ShopController", "shopType=" + (int)ShopType.Libao);

        //if (LocalDataBase.copyModels[14].star >= 0)
        //{
        //    PageManager.Instance.OpenPage("ShopController", "shopType="+(int)ShopType.Libao);
        //}
        //else
        //{
        //    BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_Module_Level_Limited1"));
        //}
    }

	public void OnSettingBtn(){
        PageManager.Instance.OpenPage("SettingController","");

	}

    public void OnZhuanpanBtn()
    {
        PageManager.Instance.OpenPage("ZhuanPanController", "");

        //if (LocalDataBase.copyModels[14].star >= 0)
        //{
        //    PageManager.Instance.OpenPage("ZhuanPanController", "");
        //}
        //else
        //{
        //    BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_Module_Level_Limited2"));
        //}
    }

    public void OnSignBtn()
    {
        bool getTimeRight = false;
        System.DateTime dt = TimeHelper.GetBeijingTime(out getTimeRight);
        if (getTimeRight)
        {
            Debug.Log(dt);
            SignRewardController.InitSignRewards(dt);
            PageManager.Instance.OpenPage("SignRewardController", "");
        }
    }

	public void OnGuanggao(){
		BoxManager.Instance.ShowGuanggaoMessage ();
		UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += OnPlay;

	}

	void OnPlay (GameObject go)
	{
		ADTouTiao.Instance.RequestRewardVideo (OnPlayDone);
	}

	void OnPlayDone ()
	{
		LocalDataBase.Instance ().AddDataNum (DataType.zhuanshi, 5);
		BoxManager.Instance.ShowPopupMessage ("获得5点钻石");
	}
}
