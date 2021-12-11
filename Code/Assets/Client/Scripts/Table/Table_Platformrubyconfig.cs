//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Platformrubyconfig : ITableOperate
{ private const string TAB_FILE_DATA = "platformrubyconfig.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_RID,
ID_IDENTIFICATION,
ID_PLATFORMID,
ID_PRICE,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_RID;
 public int RID { get{ return m_RID;}}

private string m_Identification;
 public string Identification { get{ return m_Identification;}}

private int m_PlatformId;
 public int PlatformId { get{ return m_PlatformId;}}

private int m_Price;
 public int Price { get{ return m_Price;}}

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
 Tab_Platformrubyconfig _values = new Tab_Platformrubyconfig();
 _values.m_RID =  Convert.ToInt32(valuesList[(int)_ID.ID_RID] as string);
_values.m_Identification =  valuesList[(int)_ID.ID_IDENTIFICATION] as string;
_values.m_PlatformId =  Convert.ToInt32(valuesList[(int)_ID.ID_PLATFORMID] as string);
_values.m_Price =  Convert.ToInt32(valuesList[(int)_ID.ID_PRICE] as string);

 _hash[nKey] = _values; }


}
}

