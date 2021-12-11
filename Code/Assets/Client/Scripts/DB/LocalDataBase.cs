using UnityEngine;
using System.Collections;
using System;
using GCGame.Table;
using System.Collections.Generic;
//using xjgame.message;

public enum DataType {
	zhuanshi,
	jinbi,
	power,
};

public enum SystemLock{
	Music,
	MusicP,
	Power,
};

public class LocalDataBase
{
    private static LocalDataBase m_instance = null;
    public static LocalDataBase Instance()
    {
        if (null == m_instance)
        {
            m_instance = new LocalDataBase();
        }
        return m_instance;
    }
	
	private LocalDataBase(){
		//InitFitstGameData();
		ClearTmpData();
	}


    public static int maxPower = 15;
    public static int coolDownSecond = 8 * 60;


		public bool HadGetChargeGift(){
				return PlayerPrefs.GetInt("HadGetChargeGift",-1) == 1;
		}

		public void SetHadGetChargeGift(){
				PlayerPrefs.SetInt("HadGetChargeGift",1);
		}


		public int GetChargeNum(){
				return PlayerPrefs.GetInt("GetChargeNum", 0);

		}

		public void SetChargeNum(int num){
				PlayerPrefs.SetInt("GetChargeNum", num);
		}

    public int GetFreeChouTimes()
    {
        return PlayerPrefs.GetInt("ChouFree", 0);
    }

    public void SetFreeChouTimes(int times)
    {
        PlayerPrefs.SetInt("ChouFree", times);
    }

	//道具数据;
	public int GetEquipNum(EquipEnumID type){
		return PlayerPrefs.GetInt("EquipType"+type.ToString(),0);
	}
	public static void SetEquipNum(EquipEnumID type,int num){
		PlayerPrefs.SetInt("EquipType"+type.ToString(),num);
	}


    //以下接口用来增加和减少道具试用
    public bool AddEquipNum(EquipEnumID type,int num)
    {  
       int ntem= GetEquipNum(type) + num;
       if (ntem <= 32767)
        {
            SetEquipNum(type,ntem);
            return true;

        }
        else
        {
            SetEquipNum(type,32767);
            return false;
        }
       
        //return true;
    }
    public bool DecreaseEquipNum(EquipEnumID type,int num)
    {

        //Umeng.GA.Use(type.ToString(), num, 0);
		int ntem = GetEquipTmpNum(type);
		if (ntem - num < 0)
		{
			SetEquipTmpNum(type,0);
			int ntemtmp = GetEquipNum(type);
			if (ntemtmp + (ntem - num) < 0)
			{
				SetEquipNum(type,0);
				return false;
			} 
			else
			{
				ntemtmp += (ntem - num);
				SetEquipNum(type,ntemtmp);
				return true;
			}
		} 
		else
		{
			ntem -= num;
			SetEquipTmpNum(type,ntem);
			return true;
		}


       // return true;
    }

	//临时道具数据;
	public int GetEquipTmpNum(EquipEnumID type){
		return PlayerPrefs.GetInt("EquipTypeTmp"+type.ToString(),0);
	}
	private void SetEquipTmpNum(EquipEnumID type,int num){
		PlayerPrefs.SetInt("EquipTypeTmp"+type.ToString(),num);
	}

	//以下接口用来增加和减少道具试用
	public bool AddEquipTmpNum(EquipEnumID type,int num)
	{  
		int ntem= GetEquipTmpNum(type) + num;
		if (ntem <= 32767)
		{
			SetEquipTmpNum(type,ntem);
			return true;
			
		}
		else
		{
			SetEquipTmpNum(type,32767);
			return false;
		}
		
		//return true;
	}

	//不能购买的临时道具;
	public int GetEquipTmpNum(EquipTypeTmp type){
		return PlayerPrefs.GetInt("EquipTypeTmpNot"+type.ToString(),0);
	}
	public void SetEquipTmpNum(EquipTypeTmp type,int num){
		PlayerPrefs.SetInt("EquipTypeTmpNot"+type.ToString(),num);
	}

	public void ClearTmpData(){
		SetEquipTmpNum(EquipTypeTmp.AddThree,0);
		SetEquipTmpNum(EquipTypeTmp.Miracle,0);
		SetEquipTmpNum(EquipTypeTmp.Specital,0);
		Hashtable hashTable = TableManager.GetEquip();
		foreach(DictionaryEntry dic in hashTable){
			Tab_Equip tabEquip = (Tab_Equip)dic.Value;
			SetEquipTmpNum((EquipEnumID)tabEquip.EnumID,0);
		}
	}

	/*
	//关卡数据;
    //设置玩家已经打开锁的等级
     public int GetOpenedCopyLevel() { return PlayerPrefs.GetInt("openedLevel",-1); }
     public void SetOpenedCopyLevel(int level) { PlayerPrefs.SetInt("openedLevel", level); }
	 */
    //玩家当前选中的关卡等级（比如现在可以玩1～60，而现在选中的关卡为40级关卡）
     public int GetSelectCopyLevel() { return PlayerPrefs.GetInt("selectLevel",1); }
     public void SetSelectCopyLevel(int level) { PlayerPrefs.SetInt("selectLevel", level); }
	
	
	public int GetDataNum(DataType type){return PlayerPrefs.GetInt("DataNum"+type.ToString(),0); }
	public void SetDataNum(DataType type, int number){PlayerPrefs.SetInt("DataNum"+type.ToString(),number); }
	
	public void AddDataNum(DataType type,int number){
		int maxVal = int.MaxValue;
		
         int ntem = GetDataNum(type) + number;
         if (ntem <= maxVal)
         {
             SetDataNum(type,ntem);
         }
         else
         {
             SetDataNum(type,maxVal);
         }
	}
	
    public bool DecreaseDataNum(DataType type,int num)
    {
        int ntem = GetDataNum(type);
        if (ntem - num < 0)
        {
            SetDataNum(type,0);
            return false;
        } 
        else
        {
            ntem -= num;
            SetDataNum(type,ntem);
            return true;
        }

       // return true;
    }


	public void SetCurrentTime(DateTime currentTime){
		PlayerPrefs.SetString("systime",currentTime.Ticks.ToString()); 
	}

	public DateTime GetLastDateTime(){
		string time = PlayerPrefs.GetString("systime","");
		if(string.IsNullOrEmpty(time)){
			DateTime dataTime = DateTime.Now;
			PlayerPrefs.SetString("systime",dataTime.Ticks.ToString()); 
			return dataTime;
		}else{
			return new DateTime(long.Parse(time));
		}
	}
	
	public void SetSysState(SystemLock sys,bool open){
		PlayerPrefs.SetInt(sys.ToString(),open?1:0);
	}

	public bool GetSysState(SystemLock sys){
		int ret = PlayerPrefs.GetInt(sys.ToString(),1);
		return ret != 0;
	}


    public static bool LoadResourceCompleted = false;
    public static List<CopyDataModel> copyModels = null;
    public static long pid;
    public static string plarmformID;
    public static List<KeyValuePair<int, string>> idens = new List<KeyValuePair<int,string>>();

//    public static void InitCopyDatas(List<CopyData> copys)
//    {
//        List<CopyDataModel> copyDatas = new List<CopyDataModel>();
//        foreach (CopyData copyData in copys)
//        {
//            CopyDataModel model = new CopyDataModel(copyData.Copyid);
//            model.copyID = copyData.Copyid;
//            model.star = copyData.Star;
//            model.SaveData();
//            copyDatas.Add(model);
//        }
//        copyModels = copyDatas;
//    }

    public static void InitCopyDatas()
    {
        int copyCounts = TableManager.GetCopydetail().Count;
        List<CopyDataModel> copyDatas = new List<CopyDataModel>();
        for (int i = 1; i <= copyCounts; i++)
        {
            CopyDataModel model = new CopyDataModel(i);
            if (model.copyID == 1 && model.star == -1)
            {
                model.star = 0;
            }
            model.SaveData();
            copyDatas.Add(model);
        }
        //第一次进入游戏,设置当前解锁的关卡id为1;
        copyModels = copyDatas;
    }

//    public static void InitEquipData(List<EquipData> equips)
//    {
//        foreach (EquipData equip in equips)
//        {
//            SetEquipNum((EquipEnumID)equip.EquipEnumid, equip.Num);
//        }
//    }
//
//    public List<EquipData> GetLocalEquipData()
//    {
//        List<EquipData> equips = new List<EquipData>();
//        Hashtable hashTable = TableManager.GetEquip();
//        foreach (DictionaryEntry dic in hashTable)
//        {
//            Tab_Equip tabEquip = (Tab_Equip)dic.Value;
//            EquipData equip = new EquipData();
//            equip.EquipEnumid = tabEquip.EnumID;
//            equip.Num = GetEquipNum((EquipEnumID)tabEquip.EnumID);
//            equips.Add(equip);
//        }
//        return equips;
//    }


    public void InitIapIdentifierInfo()
    {
        idens.Clear();
		Debug.Log("PlatFormID:" + SystemConfig.Instance.platformId);
        Hashtable tabs = TableManager.GetPlatformrubyconfig();
        foreach (DictionaryEntry dic in tabs)
        {
            Tab_Platformrubyconfig rubyConfig = (Tab_Platformrubyconfig)dic.Value;
//			if (rubyConfig.PlatformId == (int)SystemConfig.Instance.platformId)
//            {
//            }

			idens.Add(new KeyValuePair<int, string>(rubyConfig.RID, rubyConfig.Identification));
			Debug.Log("rubyID:" + rubyConfig.RID + "Identification:" + rubyConfig.Identification);
        }
    }


    public static bool equipGuild = false;
    public static int equipbuy = 1;
    public static int equipWaitInput = 1;



    //设置某个关卡的得分;如果要设置的分数比当前的还低,则直接返回;
    //在关卡结束后请调用该接口设置某个副本的得分(1-3);
    //并且在关卡结束后请调用该接口开启某个关卡(0)
    public static void SetCopyStar(int copyID, int star)
    {
        CopyDataModel model = LocalDataBase.copyModels[copyID - 1];
        model.star = star;
        model.SaveData();
    }

    public static int GetCopyStar(int copyID)
    {
        CopyDataModel model = LocalDataBase.copyModels[copyID - 1];
        return model.star;
    }

    public const int costPower = 3;

    public static int GetAllStars()
    {
        int sum=0;
        foreach (CopyDataModel dataModel in copyModels)
        {
            if(dataModel.star >= 0){
                sum += dataModel.star;
            }
        }
        return sum;
    }

    public static int GetCurrentMaxLevel(){
        int currentLevel = 1;
        foreach (CopyDataModel dataModel in copyModels)
        {
            if (dataModel.star >= 0)
            {
                currentLevel = dataModel.copyID;
            }
            else
            {
                break;
            }
        }
        return currentLevel;
    }

    public static bool HasShareToday()
    {
        DateTime currentTime = DateTime.Now;
        int s = PlayerPrefs.GetInt("shareTime:" + currentTime.Year + ":" + currentTime.Month + ":" + currentTime.Day, -1);
        return s != -1;
    }

    public static void SetShareToday()
    {
        DateTime currentTime = DateTime.Now;
        PlayerPrefs.SetInt("shareTime:" + currentTime.Year + ":" + currentTime.Month + ":" + currentTime.Day,1);
    }
}
