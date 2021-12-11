//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_TUserCopy : ITableOperate
{ private const string TAB_FILE_DATA = "t_user_copy.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_PID,
ID_COPYID,
ID_STAR,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_CopyID;
 public int CopyID { get{ return m_CopyID;}}

private int m_PID;
 public int PID { get{ return m_PID;}}

private int m_Star;
 public int Star { get{ return m_Star;}}

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
 Tab_TUserCopy _values = new Tab_TUserCopy();
 _values.m_CopyID =  Convert.ToInt32(valuesList[(int)_ID.ID_COPYID] as string);
_values.m_PID =  Convert.ToInt32(valuesList[(int)_ID.ID_PID] as string);
_values.m_Star =  Convert.ToInt32(valuesList[(int)_ID.ID_STAR] as string);

 _hash[nKey] = _values; }


}
}

