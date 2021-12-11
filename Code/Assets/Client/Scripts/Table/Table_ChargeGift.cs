//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_ChargeGift : ITableOperate
{ private const string TAB_FILE_DATA = "charge_gift.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_DETIAL,
ID_CHARGEMONEY,
ID_GIFTNAME,
ID_GETNUM1,
ID_EQUIPID1,
ID_GETNUM2,
ID_EQUIPID2,
ID_GETNUM3,
ID_EQUIPID3,
ID_GETZUANSHINUM,
ID_GETPOWERNUM,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_GetPowerNUM;
 public int GetPowerNUM { get{ return m_GetPowerNUM;}}

private int m_GetzuanshiNUM;
 public int GetzuanshiNUM { get{ return m_GetzuanshiNUM;}}

private int m_ChargeMoney;
 public int ChargeMoney { get{ return m_ChargeMoney;}}

private string m_Detial;
 public string Detial { get{ return m_Detial;}}

private int[] m_Equipid = new int[3];
 public int GetEquipidbyIndex(int idx) {
 if(idx>=0 && idx<3) return m_Equipid[idx];
 return -1;
 }

private int[] m_GetNum = new int[3];
 public int GetGetNumbyIndex(int idx) {
 if(idx>=0 && idx<3) return m_GetNum[idx];
 return -1;
 }

private string m_GiftName;
 public string GiftName { get{ return m_GiftName;}}

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
 Tab_ChargeGift _values = new Tab_ChargeGift();
 _values.m_GetPowerNUM =  Convert.ToInt32(valuesList[(int)_ID.ID_GETPOWERNUM] as string);
_values.m_GetzuanshiNUM =  Convert.ToInt32(valuesList[(int)_ID.ID_GETZUANSHINUM] as string);
_values.m_ChargeMoney =  Convert.ToInt32(valuesList[(int)_ID.ID_CHARGEMONEY] as string);
_values.m_Detial =  valuesList[(int)_ID.ID_DETIAL] as string;
_values.m_Equipid [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_EQUIPID1] as string);
_values.m_Equipid [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_EQUIPID2] as string);
_values.m_Equipid [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_EQUIPID3] as string);
_values.m_GetNum [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_GETNUM1] as string);
_values.m_GetNum [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_GETNUM2] as string);
_values.m_GetNum [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_GETNUM3] as string);
_values.m_GiftName =  valuesList[(int)_ID.ID_GIFTNAME] as string;

 _hash[nKey] = _values; }


}
}

