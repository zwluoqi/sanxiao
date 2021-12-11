//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_TUser : ITableOperate
{ private const string TAB_FILE_DATA = "t_user.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_USERGUID,
ID_ACCOUNT,
ID_USERLEVEL,
ID_GOLD,
ID_MONEY,
ID_LIVE,
ID_WORLDSORT,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_Account;
 public int Account { get{ return m_Account;}}

private int m_Gold;
 public int Gold { get{ return m_Gold;}}

private int m_Live;
 public int Live { get{ return m_Live;}}

private int m_Money;
 public int Money { get{ return m_Money;}}

private int m_UserGuid;
 public int UserGuid { get{ return m_UserGuid;}}

private int m_Userlevel;
 public int Userlevel { get{ return m_Userlevel;}}

private int m_Worldsort;
 public int Worldsort { get{ return m_Worldsort;}}

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
 Tab_TUser _values = new Tab_TUser();
 _values.m_Account =  Convert.ToInt32(valuesList[(int)_ID.ID_ACCOUNT] as string);
_values.m_Gold =  Convert.ToInt32(valuesList[(int)_ID.ID_GOLD] as string);
_values.m_Live =  Convert.ToInt32(valuesList[(int)_ID.ID_LIVE] as string);
_values.m_Money =  Convert.ToInt32(valuesList[(int)_ID.ID_MONEY] as string);
_values.m_UserGuid =  Convert.ToInt32(valuesList[(int)_ID.ID_USERGUID] as string);
_values.m_Userlevel =  Convert.ToInt32(valuesList[(int)_ID.ID_USERLEVEL] as string);
_values.m_Worldsort =  Convert.ToInt32(valuesList[(int)_ID.ID_WORLDSORT] as string);

 _hash[nKey] = _values; }


}
}

