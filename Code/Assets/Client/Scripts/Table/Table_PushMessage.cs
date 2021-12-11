//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_PushMessage : ITableOperate
{ private const string TAB_FILE_DATA = "pushMessage.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_SPMID,
ID_MSGTYPE,
ID_MSGNAME,
ID_MSGTEXT,
ID_DAYS,
ID_TIME,
ID_STATICNAME,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private string m_Days;
 public string Days { get{ return m_Days;}}

private string m_MsgName;
 public string MsgName { get{ return m_MsgName;}}

private string m_MsgText;
 public string MsgText { get{ return m_MsgText;}}

private int m_MsgType;
 public int MsgType { get{ return m_MsgType;}}

private int m_SPMId;
 public int SPMId { get{ return m_SPMId;}}

private string m_StaticName;
 public string StaticName { get{ return m_StaticName;}}

private string m_Time;
 public string Time { get{ return m_Time;}}

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
 Tab_PushMessage _values = new Tab_PushMessage();
 _values.m_Days =  valuesList[(int)_ID.ID_DAYS] as string;
_values.m_MsgName =  valuesList[(int)_ID.ID_MSGNAME] as string;
_values.m_MsgText =  valuesList[(int)_ID.ID_MSGTEXT] as string;
_values.m_MsgType =  Convert.ToInt32(valuesList[(int)_ID.ID_MSGTYPE] as string);
_values.m_SPMId =  Convert.ToInt32(valuesList[(int)_ID.ID_SPMID] as string);
_values.m_StaticName =  valuesList[(int)_ID.ID_STATICNAME] as string;
_values.m_Time =  valuesList[(int)_ID.ID_TIME] as string;

 _hash[nKey] = _values; }


}
}

