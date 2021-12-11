//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Equipshop : ITableOperate
{ private const string TAB_FILE_DATA = "equipshop.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_DETIAL,
ID_SHOPTYPE,
ID_GIFTNAME,
ID_COSTRUBY,
ID_GETNUM1,
ID_EQUIPID1,
ID_GETNUM2,
ID_EQUIPID2,
ID_GETNUM3,
ID_EQUIPID3,
ID_SPRITENAME,
ID_BUYCOUNT,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_BuyCount;
 public int BuyCount { get{ return m_BuyCount;}}

private int m_CostRuby;
 public int CostRuby { get{ return m_CostRuby;}}

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

private int m_ShopType;
 public int ShopType { get{ return m_ShopType;}}

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
 Tab_Equipshop _values = new Tab_Equipshop();
 _values.m_BuyCount =  Convert.ToInt32(valuesList[(int)_ID.ID_BUYCOUNT] as string);
_values.m_CostRuby =  Convert.ToInt32(valuesList[(int)_ID.ID_COSTRUBY] as string);
_values.m_Detial =  valuesList[(int)_ID.ID_DETIAL] as string;
_values.m_Equipid [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_EQUIPID1] as string);
_values.m_Equipid [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_EQUIPID2] as string);
_values.m_Equipid [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_EQUIPID3] as string);
_values.m_GetNum [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_GETNUM1] as string);
_values.m_GetNum [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_GETNUM2] as string);
_values.m_GetNum [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_GETNUM3] as string);
_values.m_GiftName =  valuesList[(int)_ID.ID_GIFTNAME] as string;
_values.m_ShopType =  Convert.ToInt32(valuesList[(int)_ID.ID_SHOPTYPE] as string);
_values.m_SpriteName =  valuesList[(int)_ID.ID_SPRITENAME] as string;

 _hash[nKey] = _values; }


}
}

