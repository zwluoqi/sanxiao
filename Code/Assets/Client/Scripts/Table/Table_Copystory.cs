//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Copystory : ITableOperate
{ private const string TAB_FILE_DATA = "copystory.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_LEFTHEROICON,
ID_RIGHTHEROICON,
ID_STORYCONTENT,
ID_NEXTSTORYID,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private string m_LeftHeroIcon;
 public string LeftHeroIcon { get{ return m_LeftHeroIcon;}}

private int m_NextStoryID;
 public int NextStoryID { get{ return m_NextStoryID;}}

private string m_RightHeroIcon;
 public string RightHeroIcon { get{ return m_RightHeroIcon;}}

private string m_StoryContent;
 public string StoryContent { get{ return m_StoryContent;}}

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
 Tab_Copystory _values = new Tab_Copystory();
 _values.m_LeftHeroIcon =  valuesList[(int)_ID.ID_LEFTHEROICON] as string;
_values.m_NextStoryID =  Convert.ToInt32(valuesList[(int)_ID.ID_NEXTSTORYID] as string);
_values.m_RightHeroIcon =  valuesList[(int)_ID.ID_RIGHTHEROICON] as string;
_values.m_StoryContent =  valuesList[(int)_ID.ID_STORYCONTENT] as string;

 _hash[nKey] = _values; }


}
}

