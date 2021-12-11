//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Drop : ITableOperate
{ private const string TAB_FILE_DATA = "drop.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_DROP_TYPE1,
ID_PRO1,
ID_VAL1,
ID_DROP_TYPE2,
ID_PRO2,
ID_VAL2,
ID_DROP_TYPE3,
ID_PRO3,
ID_VAL3,
ID_DROP_TYPE4,
ID_PRO4,
ID_VAL4,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int[] m_DropType = new int[4];
 public int GetDropTypebyIndex(int idx) {
 if(idx>=0 && idx<4) return m_DropType[idx];
 return -1;
 }

private int[] m_Pro = new int[4];
 public int GetProbyIndex(int idx) {
 if(idx>=0 && idx<4) return m_Pro[idx];
 return -1;
 }

private int[] m_Val = new int[4];
 public int GetValbyIndex(int idx) {
 if(idx>=0 && idx<4) return m_Val[idx];
 return -1;
 }

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
 Tab_Drop _values = new Tab_Drop();
 _values.m_DropType [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_DROP_TYPE1] as string);
_values.m_DropType [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_DROP_TYPE2] as string);
_values.m_DropType [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_DROP_TYPE3] as string);
_values.m_DropType [ 3 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_DROP_TYPE4] as string);
_values.m_Pro [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_PRO1] as string);
_values.m_Pro [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_PRO2] as string);
_values.m_Pro [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_PRO3] as string);
_values.m_Pro [ 3 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_PRO4] as string);
_values.m_Val [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_VAL1] as string);
_values.m_Val [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_VAL2] as string);
_values.m_Val [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_VAL3] as string);
_values.m_Val [ 3 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_VAL4] as string);

 _hash[nKey] = _values; }


}
}

