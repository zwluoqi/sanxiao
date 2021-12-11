using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GCGame.Table;


public class MissionManager 
{
    private static MissionManager instance;

    public static MissionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MissionManager();
            }
            return instance;
        }
    }
    
    private List<Mission> missionItems = new List<Mission>();
	
	public int completedScore = 0;
	public int limitAmount = 0;

	//Init all score for player by missions from levels
	public void InitScore(List<Mission> missions,int levelLimit)
	{
		//Init all score to 0
		missionItems.Clear();
		for(int i=0; i<missions.Count;i++)
		{
			Mission item = new Mission(missions[i].type,0);
            missionItems.Add(item);
		}
		
		//limitAmount = levelLimit;
		limitAmount = levelLimit;
		if(LocalDataBase.Instance().GetEquipTmpNum(EquipTypeTmp.AddThree)> 0){
			limitAmount +=  3;
		}

		completedScore = 0;
	}
	//Insert score by mission type
	//Ex: if user get one Ring, we will add 1 with type is MissionType.Ring
	public void AddScore(int amount)
	{
		completedScore += amount;
        EleUIController.Instance.UpdateScoreUI();
	}

    public bool HasMission(int missionID)
    {
        Mission mission = missionItems.Find(obj => obj.type == missionID);
        if (mission != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool AddFri(int missionType,int num)
    {
        Mission score = missionItems.Find(obj => obj.type == missionType);
        if (score != null)
        {
            score.amount += 1;
            if (!Map.Instance.removedMissionIDList.ContainsKey(missionType))
            {
                Map.Instance.removedMissionIDList.Add(missionType,1);
            }
            else
            {
                Map.Instance.removedMissionIDList[missionType]++;
            }
            return true;
        }
        return false;
    }

    public Mission GetMissionByID(int missionType)
	{

        Mission score = missionItems.Find(obj => obj.type == missionType);
        if (score != null)
        {
            return score;
        }
        return null;
     
       
	}


    public int CompletedMissionCount(Dictionary<int, int> removedMissionIDList)
    {
        int eliMissionCount = 0;
        foreach (Mission mission in MissionManager.Instance.missionItems)
        {
            if (removedMissionIDList.ContainsKey(mission.type))
            {
                eliMissionCount += removedMissionIDList[mission.type];
            }
        }

        return eliMissionCount;
    }

	//Check level complete or failed
	public bool IsWin()
	{

        if (LevelData.mode == CopyMode.TAFANG)
        {
            return false;
        }

		//If not all mission finished, return false
		foreach(Mission mission in LevelData.requestMissions)
		{
			if(GetMissionByID(mission.type).amount < mission.amount)
			{
				return false;
			}
		}
		
		if(completedScore < LevelData.requestScore){
			return false;
		}
		
		//check all missions
		return true;
	}
	
	public bool IsGetEnoughMission(){
		foreach(Mission mission in LevelData.requestMissions)
		{
			if(GetMissionByID(mission.type).amount < mission.amount)
			{
				return false;
			}
		}

		//check all missions
		return true;

	}

	public int GetResultStart(){
		int star = 0;
        if (completedScore > LevelData.currenCopyDetail.GetStarbyIndex(2))
        {
			star = 3;
        }
        else if (completedScore > LevelData.currenCopyDetail.GetStarbyIndex(1))
        {
			star = 2;
        }
        else if (completedScore > LevelData.currenCopyDetail.GetStarbyIndex(0))
        {
			star = 1;
		}
		else if(IsGetEnoughMission()){
			star = 1;
		}
		else{
			star = 0;
		}
		return star;
	}
	
	
	public int GetResultGoldNum(){
		int goldNum;
        goldNum = (int)(LevelData.currenCopyDetail.MaxGoldBase * 0.333f * GetResultStart());
		return goldNum;
	}
    
}
