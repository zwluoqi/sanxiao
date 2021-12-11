using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GCGame.Table;


public class TFData
{
    public int tfyxID;
    public int tfendID;
    public List<int> tfGeZiIDs;
    public List<int> tfDiRenIDs;
}


public class LevelData 
{	
	public static int[] mapData = new int[Map.maxCol*Map.maxRow];
    public static int[] guildEleData = new int[Map.maxCol*Map.maxRow];
	
	//List of mission in this map
	public static List<Mission> requestMissions = new List<Mission>();
	public static int requestScore = 0;
	public static int maxScore = 0;
    public static CopyType type = CopyType.MoveLimit;
    public static CopyMode mode = CopyMode.ElE;
    public static TFData tfData = null; 
	public static int limitAmount = 40;
    public static bool lastRowShow = false;

    public static bool lastAnimationShow = false;


    public static Dictionary<string, TextAsset> copyTextasset = new Dictionary<string, TextAsset>();
    public static Dictionary<string, TextAsset> guildTextasset = new Dictionary<string, TextAsset>();

    public static int currentLevel = 0;
    public static Tab_Copydetail currenCopyDetail;
    public static int guildLevel = 0;
    public static bool needGuild = false;
    public static int guildWaitInputCount = 0;
    public static int guildTFCount = 0;
    public static int guildMissionCount = 0;



    public static List<Tab_Copystory> font_storys;
    public static List<Tab_Copystory> back_storys;

    public static bool needWaitInputGuild{
        get { 
            bool ret = needGuild && (guildWaitInputCount < currenCopyDetail.GuildOperationCount || currenCopyDetail.GuildOperationCount == 0);
            return ret; }
    }

    public static bool needTFGuild
    {
        get { 
            bool ret = needGuild && guildTFCount < 2; 
            if (ret)guildTFCount++; 
            return ret; }
    }

    public static bool lastMissionGuild
    {
        get { 
            bool ret = needGuild && guildMissionCount < 1 && guildWaitInputCount >= currenCopyDetail.GuildOperationCount; 
            if (ret) guildMissionCount++; 
            return ret; }
    }


    public static bool WinGameToUI = false;
    public static bool AutoOpenNextLevel = false;

	public static void LoadDataFromLocal(int level)
	{
		
		//加载新人物前应该将以前任务清除干净;
        tfData = null;
		requestMissions.Clear();
		requestScore = 0;
		maxScore = 0;
        currentLevel = level;
        WinGameToUI = false;
        AutoOpenNextLevel = false;
        lastAnimationShow = false;
		ProcessGameDataFromTable(currentLevel);
		
	}
	public static void LoadDataFromURL(int currentLevel)
	{
		//Read data from your server, if you want
	}
	
	//从策划表中加载数据;
	static void ProcessGameDataFromTable(int currentLevel){
		currenCopyDetail = TableManager.GetCopydetailByID(currentLevel);

        guildLevel = currenCopyDetail.GuildLevel;
        lastRowShow = currenCopyDetail.LastRowShow == 1;
        type = (CopyType)currenCopyDetail.CopyType;
        mode = (CopyMode)currenCopyDetail.CopyMode;
        if(mode == CopyMode.TAFANG){
            InitTFDATA(currenCopyDetail);
        }
		limitAmount = currenCopyDetail.MoveLimitedNum;

		for(int i=0;i<4;i++){
			int mission_id = currenCopyDetail.GetRequestMissionIDbyIndex(i);
			if(mission_id != -1){
				requestMissions.Add( new Mission(mission_id, currenCopyDetail.GetRequestMissionNumbyIndex(i)));
			}
		}
		requestScore = currenCopyDetail.ScoreRequest;
		maxScore = currenCopyDetail.StarMax;//最高分;
        TextAsset mapText;
        if (LevelData.copyTextasset.ContainsKey(currenCopyDetail.MapName))
        {
            mapText = LevelData.copyTextasset[currenCopyDetail.MapName];
        }
        else
        {
            mapText = Resources.Load("Maps/" + currenCopyDetail.MapName) as TextAsset;
            LevelData.copyTextasset.Add(currenCopyDetail.MapName, mapText);
        }
        if (mapText)
        {
			ProcessGameDataFromString(mapText.text);
        } 
        else
        {
           Debug.LogError("level map error");
        }

        #region guild
        //引导数据
        if (PlayerPrefs.GetInt("guildCopy" + currentLevel, -1) != 1
            && currenCopyDetail.GuildEleData != "None")
        {
            PlayerPrefs.SetInt("guildCopy" + currentLevel, 1);
            TextAsset guildData;
            if (LevelData.guildTextasset.ContainsKey(currenCopyDetail.GuildEleData))
            {
                guildData = LevelData.guildTextasset[currenCopyDetail.GuildEleData];
            }
            else
            {
                guildData = Resources.Load("Maps/" + currenCopyDetail.GuildEleData) as TextAsset;
                LevelData.guildTextasset.Add(currenCopyDetail.GuildEleData, guildData);
            }
            if (guildData)
            {
                ProcessGameGuildDataFromString(guildData.text);
                needGuild = true;
            }
            else
            {
                Debug.LogError("level guildData error");
                needGuild = false;
            }
        }
        else
        {
            needGuild = false;
        }
        guildWaitInputCount = 0;
        guildMissionCount = 0;
        guildTFCount = 0;
        #endregion


        //任务提示
        font_storys = new List<Tab_Copystory>();
        back_storys = new List<Tab_Copystory>();
        if (currenCopyDetail.FontText != -1)
        {
            Tab_Copystory story = TableManager.GetCopystoryByID(currenCopyDetail.FontText);
            if (story != null)
            {
                font_storys.Add(story);
                while (story.NextStoryID != -1)
                {
                    story = TableManager.GetCopystoryByID(story.NextStoryID);
                    font_storys.Add(story);
                }
            }
        }
        if (currenCopyDetail.BackText != -1)
        {
            Tab_Copystory story = TableManager.GetCopystoryByID(currenCopyDetail.BackText);
            if (story != null)
            {
                back_storys.Add(story);
                while (story.NextStoryID != -1)
                {
                    story = TableManager.GetCopystoryByID(story.NextStoryID);
                    back_storys.Add(story);
                }
            }
        }
	}


    static void InitTFDATA(Tab_Copydetail copy)
    {
        Tab_TourContent tab_tf = TableManager.GetTourContentByID(copy.TFID);
        if (tab_tf != null)
        {
            tfData = new TFData();
            tfData.tfyxID = tab_tf.TFYinXiongSquareId;
            tfData.tfendID = tab_tf.TFEndPos;
            tfData.tfGeZiIDs = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                int geziid = tab_tf.GetTFGeZiSquareIdbyIndex(i);
                if (geziid > 0)
                {
                    tfData.tfGeZiIDs.Add(geziid);
                }
                else
                {
                    break;
                }
            }
            tfData.tfDiRenIDs = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                int direnid = tab_tf.GetTFDiRenSquareIdbyIndex(i);
                if (direnid > 0)
                {
                    tfData.tfDiRenIDs.Add(direnid);
                }
                else
                {
                    break;
                }
            }

        }
    }

	//加载地图;
	static void ProcessGameDataFromString(string mapText)
	{

		string[] lines = mapText.Split(new string[]{"\n"},StringSplitOptions.RemoveEmptyEntries);
		
		int mapLine = 0;
		foreach(string line in lines)
		{
			//Split lines again to get map numbers
			string[] squareTypes = line.Split(new string[]{" "},StringSplitOptions.RemoveEmptyEntries);
			for(int i=0; i < squareTypes.Length; i++)
			{
				mapData[mapLine * Map.maxCol + i] = int.Parse(squareTypes[i].Trim());
			}
			mapLine++;

		}
	}

    static void ProcessGameGuildDataFromString(string guildText)
    {
        string[] lines = guildText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        int mapLine = lines.Length - 1;
        foreach (string line in lines)
        {
            //Split lines again to get map numbers
            string[] eleTypes = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < eleTypes.Length; i++)
            {
                guildEleData[mapLine * Map.maxCol + i] = int.Parse(eleTypes[i].Trim());
            }
            mapLine--;
        }
    }

    public static int GetGuildEleId(int row, int col)
    {
        return guildEleData[row * Map.maxCol + col];
    }

	public static Mission GetMissionByID(int id){
		return requestMissions.Find((obj) => obj.type == id);
	}
	
    public static Mission GetMissionByIndex(int ndex)
    {
        //If not all mission finished, return false
        if (ndex >= LevelData.requestMissions.Count) return null;
        Mission temp = LevelData.requestMissions[ndex];
        return temp;
    }
    public static int GetMissionCount()
    {
        return LevelData.requestMissions.Count;
    }
}
