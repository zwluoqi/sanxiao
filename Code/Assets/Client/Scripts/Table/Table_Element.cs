//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Element : ITableOperate
{ private const string TAB_FILE_DATA = "element.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_ELE_COLOR,
ID_SCORE_COLOR,
ID_CAN_DROPATDOWN,
ID_CAN_COVERWHENINIT,
ID_CAN_ELIMILATED,
ID_ELIMILATEBYEQUIP,
ID_ELIMILATEBYMIRACLE,
ID_ELIMILATEBYROW,
ID_ELIMILATEBYCOL,
ID_ELIMILATEBYBOM,
ID_ELIMILATEBYCHECK,
ID_CAN_ELEBYSAME,
ID_REWARD_IDINCOPY,
ID_REWARD_NUMINCOPY,
ID_PRODUCE_DROPBAGID,
ID_PRODUCE_DROPBAGNUM,
ID_PRODUCE_DATAID,
ID_PRODUCE_DATANUM,
ID_PRODUCE_MISSIONID,
ID_PRODUCE_MISSIONNUM,
ID_COLLECTIVE_MISSIONID,
ID_COLLECTIVE_MISSIONNUM,
ID_DROP_MISSIONID,
ID_DROP_MISSION_NUM,
ID_CROSS_ELE,
ID_THREE_ELE,
ID_FOUR_ELEROW,
ID_FOUR_ELECOL,
ID_FIVE_ELE,
ID_AFFECT_TYPE,
ID_BASE_SCORE,
ID_NORMAL_SPRITE,
ID_HOVER_SPRITE,
ID_CLICK_SPRITE,
ID_DISABLE_SPRITE,
ID_PARTICLE1,
ID_PARTICLE2,
ID_PARTICLE3,
ID_ELIMINATESOUND,
ID_PRODUCESOUND,
ID_PRODUCEPARTICLE,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private int m_AffectType;
 public int AffectType { get{ return m_AffectType;}}

private int m_BaseScore;
 public int BaseScore { get{ return m_BaseScore;}}

private int m_CanCoverWhenInit;
 public int CanCoverWhenInit { get{ return m_CanCoverWhenInit;}}

private int m_CanDropatdown;
 public int CanDropatdown { get{ return m_CanDropatdown;}}

private int m_CanEleBySame;
 public int CanEleBySame { get{ return m_CanEleBySame;}}

private int m_CanElimilated;
 public int CanElimilated { get{ return m_CanElimilated;}}

private string m_ClickSprite;
 public string ClickSprite { get{ return m_ClickSprite;}}

private int m_CollectiveMissionid;
 public int CollectiveMissionid { get{ return m_CollectiveMissionid;}}

private int m_CollectiveMissionnum;
 public int CollectiveMissionnum { get{ return m_CollectiveMissionnum;}}

private int m_CrossEle;
 public int CrossEle { get{ return m_CrossEle;}}

private string m_DisableSprite;
 public string DisableSprite { get{ return m_DisableSprite;}}

private int m_DropMissionNum;
 public int DropMissionNum { get{ return m_DropMissionNum;}}

private int m_DropMissionid;
 public int DropMissionid { get{ return m_DropMissionid;}}

private int m_EleColor;
 public int EleColor { get{ return m_EleColor;}}

private int m_ElimilateByCheck;
 public int ElimilateByCheck { get{ return m_ElimilateByCheck;}}

private int m_ElimilateByEquip;
 public int ElimilateByEquip { get{ return m_ElimilateByEquip;}}

private int m_ElimilateByMiracle;
 public int ElimilateByMiracle { get{ return m_ElimilateByMiracle;}}

private int m_ElimilateBybom;
 public int ElimilateBybom { get{ return m_ElimilateBybom;}}

private int m_ElimilateBycol;
 public int ElimilateBycol { get{ return m_ElimilateBycol;}}

private int m_ElimilateByrow;
 public int ElimilateByrow { get{ return m_ElimilateByrow;}}

private string m_EliminateSound;
 public string EliminateSound { get{ return m_EliminateSound;}}

private int m_FiveEle;
 public int FiveEle { get{ return m_FiveEle;}}

private int m_FourElecol;
 public int FourElecol { get{ return m_FourElecol;}}

private int m_FourElerow;
 public int FourElerow { get{ return m_FourElerow;}}

private string m_HoverSprite;
 public string HoverSprite { get{ return m_HoverSprite;}}

private string m_NormalSprite;
 public string NormalSprite { get{ return m_NormalSprite;}}

private int[] m_Particle = new int[3];
 public int GetParticlebyIndex(int idx) {
 if(idx>=0 && idx<3) return m_Particle[idx];
 return -1;
 }

private int m_ProduceParticle;
 public int ProduceParticle { get{ return m_ProduceParticle;}}

private string m_ProduceSound;
 public string ProduceSound { get{ return m_ProduceSound;}}

private int m_ProduceDataid;
 public int ProduceDataid { get{ return m_ProduceDataid;}}

private int m_ProduceDatanum;
 public int ProduceDatanum { get{ return m_ProduceDatanum;}}

private int m_ProduceDropbagid;
 public int ProduceDropbagid { get{ return m_ProduceDropbagid;}}

private int m_ProduceDropbagnum;
 public int ProduceDropbagnum { get{ return m_ProduceDropbagnum;}}

private int m_ProduceMissionid;
 public int ProduceMissionid { get{ return m_ProduceMissionid;}}

private int m_ProduceMissionnum;
 public int ProduceMissionnum { get{ return m_ProduceMissionnum;}}

private int m_RewardIdInCopy;
 public int RewardIdInCopy { get{ return m_RewardIdInCopy;}}

private int m_RewardNumInCopy;
 public int RewardNumInCopy { get{ return m_RewardNumInCopy;}}

private string m_ScoreColor;
 public string ScoreColor { get{ return m_ScoreColor;}}

private int m_ThreeEle;
 public int ThreeEle { get{ return m_ThreeEle;}}

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
 Tab_Element _values = new Tab_Element();
 _values.m_AffectType =  Convert.ToInt32(valuesList[(int)_ID.ID_AFFECT_TYPE] as string);
_values.m_BaseScore =  Convert.ToInt32(valuesList[(int)_ID.ID_BASE_SCORE] as string);
_values.m_CanCoverWhenInit =  Convert.ToInt32(valuesList[(int)_ID.ID_CAN_COVERWHENINIT] as string);
_values.m_CanDropatdown =  Convert.ToInt32(valuesList[(int)_ID.ID_CAN_DROPATDOWN] as string);
_values.m_CanEleBySame =  Convert.ToInt32(valuesList[(int)_ID.ID_CAN_ELEBYSAME] as string);
_values.m_CanElimilated =  Convert.ToInt32(valuesList[(int)_ID.ID_CAN_ELIMILATED] as string);
_values.m_ClickSprite =  valuesList[(int)_ID.ID_CLICK_SPRITE] as string;
_values.m_CollectiveMissionid =  Convert.ToInt32(valuesList[(int)_ID.ID_COLLECTIVE_MISSIONID] as string);
_values.m_CollectiveMissionnum =  Convert.ToInt32(valuesList[(int)_ID.ID_COLLECTIVE_MISSIONNUM] as string);
_values.m_CrossEle =  Convert.ToInt32(valuesList[(int)_ID.ID_CROSS_ELE] as string);
_values.m_DisableSprite =  valuesList[(int)_ID.ID_DISABLE_SPRITE] as string;
_values.m_DropMissionNum =  Convert.ToInt32(valuesList[(int)_ID.ID_DROP_MISSION_NUM] as string);
_values.m_DropMissionid =  Convert.ToInt32(valuesList[(int)_ID.ID_DROP_MISSIONID] as string);
_values.m_EleColor =  Convert.ToInt32(valuesList[(int)_ID.ID_ELE_COLOR] as string);
_values.m_ElimilateByCheck =  Convert.ToInt32(valuesList[(int)_ID.ID_ELIMILATEBYCHECK] as string);
_values.m_ElimilateByEquip =  Convert.ToInt32(valuesList[(int)_ID.ID_ELIMILATEBYEQUIP] as string);
_values.m_ElimilateByMiracle =  Convert.ToInt32(valuesList[(int)_ID.ID_ELIMILATEBYMIRACLE] as string);
_values.m_ElimilateBybom =  Convert.ToInt32(valuesList[(int)_ID.ID_ELIMILATEBYBOM] as string);
_values.m_ElimilateBycol =  Convert.ToInt32(valuesList[(int)_ID.ID_ELIMILATEBYCOL] as string);
_values.m_ElimilateByrow =  Convert.ToInt32(valuesList[(int)_ID.ID_ELIMILATEBYROW] as string);
_values.m_EliminateSound =  valuesList[(int)_ID.ID_ELIMINATESOUND] as string;
_values.m_FiveEle =  Convert.ToInt32(valuesList[(int)_ID.ID_FIVE_ELE] as string);
_values.m_FourElecol =  Convert.ToInt32(valuesList[(int)_ID.ID_FOUR_ELECOL] as string);
_values.m_FourElerow =  Convert.ToInt32(valuesList[(int)_ID.ID_FOUR_ELEROW] as string);
_values.m_HoverSprite =  valuesList[(int)_ID.ID_HOVER_SPRITE] as string;
_values.m_NormalSprite =  valuesList[(int)_ID.ID_NORMAL_SPRITE] as string;
_values.m_Particle [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_PARTICLE1] as string);
_values.m_Particle [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_PARTICLE2] as string);
_values.m_Particle [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_PARTICLE3] as string);
_values.m_ProduceParticle =  Convert.ToInt32(valuesList[(int)_ID.ID_PRODUCEPARTICLE] as string);
_values.m_ProduceSound =  valuesList[(int)_ID.ID_PRODUCESOUND] as string;
_values.m_ProduceDataid =  Convert.ToInt32(valuesList[(int)_ID.ID_PRODUCE_DATAID] as string);
_values.m_ProduceDatanum =  Convert.ToInt32(valuesList[(int)_ID.ID_PRODUCE_DATANUM] as string);
_values.m_ProduceDropbagid =  Convert.ToInt32(valuesList[(int)_ID.ID_PRODUCE_DROPBAGID] as string);
_values.m_ProduceDropbagnum =  Convert.ToInt32(valuesList[(int)_ID.ID_PRODUCE_DROPBAGNUM] as string);
_values.m_ProduceMissionid =  Convert.ToInt32(valuesList[(int)_ID.ID_PRODUCE_MISSIONID] as string);
_values.m_ProduceMissionnum =  Convert.ToInt32(valuesList[(int)_ID.ID_PRODUCE_MISSIONNUM] as string);
_values.m_RewardIdInCopy =  Convert.ToInt32(valuesList[(int)_ID.ID_REWARD_IDINCOPY] as string);
_values.m_RewardNumInCopy =  Convert.ToInt32(valuesList[(int)_ID.ID_REWARD_NUMINCOPY] as string);
_values.m_ScoreColor =  valuesList[(int)_ID.ID_SCORE_COLOR] as string;
_values.m_ThreeEle =  Convert.ToInt32(valuesList[(int)_ID.ID_THREE_ELE] as string);

 _hash[nKey] = _values; }


}
}

