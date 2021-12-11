//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_TItem : ITableOperate
{ private const string TAB_FILE_DATA = "t_item.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_USERGUID,
ID_ITEMID,
ID_ITEMCOUNT,
ID_ISVALID,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_Isvalid;
 public int Isvalid { get{ return m_Isvalid;}}

private int m_Itemcount;
 public int Itemcount { get{ return m_Itemcount;}}

private int m_Itemid;
 public int Itemid { get{ return m_Itemid;}}

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
 Tab_TItem _values = new Tab_TItem();
 _values.m_Isvalid =  Convert.ToInt32(valuesList[(int)_ID.ID_ISVALID] as string);
_values.m_Itemcount =  Convert.ToInt32(valuesList[(int)_ID.ID_ITEMCOUNT] as string);
_values.m_Itemid =  Convert.ToInt32(valuesList[(int)_ID.ID_ITEMID] as string);
_values.m_UserGuid =  Convert.ToInt32(valuesList[(int)_ID.ID_USERGUID] as string);

 _hash[nKey] = _values; }


}
}

