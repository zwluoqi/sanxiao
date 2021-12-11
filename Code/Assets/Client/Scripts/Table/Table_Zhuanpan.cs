//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Zhuanpan : ITableOperate
{ private const string TAB_FILE_DATA = "zhuanpan.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_CLASSID,
ID_PROPID,
ID_OBJID,
ID_NUM,
ID_RATE,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_Classid;
 public int Classid { get{ return m_Classid;}}

private int m_Num;
 public int Num { get{ return m_Num;}}

private int m_Objid;
 public int Objid { get{ return m_Objid;}}

private int m_Propid;
 public int Propid { get{ return m_Propid;}}

private int m_Rate;
 public int Rate { get{ return m_Rate;}}

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
 Tab_Zhuanpan _values = new Tab_Zhuanpan();
 _values.m_Classid =  Convert.ToInt32(valuesList[(int)_ID.ID_CLASSID] as string);
_values.m_Num =  Convert.ToInt32(valuesList[(int)_ID.ID_NUM] as string);
_values.m_Objid =  Convert.ToInt32(valuesList[(int)_ID.ID_OBJID] as string);
_values.m_Propid =  Convert.ToInt32(valuesList[(int)_ID.ID_PROPID] as string);
_values.m_Rate =  Convert.ToInt32(valuesList[(int)_ID.ID_RATE] as string);

 _hash[nKey] = _values; }


}
}

