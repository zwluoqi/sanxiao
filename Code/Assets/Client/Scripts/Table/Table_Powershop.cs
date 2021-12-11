//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Powershop : ITableOperate
{ private const string TAB_FILE_DATA = "powershop.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_DETIAL,
ID_SPRITENAME,
ID_COSTRUBY,
ID_GETNUM,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_CostRuby;
 public int CostRuby { get{ return m_CostRuby;}}

private string m_Detial;
 public string Detial { get{ return m_Detial;}}

private int m_GetNum;
 public int GetNum { get{ return m_GetNum;}}

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
 Tab_Powershop _values = new Tab_Powershop();
 _values.m_CostRuby =  Convert.ToInt32(valuesList[(int)_ID.ID_COSTRUBY] as string);
_values.m_Detial =  valuesList[(int)_ID.ID_DETIAL] as string;
_values.m_GetNum =  Convert.ToInt32(valuesList[(int)_ID.ID_GETNUM] as string);
_values.m_SpriteName =  valuesList[(int)_ID.ID_SPRITENAME] as string;

 _hash[nKey] = _values; }


}
}

