//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_TableModel : ITableOperate
{ private const string TAB_FILE_DATA = "TableModel.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_EFFECTID,
ID_CD_ROUND,
ID_LOGIC_ID,
ID_ORDER,
ID_PERCENT,
ID_DATA,
ID_EXAMPLE,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_CdRound;
 public int CdRound { get{ return m_CdRound;}}

private int m_Data;
 public int Data { get{ return m_Data;}}

private int m_EffectID;
 public int EffectID { get{ return m_EffectID;}}

private string m_Example;
 public string Example { get{ return m_Example;}}

private int m_LogicId;
 public int LogicId { get{ return m_LogicId;}}

private int m_Order;
 public int Order { get{ return m_Order;}}

private int m_Percent;
 public int Percent { get{ return m_Percent;}}

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
 Tab_TableModel _values = new Tab_TableModel();
 _values.m_CdRound =  Convert.ToInt32(valuesList[(int)_ID.ID_CD_ROUND] as string);
_values.m_Data =  Convert.ToInt32(valuesList[(int)_ID.ID_DATA] as string);
_values.m_EffectID =  Convert.ToInt32(valuesList[(int)_ID.ID_EFFECTID] as string);
_values.m_Example =  valuesList[(int)_ID.ID_EXAMPLE] as string;
_values.m_LogicId =  Convert.ToInt32(valuesList[(int)_ID.ID_LOGIC_ID] as string);
_values.m_Order =  Convert.ToInt32(valuesList[(int)_ID.ID_ORDER] as string);
_values.m_Percent =  Convert.ToInt32(valuesList[(int)_ID.ID_PERCENT] as string);

 _hash[nKey] = _values; }


}
}

