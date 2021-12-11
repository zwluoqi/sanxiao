using UnityEngine;
using System.Collections;
using GCGame.Table;

public class Mission 
{
    public int type = -1;//任务ID;
	public int amount = 0;
    public string missionName;
	public Mission(int id, int num)
	{
		amount = num;
		type = id;
        missionName = TableManager.GetMissionByID(id).Detial;
	}
    public bool IsValid()
    {
        if (type==-1)
        {
            return true;
        }
        if (amount<1)
        {
            return true;
        }
        return false;
    }
}
