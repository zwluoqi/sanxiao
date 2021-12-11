using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GCGame.Table;

public class CopyDataModel
{
	public Tab_Copydetail tab_copy;
	public int copyID = 0;//副本ID;
	public int star = -1;//得分等级;(-1——3);
	public bool buyUnLock = false;
	
	public void LoadData(){
		star = PlayerPrefs.GetInt("copy star"+copyID,-1);
		buyUnLock = (PlayerPrefs.GetInt("copy buy unlock"+copyID,-1) == 1);
		//SystemConfig.MyLog(string.Format( "load copy data id:{0},star:{1}",copyID,star));
	}
	
	public void SaveData(){
		//SystemConfig.MyLog(string.Format( "save copy data id:{0},star:{1}",copyID,star));
		PlayerPrefs.SetInt("copy star"+copyID,star);
		PlayerPrefs.SetInt("copy buy unlock"+copyID,buyUnLock?1:-1);
		PlayerPrefs.Save();
	}
	
	
	public CopyDataModel (int id)
	{
		copyID = id;
		tab_copy = TableManager.GetCopydetailByID(id);
		LoadData();
	}
	
}

