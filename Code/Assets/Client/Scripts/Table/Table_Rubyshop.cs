//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Rubyshop : ITableOperate
{ private const string TAB_FILE_DATA = "rubyshop.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_DETIAL,
ID_SPRITENAME,
ID_COSTRMB,
ID_GETNUM,
ID_BASENUM,
ID_SONGNUM,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_BaseNum;
 public int BaseNum { get{ return m_BaseNum;}}

private int m_CostRMB;
 public int CostRMB { get{ return m_CostRMB;}}

private string m_Detial;
 public string Detial { get{ return m_Detial;}}

private int m_GetNum;
 public int GetNum { get{ return m_GetNum;}}

private int m_SongNum;
 public int SongNum { get{ return m_SongNum;}}

private string m_SpriteName;
 public string SpriteName { get{ return m_SpriteName;}}

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
 Tab_Rubyshop _values = new Tab_Rubyshop();
 _values.m_BaseNum =  Convert.ToInt32(valuesList[(int)_ID.ID_BASENUM] as string);
_values.m_CostRMB =  Convert.ToInt32(valuesList[(int)_ID.ID_COSTRMB] as string);
_values.m_Detial =  valuesList[(int)_ID.ID_DETIAL] as string;
_values.m_GetNum =  Convert.ToInt32(valuesList[(int)_ID.ID_GETNUM] as string);
_values.m_SongNum =  Convert.ToInt32(valuesList[(int)_ID.ID_SONGNUM] as string);
_values.m_SpriteName =  valuesList[(int)_ID.ID_SPRITENAME] as string;

 _hash[nKey] = _values; }


}
}

