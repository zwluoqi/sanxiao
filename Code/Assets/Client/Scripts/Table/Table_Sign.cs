//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Sign : ITableOperate
{ private const string TAB_FILE_DATA = "sign.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_YEAR,
ID_WEEKOFYEAR,
ID_DAYOFWEEK,
ID_CLASSID,
ID_PROPID,
ID_OBJID,
ID_NUM,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_DayOfWeek;
 public int DayOfWeek { get{ return m_DayOfWeek;}}

private int m_WeekOfYear;
 public int WeekOfYear { get{ return m_WeekOfYear;}}

private int m_Year;
 public int Year { get{ return m_Year;}}

private int m_Classid;
 public int Classid { get{ return m_Classid;}}

private int m_Num;
 public int Num { get{ return m_Num;}}

private int m_Objid;
 public int Objid { get{ return m_Objid;}}

private int m_Propid;
 public int Propid { get{ return m_Propid;}}

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
 Tab_Sign _values = new Tab_Sign();
 _values.m_DayOfWeek =  Convert.ToInt32(valuesList[(int)_ID.ID_DAYOFWEEK] as string);
_values.m_WeekOfYear =  Convert.ToInt32(valuesList[(int)_ID.ID_WEEKOFYEAR] as string);
_values.m_Year =  Convert.ToInt32(valuesList[(int)_ID.ID_YEAR] as string);
_values.m_Classid =  Convert.ToInt32(valuesList[(int)_ID.ID_CLASSID] as string);
_values.m_Num =  Convert.ToInt32(valuesList[(int)_ID.ID_NUM] as string);
_values.m_Objid =  Convert.ToInt32(valuesList[(int)_ID.ID_OBJID] as string);
_values.m_Propid =  Convert.ToInt32(valuesList[(int)_ID.ID_PROPID] as string);

 _hash[nKey] = _values; }


}
}

