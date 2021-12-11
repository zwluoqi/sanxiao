//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_TourContent : ITableOperate
{ private const string TAB_FILE_DATA = "tourContent.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_TFYINXIONGSQUAREID,
ID_TFENDPOS,
ID_TFGEZISQUAREID1,
ID_TFGEZISQUAREID2,
ID_TFGEZISQUAREID3,
ID_TFGEZISQUAREID4,
ID_TFGEZISQUAREID5,
ID_TFGEZISQUAREID6,
ID_TFGEZISQUAREID7,
ID_TFGEZISQUAREID8,
ID_TFDIRENSQUAREID1,
ID_TFDIRENSQUAREID2,
ID_TFDIRENSQUAREID3,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int[] m_TFDiRenSquareId = new int[3];
 public int GetTFDiRenSquareIdbyIndex(int idx) {
 if(idx>=0 && idx<3) return m_TFDiRenSquareId[idx];
 return -1;
 }

private int m_TFEndPos;
 public int TFEndPos { get{ return m_TFEndPos;}}

private int[] m_TFGeZiSquareId = new int[8];
 public int GetTFGeZiSquareIdbyIndex(int idx) {
 if(idx>=0 && idx<8) return m_TFGeZiSquareId[idx];
 return -1;
 }

private int m_TFYinXiongSquareId;
 public int TFYinXiongSquareId { get{ return m_TFYinXiongSquareId;}}

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
 Tab_TourContent _values = new Tab_TourContent();
 _values.m_TFDiRenSquareId [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_TFDIRENSQUAREID1] as string);
_values.m_TFDiRenSquareId [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_TFDIRENSQUAREID2] as string);
_values.m_TFDiRenSquareId [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_TFDIRENSQUAREID3] as string);
_values.m_TFEndPos =  Convert.ToInt32(valuesList[(int)_ID.ID_TFENDPOS] as string);
_values.m_TFGeZiSquareId [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_TFGEZISQUAREID1] as string);
_values.m_TFGeZiSquareId [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_TFGEZISQUAREID2] as string);
_values.m_TFGeZiSquareId [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_TFGEZISQUAREID3] as string);
_values.m_TFGeZiSquareId [ 3 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_TFGEZISQUAREID4] as string);
_values.m_TFGeZiSquareId [ 4 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_TFGEZISQUAREID5] as string);
_values.m_TFGeZiSquareId [ 5 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_TFGEZISQUAREID6] as string);
_values.m_TFGeZiSquareId [ 6 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_TFGEZISQUAREID7] as string);
_values.m_TFGeZiSquareId [ 7 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_TFGEZISQUAREID8] as string);
_values.m_TFYinXiongSquareId =  Convert.ToInt32(valuesList[(int)_ID.ID_TFYINXIONGSQUAREID] as string);

 _hash[nKey] = _values; }


}
}

