//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Dictclass : ITableOperate
{ private const string TAB_FILE_DATA = "dictclass.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_STATICNAME,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private string m_StaticName;
 public string StaticName { get{ return m_StaticName;}}

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
 Tab_Dictclass _values = new Tab_Dictclass();
 _values.m_StaticName =  valuesList[(int)_ID.ID_STATICNAME] as string;

 _hash[nKey] = _values; }


}
}

