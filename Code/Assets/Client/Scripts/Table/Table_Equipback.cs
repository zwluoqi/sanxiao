//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Equipback : ITableOperate
{ private const string TAB_FILE_DATA = "equipback.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_DETIAL,
ID_SPRITENAME,
ID_ENUMID,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private string m_Detial;
 public string Detial { get{ return m_Detial;}}

private int m_EnumID;
 public int EnumID { get{ return m_EnumID;}}

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
 Tab_Equipback _values = new Tab_Equipback();
 _values.m_Detial =  valuesList[(int)_ID.ID_DETIAL] as string;
_values.m_EnumID =  Convert.ToInt32(valuesList[(int)_ID.ID_ENUMID] as string);
_values.m_SpriteName =  valuesList[(int)_ID.ID_SPRITENAME] as string;

 _hash[nKey] = _values; }


}
}

