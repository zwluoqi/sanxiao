//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_TAchievementSpecialChapter : ITableOperate
{ private const string TAB_FILE_DATA = "t_achievement_special_chapter.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_USERGUID,
ID_COPYID,
ID_NUM,
ID_SUCCESSNUM,
ID_PLAYNUM,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_Copyid;
 public int Copyid { get{ return m_Copyid;}}

private int m_Num;
 public int Num { get{ return m_Num;}}

private int m_Playnum;
 public int Playnum { get{ return m_Playnum;}}

private int m_Successnum;
 public int Successnum { get{ return m_Successnum;}}

private int m_UserGuid;
 public int UserGuid { get{ return m_UserGuid;}}

public bool LoadTable(Hashtable _tab)
 {
 if(!TableManager.ReaderPList(GetInstanceFile(),SerializableTable,_tab))
 {
 throw TableException.ErrorReader("Load File{0} Fail!!!",GetInstanceFile());
 }
 return true;
 }
 public void SerializableTable(ArrayList valuesList,string skey,Hashtable _hash)
 {
 if (string.IsNullOrEmpty(skey))
 {
 throw TableException.ErrorReader("Read File{0} as key is Empty Fail!!!", GetInstanceFile());
 }

 if ((int)_ID.MAX_RECORD!=valuesList.Count)
 {
 throw TableException.ErrorReader("Load {0} error as CodeSize:{1} not Equal DataSize:{2}", GetInstanceFile(),_ID.MAX_RECORD,valuesList.Count);
 }
 Int32 nKey = Convert.ToInt32(skey);
 Tab_TAchievementSpecialChapter _values = new Tab_TAchievementSpecialChapter();
 _values.m_Copyid =  Convert.ToInt32(valuesList[(int)_ID.ID_COPYID] as string);
_values.m_Num =  Convert.ToInt32(valuesList[(int)_ID.ID_NUM] as string);
_values.m_Playnum =  Convert.ToInt32(valuesList[(int)_ID.ID_PLAYNUM] as string);
_values.m_Successnum =  Convert.ToInt32(valuesList[(int)_ID.ID_SUCCESSNUM] as string);
_values.m_UserGuid =  Convert.ToInt32(valuesList[(int)_ID.ID_USERGUID] as string);

 _hash[nKey] = _values; }


}
}

