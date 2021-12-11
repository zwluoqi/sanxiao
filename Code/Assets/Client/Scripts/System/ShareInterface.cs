using UnityEngine;
using System.Collections;
//using cn.sharesdk.unity3d;
using System;
using GCGame.Table;

public class ShareInterface : MonoBehaviour {

	public static ShareInterface Instance{
		get{
			return _instance;
		}
	}
	static ShareInterface _instance;
	void Awake(){
		_instance = this;
	}

	// Use this for initialization
	void Start () {
//        ShareSDK.setCallbackObjectName("Main Camera");
//		ShareSDK.open("6db79eaac85a");
//		
//        //Sina Weibo
//        //Hashtable sinaWeiboConf = new Hashtable();
//        //sinaWeiboConf.Add("app_key", "568898243");
//        //sinaWeiboConf.Add("app_secret", "38a4f8204cc784f81f9f0daaf31e02e3");
//        //sinaWeiboConf.Add("redirect_uri", "http://www.sharesdk.cn");
//        //ShareSDK.setPlatformConfig(PlatformType.SinaWeibo, sinaWeiboConf);
//
//        //QZone
//        //Hashtable qzConf = new Hashtable();
//        //qzConf.Add("app_id", "100371282");
//        //qzConf.Add("app_key", "aed9b0303e3ed1e27bae87c33761161d");
//        //ShareSDK.setPlatformConfig(PlatformType.QZone, qzConf);
//
//        //WeChat
//        Hashtable wcConf = new Hashtable();
//        wcConf.Add("app_id", "wx5d6dd2a4800b5f11");
//        wcConf.Add("app_secret", "204b71fe48693956ba5808102e3171af");
//        //ShareSDK.setPlatformConfig(PlatformType.WeChatSession, wcConf);
//        ShareSDK.setPlatformConfig(PlatformType.WeChatTimeline, wcConf);
        //ShareSDK.setPlatformConfig(PlatformType.WeChatFav, wcConf);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //public void ShareWeiChat()
    //{
    //    Hashtable content = new Hashtable();
    //    content["content"] = LanguageManger.GetMe().GetWords("L_1095");
    //    content["image"] = "http://ww1.sinaimg.cn/mw690/005ZcfVxgw1er9r29hr6dj30e80e8wg0.jpg";
    //    content["title"] = LanguageManger.GetMe().GetWords("L_1094");
    //    content["description"] = LanguageManger.GetMe().GetWords("L_1094");
    //    content["url"] = "http://mp.weixin.qq.com/s?__biz=MzAwMTQ3NzU1MA==&mid=205218909&idx=1&sn=8f41927ea6bd0f8fed56e9e22e2e0be4&scene=2&from=timeline&isappinstalled=0#rd";
    //    content["type"] = Convert.ToString((int)ContentType.News);
    //    content["siteUrl"] = SystemConfig.appurl;
    //    content["site"] = "iTunes";
    //    //content["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";

    //    ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
    //    ShareSDK.showShareMenu (null, content, 100, 100, MenuArrowDirection.Up, evt);
    //}

    public void OnReTryGiftShare(GameObject go)
    {
//        ShareWeiChatDirectly(1);
    }

    public void OnReTryPowerShare(GameObject go)
    {
//        ShareWeiChatDirectly(2);
    }

    public void ShareWeiChatDirectly(int share_type)
    {
//        return;
//        Hashtable content = new Hashtable();
//        content["content"] = LanguageManger.GetMe().GetWords("L_1095");
//        content["image"] = "http://ww1.sinaimg.cn/mw690/005ZcfVxgw1er9r29hr6dj30e80e8wg0.jpg";
//        content["title"] = LanguageManger.GetMe().GetWords("L_1094");
//        content["description"] = LanguageManger.GetMe().GetWords("L_1094");
//        content["url"] = "http://mp.weixin.qq.com/s?__biz=MzAwMTQ3NzU1MA==&mid=205271199&idx=1&sn=c870ba581c93efb29a1b75a7d375e893#rd";
//        content["type"] = Convert.ToString((int)ContentType.News);
//        content["siteUrl"] = SystemConfig.appurl;
//        content["site"] = "iTunes";
//        //content["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
//
//        ShareResultEvent evt = null;
//        if (share_type == 1)
//        {
//            evt = new ShareResultEvent(ShareResultHandlerDirectly);
//        }
//        else if(share_type == 2)
//        {
//            evt = new ShareResultEvent(ShareResultHandler);
//        }
//        ShareSDK.shareContent(PlatformType.WeChatTimeline, content,evt);
    }
	
//    void ShareResultHandler(ResponseState state, PlatformType type, Hashtable shareInfo, Hashtable error, bool end)
//    {
//        if (state == ResponseState.Success)
//        {
//            print("share result :");
//            print(MiniJSON.jsonEncode(shareInfo));
//			if(!LocalDataBase.HasShareToday()){
//				LocalDataBase.Instance().AddDataNum(DataType.power, 3);
//				BoxManager.Instance.ShowPopupMessage(string.Format( LanguageManger.GetMe().GetWords("L_1096"),3));
//			}
//			LocalDataBase.SetShareToday();
//            return;
//        }
//        else if (state == ResponseState.Fail)
//        {
//            print("fail! error code = " + error["error_code"] + "; error msg = " + error["error_msg"]);
//            BoxManager.Instance.ShowLibaoShareMessage(string.Format(LanguageManger.GetMe().GetWords("L_S009")));
//            UIEventListener.Get(BoxManager.Instance.buttonCancle).onClick += OnReTryPowerShare;
//        }
//        else if (state == ResponseState.Cancel)
//        {
//            print("cancel !");
//            BoxManager.Instance.ShowLibaoShareMessage(string.Format(LanguageManger.GetMe().GetWords("L_S009")));
//            UIEventListener.Get(BoxManager.Instance.buttonCancle).onClick += OnReTryPowerShare;
//        }

//    }

    //void ShareResultHandlerDirectly(ResponseState state, PlatformType type, Hashtable shareInfo, Hashtable error, bool end)
    //{
//        if (state == ResponseState.Success)
//        {
//            print("share result :");
//            print(MiniJSON.jsonEncode(shareInfo));
//            BoxManager.Instance.ShowPopupMessage(string.Format(LanguageManger.GetMe().GetWords("L_S008")));
//            LocalDataBase.Instance().AddDataNum(DataType.power, 3);
//            LocalDataBase.Instance().SetDataNum(DataType.zhuanshi, 200);
//            Hashtable hashTable = TableManager.GetEquip();
//            foreach (DictionaryEntry dic in hashTable)
//            {
//                Tab_Equip tabEquip = (Tab_Equip)dic.Value;
//                if (tabEquip.EnumID == (int)EquipEnumID.Hammer)
//                {
//                    LocalDataBase.SetEquipNum((EquipEnumID)tabEquip.EnumID, 2);
//                }
//                if (tabEquip.EnumID == (int)EquipEnumID.ResetItem)
//                {
//                    LocalDataBase.SetEquipNum((EquipEnumID)tabEquip.EnumID, 1);
//                }
//                if (tabEquip.EnumID == (int)EquipEnumID.Exchange)
//                {
//                    LocalDataBase.SetEquipNum((EquipEnumID)tabEquip.EnumID, 2);
//                }
//                if (tabEquip.EnumID == (int)EquipEnumID.BomEffect)
//                {
//                    LocalDataBase.SetEquipNum((EquipEnumID)tabEquip.EnumID, 1);
//                }
//                if (tabEquip.EnumID == (int)EquipEnumID.RowColEliminate)
//                {
//                    LocalDataBase.SetEquipNum((EquipEnumID)tabEquip.EnumID, 1);
//                }
//            }
//
//            return;
//        }
//        else if (state == ResponseState.Fail)
//        {
//            print("fail! error code = " + error["error_code"] + "; error msg = " + error["error_msg"]);
//            BoxManager.Instance.ShowLibaoShareMessage(string.Format(LanguageManger.GetMe().GetWords("L_S009")));
//            UIEventListener.Get(BoxManager.Instance.buttonCancle).onClick += OnReTryGiftShare;
//        }
//        else if (state == ResponseState.Cancel)
//        {
//            print("cancel !");
//            BoxManager.Instance.ShowLibaoShareMessage(string.Format(LanguageManger.GetMe().GetWords("L_S009")));
//            UIEventListener.Get(BoxManager.Instance.buttonCancle).onClick += OnReTryGiftShare;
//        }


    //}
}
