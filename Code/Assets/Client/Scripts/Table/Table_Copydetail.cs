//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Copydetail : ITableOperate
{ private const string TAB_FILE_DATA = "copydetail.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_COPYNAME,
ID_COST_VALUE,
ID_MAP_NAME,
ID_STARNUMNEED,
ID_SCOREREQUEST,
ID_STAR1,
ID_STAR2,
ID_STAR3,
ID_STARMAX,
ID_OPENEDLIMITED,
ID_MAX_GOLDBASE,
ID_REQUESTMISSIONID1,
ID_REQUESTMISSIONNUM1,
ID_REQUESTMISSIONID2,
ID_REQUESTMISSIONNUM2,
ID_REQUESTMISSIONID3,
ID_REQUESTMISSIONNUM3,
ID_REQUESTMISSIONID4,
ID_REQUESTMISSIONNUM4,
ID_COPYTYPE,
ID_MOVELIMITED_NUM,
ID_CANPRODUCEID1,
ID_CANEXISTNUM1,
ID_CANPRODUCEMAX1,
ID_CANPRODUCEID2,
ID_CANEXISTNUM2,
ID_CANPRODUCEMAX2,
ID_CANPRODUCEID3,
ID_CANEXISTNUM3,
ID_CANPRODUCEMAX3,
ID_CANPRODUCEID4,
ID_CANEXISTNUM4,
ID_CANPRODUCEMAX4,
ID_CANPRODUCEID5,
ID_CANEXISTNUM5,
ID_CANPRODUCEMAX5,
ID_CANPRODUCEID6,
ID_CANEXISTNUM6,
ID_CANPRODUCEMAX6,
ID_CANPRODUCEID7,
ID_CANEXISTNUM7,
ID_CANPRODUCEMAX7,
ID_CANPRODUCEID8,
ID_CANEXISTNUM8,
ID_CANPRODUCEMAX8,
ID_CANPRODUCEID9,
ID_CANEXISTNUM9,
ID_CANPRODUCEMAX9,
ID_CANPRODUCEID10,
ID_CANEXISTNUM10,
ID_CANPRODUCEMAX10,
ID_BOM_EXISTMAX,
ID_BOM_PRODUCEMAX,
ID_BOM_PRO,
ID_EVENTID,
ID_COPY_LIMIT,
ID_FONT_TEXT,
ID_BACK_TEXT,
ID_INFORMATION_NOTICE,
ID_COPYMODE,
ID_TFID,
ID_LASTROWSHOW,
ID_GUILDELEDATA,
ID_GUILDOPERATIONCOUNT,
ID_GUILDLEVEL,
ID_FREEERNIE,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_BomExistMax;
 public int BomExistMax { get{ return m_BomExistMax;}}

private int m_BomPro;
 public int BomPro { get{ return m_BomPro;}}

private int m_BomProduceMAx;
 public int BomProduceMAx { get{ return m_BomProduceMAx;}}

private int[] m_CanExistNum = new int[10];
 public int GetCanExistNumbyIndex(int idx) {
 if(idx>=0 && idx<10) return m_CanExistNum[idx];
 return -1;
 }

private int[] m_CanProduceID = new int[10];
 public int GetCanProduceIDbyIndex(int idx) {
 if(idx>=0 && idx<10) return m_CanProduceID[idx];
 return -1;
 }

private int[] m_CanProduceMax = new int[10];
 public int GetCanProduceMaxbyIndex(int idx) {
 if(idx>=0 && idx<10) return m_CanProduceMax[idx];
 return -1;
 }

private int m_CopyMode;
 public int CopyMode { get{ return m_CopyMode;}}

private int m_CopyType;
 public int CopyType { get{ return m_CopyType;}}

private int m_FreeErnie;
 public int FreeErnie { get{ return m_FreeErnie;}}

private string m_GuildEleData;
 public string GuildEleData { get{ return m_GuildEleData;}}

private int m_GuildLevel;
 public int GuildLevel { get{ return m_GuildLevel;}}

private int m_GuildOperationCount;
 public int GuildOperationCount { get{ return m_GuildOperationCount;}}

private int m_InformationNotice;
 public int InformationNotice { get{ return m_InformationNotice;}}

private int m_MoveLimitedNum;
 public int MoveLimitedNum { get{ return m_MoveLimitedNum;}}

private int m_ScoreRequest;
 public int ScoreRequest { get{ return m_ScoreRequest;}}

private int[] m_Star = new int[3];
 public int GetStarbyIndex(int idx) {
 if(idx>=0 && idx<3) return m_Star[idx];
 return -1;
 }

private int m_StarMax;
 public int StarMax { get{ return m_StarMax;}}

private int m_StarNumNeed;
 public int StarNumNeed { get{ return m_StarNumNeed;}}

private int m_TFID;
 public int TFID { get{ return m_TFID;}}

private int m_BackText;
 public int BackText { get{ return m_BackText;}}

private int m_CopyLimit;
 public int CopyLimit { get{ return m_CopyLimit;}}

private int m_Copyname;
 public int Copyname { get{ return m_Copyname;}}

private int m_CostValue;
 public int CostValue { get{ return m_CostValue;}}

private int m_EventID;
 public int EventID { get{ return m_EventID;}}

private int m_FontText;
 public int FontText { get{ return m_FontText;}}

private int m_LastRowShow;
 public int LastRowShow { get{ return m_LastRowShow;}}

private string m_MapName;
 public string MapName { get{ return m_MapName;}}

private int m_MaxGoldBase;
 public int MaxGoldBase { get{ return m_MaxGoldBase;}}

private int m_OpenedLimited;
 public int OpenedLimited { get{ return m_OpenedLimited;}}

private int[] m_RequestMissionID = new int[4];
 public int GetRequestMissionIDbyIndex(int idx) {
 if(idx>=0 && idx<4) return m_RequestMissionID[idx];
 return -1;
 }

private int[] m_RequestMissionNum = new int[4];
 public int GetRequestMissionNumbyIndex(int idx) {
 if(idx>=0 && idx<4) return m_RequestMissionNum[idx];
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
 Tab_Copydetail _values = new Tab_Copydetail();
 _values.m_BomExistMax =  Convert.ToInt32(valuesList[(int)_ID.ID_BOM_EXISTMAX] as string);
_values.m_BomPro =  Convert.ToInt32(valuesList[(int)_ID.ID_BOM_PRO] as string);
_values.m_BomProduceMAx =  Convert.ToInt32(valuesList[(int)_ID.ID_BOM_PRODUCEMAX] as string);
_values.m_CanExistNum [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANEXISTNUM1] as string);
_values.m_CanExistNum [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANEXISTNUM2] as string);
_values.m_CanExistNum [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANEXISTNUM3] as string);
_values.m_CanExistNum [ 3 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANEXISTNUM4] as string);
_values.m_CanExistNum [ 4 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANEXISTNUM5] as string);
_values.m_CanExistNum [ 5 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANEXISTNUM6] as string);
_values.m_CanExistNum [ 6 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANEXISTNUM7] as string);
_values.m_CanExistNum [ 7 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANEXISTNUM8] as string);
_values.m_CanExistNum [ 8 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANEXISTNUM9] as string);
_values.m_CanExistNum [ 9 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANEXISTNUM10] as string);
_values.m_CanProduceID [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEID1] as string);
_values.m_CanProduceID [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEID2] as string);
_values.m_CanProduceID [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEID3] as string);
_values.m_CanProduceID [ 3 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEID4] as string);
_values.m_CanProduceID [ 4 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEID5] as string);
_values.m_CanProduceID [ 5 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEID6] as string);
_values.m_CanProduceID [ 6 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEID7] as string);
_values.m_CanProduceID [ 7 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEID8] as string);
_values.m_CanProduceID [ 8 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEID9] as string);
_values.m_CanProduceID [ 9 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEID10] as string);
_values.m_CanProduceMax [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEMAX1] as string);
_values.m_CanProduceMax [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEMAX2] as string);
_values.m_CanProduceMax [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEMAX3] as string);
_values.m_CanProduceMax [ 3 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEMAX4] as string);
_values.m_CanProduceMax [ 4 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEMAX5] as string);
_values.m_CanProduceMax [ 5 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEMAX6] as string);
_values.m_CanProduceMax [ 6 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEMAX7] as string);
_values.m_CanProduceMax [ 7 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEMAX8] as string);
_values.m_CanProduceMax [ 8 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEMAX9] as string);
_values.m_CanProduceMax [ 9 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_CANPRODUCEMAX10] as string);
_values.m_CopyMode =  Convert.ToInt32(valuesList[(int)_ID.ID_COPYMODE] as string);
_values.m_CopyType =  Convert.ToInt32(valuesList[(int)_ID.ID_COPYTYPE] as string);
_values.m_FreeErnie =  Convert.ToInt32(valuesList[(int)_ID.ID_FREEERNIE] as string);
_values.m_GuildEleData =  valuesList[(int)_ID.ID_GUILDELEDATA] as string;
_values.m_GuildLevel =  Convert.ToInt32(valuesList[(int)_ID.ID_GUILDLEVEL] as string);
_values.m_GuildOperationCount =  Convert.ToInt32(valuesList[(int)_ID.ID_GUILDOPERATIONCOUNT] as string);
_values.m_InformationNotice =  Convert.ToInt32(valuesList[(int)_ID.ID_INFORMATION_NOTICE] as string);
_values.m_MoveLimitedNum =  Convert.ToInt32(valuesList[(int)_ID.ID_MOVELIMITED_NUM] as string);
_values.m_ScoreRequest =  Convert.ToInt32(valuesList[(int)_ID.ID_SCOREREQUEST] as string);
_values.m_Star [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_STAR1] as string);
_values.m_Star [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_STAR2] as string);
_values.m_Star [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_STAR3] as string);
_values.m_StarMax =  Convert.ToInt32(valuesList[(int)_ID.ID_STARMAX] as string);
_values.m_StarNumNeed =  Convert.ToInt32(valuesList[(int)_ID.ID_STARNUMNEED] as string);
_values.m_TFID =  Convert.ToInt32(valuesList[(int)_ID.ID_TFID] as string);
_values.m_BackText =  Convert.ToInt32(valuesList[(int)_ID.ID_BACK_TEXT] as string);
_values.m_CopyLimit =  Convert.ToInt32(valuesList[(int)_ID.ID_COPY_LIMIT] as string);
_values.m_Copyname =  Convert.ToInt32(valuesList[(int)_ID.ID_COPYNAME] as string);
_values.m_CostValue =  Convert.ToInt32(valuesList[(int)_ID.ID_COST_VALUE] as string);
_values.m_EventID =  Convert.ToInt32(valuesList[(int)_ID.ID_EVENTID] as string);
_values.m_FontText =  Convert.ToInt32(valuesList[(int)_ID.ID_FONT_TEXT] as string);
_values.m_LastRowShow =  Convert.ToInt32(valuesList[(int)_ID.ID_LASTROWSHOW] as string);
_values.m_MapName =  valuesList[(int)_ID.ID_MAP_NAME] as string;
_values.m_MaxGoldBase =  Convert.ToInt32(valuesList[(int)_ID.ID_MAX_GOLDBASE] as string);
_values.m_OpenedLimited =  Convert.ToInt32(valuesList[(int)_ID.ID_OPENEDLIMITED] as string);
_values.m_RequestMissionID [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_REQUESTMISSIONID1] as string);
_values.m_RequestMissionID [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_REQUESTMISSIONID2] as string);
_values.m_RequestMissionID [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_REQUESTMISSIONID3] as string);
_values.m_RequestMissionID [ 3 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_REQUESTMISSIONID4] as string);
_values.m_RequestMissionNum [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_REQUESTMISSIONNUM1] as string);
_values.m_RequestMissionNum [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_REQUESTMISSIONNUM2] as string);
_values.m_RequestMissionNum [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_REQUESTMISSIONNUM3] as string);
_values.m_RequestMissionNum [ 3 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_REQUESTMISSIONNUM4] as string);

 _hash[nKey] = _values; }


}
}

