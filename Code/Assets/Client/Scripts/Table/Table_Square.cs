//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_Square : ITableOperate
{ private const string TAB_FILE_DATA = "square.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_DETAIL,
ID_SCORE_COLOR,
ID_IFWAI_KUANG,
ID_UPDOWN,
ID_CAN_MOVE,
ID_CAN_THROUGH,
ID_CAN_COVERBYOTHER,
ID_ELIMINATEBEFOREREMOVED,
ID_ELIMILATEBYEQUIP,
ID_ELIMILATEBYMIRACLE,
ID_ELIMILATEBYSPECIAL,
ID_ELIMILATEBYCHECK,
ID_ADDIFNOTDISAPPEAR,
ID_MOVEIFNOTDISAPPEAR,
ID_DISAPPEARBYSELF,
ID_DISAPPEARBYOTHER,
ID_SQUAREATDOWN,
ID_EQUIPID,
ID_EQUIPNUM,
ID_MISSIONID,
ID_MISSIONNUM,
ID_DATAID,
ID_DATANUM,
ID_BASE_SCORE,
ID_SPRITENAME,
ID_SPRITENAMEINEDITOR,
ID_DEFAULTHP,
ID_PARTICLE1,
ID_PARTICLE2,
ID_PARTICLE3,
ID_ELIMINATESOUND,
ID_PRODUCESOUND,
ID_PRODUCEPARTICLE,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private string m_SpriteNameInEditor;
 public string SpriteNameInEditor { get{ return m_SpriteNameInEditor;}}

private int m_Addifnotdisappear;
 public int Addifnotdisappear { get{ return m_Addifnotdisappear;}}

private int m_BaseScore;
 public int BaseScore { get{ return m_BaseScore;}}

private int m_CanCoverByOther;
 public int CanCoverByOther { get{ return m_CanCoverByOther;}}

private int m_CanMove;
 public int CanMove { get{ return m_CanMove;}}

private int m_CanThrough;
 public int CanThrough { get{ return m_CanThrough;}}

private int m_DataID;
 public int DataID { get{ return m_DataID;}}

private int m_DataNum;
 public int DataNum { get{ return m_DataNum;}}

private int m_DefaultHP;
 public int DefaultHP { get{ return m_DefaultHP;}}

private string m_Detail;
 public string Detail { get{ return m_Detail;}}

private int m_DisappearByOther;
 public int DisappearByOther { get{ return m_DisappearByOther;}}

private int m_DisappearBySelf;
 public int DisappearBySelf { get{ return m_DisappearBySelf;}}

private int m_ElimilateByCheck;
 public int ElimilateByCheck { get{ return m_ElimilateByCheck;}}

private int m_ElimilateByEquip;
 public int ElimilateByEquip { get{ return m_ElimilateByEquip;}}

private int m_ElimilateByMiracle;
 public int ElimilateByMiracle { get{ return m_ElimilateByMiracle;}}

private int m_ElimilateBySpecial;
 public int ElimilateBySpecial { get{ return m_ElimilateBySpecial;}}

private int m_EliminateBeforeRemoved;
 public int EliminateBeforeRemoved { get{ return m_EliminateBeforeRemoved;}}

private string m_EliminateSound;
 public string EliminateSound { get{ return m_EliminateSound;}}

private int m_EquipID;
 public int EquipID { get{ return m_EquipID;}}

private int m_EquipNum;
 public int EquipNum { get{ return m_EquipNum;}}

private int m_IfwaiKuang;
 public int IfwaiKuang { get{ return m_IfwaiKuang;}}

private int m_MissionID;
 public int MissionID { get{ return m_MissionID;}}

private int m_MissionNum;
 public int MissionNum { get{ return m_MissionNum;}}

private int m_Moveifnotdisappear;
 public int Moveifnotdisappear { get{ return m_Moveifnotdisappear;}}

private int[] m_Particle = new int[3];
 public int GetParticlebyIndex(int idx) {
 if(idx>=0 && idx<3) return m_Particle[idx];
 return -1;
 }

private int m_ProduceParticle;
 public int ProduceParticle { get{ return m_ProduceParticle;}}

private string m_ProduceSound;
 public string ProduceSound { get{ return m_ProduceSound;}}

private string m_ScoreColor;
 public string ScoreColor { get{ return m_ScoreColor;}}

private string m_SpriteName;
 public string SpriteName { get{ return m_SpriteName;}}

private int m_SquareAtDown;
 public int SquareAtDown { get{ return m_SquareAtDown;}}

private int m_Updown;
 public int Updown { get{ return m_Updown;}}

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
 Tab_Square _values = new Tab_Square();
 _values.m_SpriteNameInEditor =  valuesList[(int)_ID.ID_SPRITENAMEINEDITOR] as string;
_values.m_Addifnotdisappear =  Convert.ToInt32(valuesList[(int)_ID.ID_ADDIFNOTDISAPPEAR] as string);
_values.m_BaseScore =  Convert.ToInt32(valuesList[(int)_ID.ID_BASE_SCORE] as string);
_values.m_CanCoverByOther =  Convert.ToInt32(valuesList[(int)_ID.ID_CAN_COVERBYOTHER] as string);
_values.m_CanMove =  Convert.ToInt32(valuesList[(int)_ID.ID_CAN_MOVE] as string);
_values.m_CanThrough =  Convert.ToInt32(valuesList[(int)_ID.ID_CAN_THROUGH] as string);
_values.m_DataID =  Convert.ToInt32(valuesList[(int)_ID.ID_DATAID] as string);
_values.m_DataNum =  Convert.ToInt32(valuesList[(int)_ID.ID_DATANUM] as string);
_values.m_DefaultHP =  Convert.ToInt32(valuesList[(int)_ID.ID_DEFAULTHP] as string);
_values.m_Detail =  valuesList[(int)_ID.ID_DETAIL] as string;
_values.m_DisappearByOther =  Convert.ToInt32(valuesList[(int)_ID.ID_DISAPPEARBYOTHER] as string);
_values.m_DisappearBySelf =  Convert.ToInt32(valuesList[(int)_ID.ID_DISAPPEARBYSELF] as string);
_values.m_ElimilateByCheck =  Convert.ToInt32(valuesList[(int)_ID.ID_ELIMILATEBYCHECK] as string);
_values.m_ElimilateByEquip =  Convert.ToInt32(valuesList[(int)_ID.ID_ELIMILATEBYEQUIP] as string);
_values.m_ElimilateByMiracle =  Convert.ToInt32(valuesList[(int)_ID.ID_ELIMILATEBYMIRACLE] as string);
_values.m_ElimilateBySpecial =  Convert.ToInt32(valuesList[(int)_ID.ID_ELIMILATEBYSPECIAL] as string);
_values.m_EliminateBeforeRemoved =  Convert.ToInt32(valuesList[(int)_ID.ID_ELIMINATEBEFOREREMOVED] as string);
_values.m_EliminateSound =  valuesList[(int)_ID.ID_ELIMINATESOUND] as string;
_values.m_EquipID =  Convert.ToInt32(valuesList[(int)_ID.ID_EQUIPID] as string);
_values.m_EquipNum =  Convert.ToInt32(valuesList[(int)_ID.ID_EQUIPNUM] as string);
_values.m_IfwaiKuang =  Convert.ToInt32(valuesList[(int)_ID.ID_IFWAI_KUANG] as string);
_values.m_MissionID =  Convert.ToInt32(valuesList[(int)_ID.ID_MISSIONID] as string);
_values.m_MissionNum =  Convert.ToInt32(valuesList[(int)_ID.ID_MISSIONNUM] as string);
_values.m_Moveifnotdisappear =  Convert.ToInt32(valuesList[(int)_ID.ID_MOVEIFNOTDISAPPEAR] as string);
_values.m_Particle [ 0 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_PARTICLE1] as string);
_values.m_Particle [ 1 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_PARTICLE2] as string);
_values.m_Particle [ 2 ] =  Convert.ToInt32(valuesList[(int)_ID.ID_PARTICLE3] as string);
_values.m_ProduceParticle =  Convert.ToInt32(valuesList[(int)_ID.ID_PRODUCEPARTICLE] as string);
_values.m_ProduceSound =  valuesList[(int)_ID.ID_PRODUCESOUND] as string;
_values.m_ScoreColor =  valuesList[(int)_ID.ID_SCORE_COLOR] as string;
_values.m_SpriteName =  valuesList[(int)_ID.ID_SPRITENAME] as string;
_values.m_SquareAtDown =  Convert.ToInt32(valuesList[(int)_ID.ID_SQUAREATDOWN] as string);
_values.m_Updown =  Convert.ToInt32(valuesList[(int)_ID.ID_UPDOWN] as string);

 _hash[nKey] = _values; }


}
}

