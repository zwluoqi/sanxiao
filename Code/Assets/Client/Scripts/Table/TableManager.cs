//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;
 using System.Xml;
 using UnityEngine;
 using System.IO;

namespace GCGame.Table{

public interface ITableOperate
 {
 bool LoadTable(Hashtable _tab);
 string GetInstanceFile();
 }

 public delegate void SerializableTable(ArrayList valuesList, string skey, Hashtable _hash);
 
[Serializable]
 public class TableManager
{
 public static bool IsLoadFromLocal = false;
 public static bool ReaderPList(String xmlFile, SerializableTable _fun, Hashtable _hash)
 {
 string[] list= xmlFile.Split('.');
 string tableFilePath = Application.persistentDataPath + "/Table/" + list[0] + ".txt";
 string[] alldataRow ;
 if (File.Exists(tableFilePath))
 {
 //Debug.LogWarning("load from txt");
 StreamReader sr = null;
 sr = File.OpenText(tableFilePath);
 string tableData = sr.ReadToEnd();
 alldataRow = tableData.Split('\n');
 }
 else
 {
 //Debug.LogWarning("load from localdata");
 TextAsset testAsset = Resources.Load("Table/"+list[0], typeof(TextAsset)) as TextAsset;
 alldataRow = testAsset.text.Split('\n');
 }
 foreach(string line in alldataRow)
 {
 if(String.IsNullOrEmpty(line))continue;
 string[] strCol = line.Split('\t');
 if (strCol.Length == 0) continue;
 string skey = strCol[0];
 if (string.IsNullOrEmpty(skey)) return false;
 ArrayList valuesList = new ArrayList();
 for (int i = 1; i < strCol.Length;++i )
 {
 valuesList.Add(strCol[i]);
 }
 _fun(valuesList, skey, _hash);
 }
 return true;
 }


private static Hashtable g_TableModel = new Hashtable();
 public static Hashtable GetTableModel()
 {
 if (g_TableModel.Count == 0)
 {
 Tab_TableModel s_Tab_TableModel = new Tab_TableModel();
 s_Tab_TableModel.LoadTable(g_TableModel);
 }
 return g_TableModel;
 }

private static Hashtable g_ChargeGift = new Hashtable();
 public static Hashtable GetChargeGift()
 {
 if (g_ChargeGift.Count == 0)
 {
 Tab_ChargeGift s_Tab_ChargeGift = new Tab_ChargeGift();
 s_Tab_ChargeGift.LoadTable(g_ChargeGift);
 }
 return g_ChargeGift;
 }

private static Hashtable g_Copydetail = new Hashtable();
 public static Hashtable GetCopydetail()
 {
 if (g_Copydetail.Count == 0)
 {
 Tab_Copydetail s_Tab_Copydetail = new Tab_Copydetail();
 s_Tab_Copydetail.LoadTable(g_Copydetail);
 }
 return g_Copydetail;
 }

private static Hashtable g_Copystory = new Hashtable();
 public static Hashtable GetCopystory()
 {
 if (g_Copystory.Count == 0)
 {
 Tab_Copystory s_Tab_Copystory = new Tab_Copystory();
 s_Tab_Copystory.LoadTable(g_Copystory);
 }
 return g_Copystory;
 }

private static Hashtable g_Dictclass = new Hashtable();
 public static Hashtable GetDictclass()
 {
 if (g_Dictclass.Count == 0)
 {
 Tab_Dictclass s_Tab_Dictclass = new Tab_Dictclass();
 s_Tab_Dictclass.LoadTable(g_Dictclass);
 }
 return g_Dictclass;
 }

private static Hashtable g_Drop = new Hashtable();
 public static Hashtable GetDrop()
 {
 if (g_Drop.Count == 0)
 {
 Tab_Drop s_Tab_Drop = new Tab_Drop();
 s_Tab_Drop.LoadTable(g_Drop);
 }
 return g_Drop;
 }

private static Hashtable g_Element = new Hashtable();
 public static Hashtable GetElement()
 {
 if (g_Element.Count == 0)
 {
 Tab_Element s_Tab_Element = new Tab_Element();
 s_Tab_Element.LoadTable(g_Element);
 }
 return g_Element;
 }

private static Hashtable g_Equip = new Hashtable();
 public static Hashtable GetEquip()
 {
 if (g_Equip.Count == 0)
 {
 Tab_Equip s_Tab_Equip = new Tab_Equip();
 s_Tab_Equip.LoadTable(g_Equip);
 }
 return g_Equip;
 }

private static Hashtable g_Equipback = new Hashtable();
 public static Hashtable GetEquipback()
 {
 if (g_Equipback.Count == 0)
 {
 Tab_Equipback s_Tab_Equipback = new Tab_Equipback();
 s_Tab_Equipback.LoadTable(g_Equipback);
 }
 return g_Equipback;
 }

private static Hashtable g_Equipshop = new Hashtable();
 public static Hashtable GetEquipshop()
 {
 if (g_Equipshop.Count == 0)
 {
 Tab_Equipshop s_Tab_Equipshop = new Tab_Equipshop();
 s_Tab_Equipshop.LoadTable(g_Equipshop);
 }
 return g_Equipshop;
 }

private static Hashtable g_Mission = new Hashtable();
 public static Hashtable GetMission()
 {
 if (g_Mission.Count == 0)
 {
 Tab_Mission s_Tab_Mission = new Tab_Mission();
 s_Tab_Mission.LoadTable(g_Mission);
 }
 return g_Mission;
 }

private static Hashtable g_ParticleAnimation = new Hashtable();
 public static Hashtable GetParticleAnimation()
 {
 if (g_ParticleAnimation.Count == 0)
 {
 Tab_ParticleAnimation s_Tab_ParticleAnimation = new Tab_ParticleAnimation();
 s_Tab_ParticleAnimation.LoadTable(g_ParticleAnimation);
 }
 return g_ParticleAnimation;
 }

private static Hashtable g_Platformrubyconfig = new Hashtable();
 public static Hashtable GetPlatformrubyconfig()
 {
 if (g_Platformrubyconfig.Count == 0)
 {
 Tab_Platformrubyconfig s_Tab_Platformrubyconfig = new Tab_Platformrubyconfig();
 s_Tab_Platformrubyconfig.LoadTable(g_Platformrubyconfig);
 }
 return g_Platformrubyconfig;
 }

private static Hashtable g_Powershop = new Hashtable();
 public static Hashtable GetPowershop()
 {
 if (g_Powershop.Count == 0)
 {
 Tab_Powershop s_Tab_Powershop = new Tab_Powershop();
 s_Tab_Powershop.LoadTable(g_Powershop);
 }
 return g_Powershop;
 }

private static Hashtable g_PushMessage = new Hashtable();
 public static Hashtable GetPushMessage()
 {
 if (g_PushMessage.Count == 0)
 {
 Tab_PushMessage s_Tab_PushMessage = new Tab_PushMessage();
 s_Tab_PushMessage.LoadTable(g_PushMessage);
 }
 return g_PushMessage;
 }

private static Hashtable g_Rubyshop = new Hashtable();
 public static Hashtable GetRubyshop()
 {
 if (g_Rubyshop.Count == 0)
 {
 Tab_Rubyshop s_Tab_Rubyshop = new Tab_Rubyshop();
 s_Tab_Rubyshop.LoadTable(g_Rubyshop);
 }
 return g_Rubyshop;
 }

private static Hashtable g_Sign = new Hashtable();
 public static Hashtable GetSign()
 {
 if (g_Sign.Count == 0)
 {
 Tab_Sign s_Tab_Sign = new Tab_Sign();
 s_Tab_Sign.LoadTable(g_Sign);
 }
 return g_Sign;
 }

private static Hashtable g_Square = new Hashtable();
 public static Hashtable GetSquare()
 {
 if (g_Square.Count == 0)
 {
 Tab_Square s_Tab_Square = new Tab_Square();
 s_Tab_Square.LoadTable(g_Square);
 }
 return g_Square;
 }

private static Hashtable g_TAchievement = new Hashtable();
 public static Hashtable GetTAchievement()
 {
 if (g_TAchievement.Count == 0)
 {
 Tab_TAchievement s_Tab_TAchievement = new Tab_TAchievement();
 s_Tab_TAchievement.LoadTable(g_TAchievement);
 }
 return g_TAchievement;
 }

private static Hashtable g_TAchievementSpecialChapter = new Hashtable();
 public static Hashtable GetTAchievementSpecialChapter()
 {
 if (g_TAchievementSpecialChapter.Count == 0)
 {
 Tab_TAchievementSpecialChapter s_Tab_TAchievementSpecialChapter = new Tab_TAchievementSpecialChapter();
 s_Tab_TAchievementSpecialChapter.LoadTable(g_TAchievementSpecialChapter);
 }
 return g_TAchievementSpecialChapter;
 }

private static Hashtable g_TItem = new Hashtable();
 public static Hashtable GetTItem()
 {
 if (g_TItem.Count == 0)
 {
 Tab_TItem s_Tab_TItem = new Tab_TItem();
 s_Tab_TItem.LoadTable(g_TItem);
 }
 return g_TItem;
 }

private static Hashtable g_TRelation = new Hashtable();
 public static Hashtable GetTRelation()
 {
 if (g_TRelation.Count == 0)
 {
 Tab_TRelation s_Tab_TRelation = new Tab_TRelation();
 s_Tab_TRelation.LoadTable(g_TRelation);
 }
 return g_TRelation;
 }

private static Hashtable g_TSpecialChapter = new Hashtable();
 public static Hashtable GetTSpecialChapter()
 {
 if (g_TSpecialChapter.Count == 0)
 {
 Tab_TSpecialChapter s_Tab_TSpecialChapter = new Tab_TSpecialChapter();
 s_Tab_TSpecialChapter.LoadTable(g_TSpecialChapter);
 }
 return g_TSpecialChapter;
 }

private static Hashtable g_TUser = new Hashtable();
 public static Hashtable GetTUser()
 {
 if (g_TUser.Count == 0)
 {
 Tab_TUser s_Tab_TUser = new Tab_TUser();
 s_Tab_TUser.LoadTable(g_TUser);
 }
 return g_TUser;
 }

private static Hashtable g_TUserCopy = new Hashtable();
 public static Hashtable GetTUserCopy()
 {
 if (g_TUserCopy.Count == 0)
 {
 Tab_TUserCopy s_Tab_TUserCopy = new Tab_TUserCopy();
 s_Tab_TUserCopy.LoadTable(g_TUserCopy);
 }
 return g_TUserCopy;
 }

private static Hashtable g_TUserEquip = new Hashtable();
 public static Hashtable GetTUserEquip()
 {
 if (g_TUserEquip.Count == 0)
 {
 Tab_TUserEquip s_Tab_TUserEquip = new Tab_TUserEquip();
 s_Tab_TUserEquip.LoadTable(g_TUserEquip);
 }
 return g_TUserEquip;
 }

private static Hashtable g_TourContent = new Hashtable();
 public static Hashtable GetTourContent()
 {
 if (g_TourContent.Count == 0)
 {
 Tab_TourContent s_Tab_TourContent = new Tab_TourContent();
 s_Tab_TourContent.LoadTable(g_TourContent);
 }
 return g_TourContent;
 }

private static Hashtable g_Zhuanpan = new Hashtable();
 public static Hashtable GetZhuanpan()
 {
 if (g_Zhuanpan.Count == 0)
 {
 Tab_Zhuanpan s_Tab_Zhuanpan = new Tab_Zhuanpan();
 s_Tab_Zhuanpan.LoadTable(g_Zhuanpan);
 }
 return g_Zhuanpan;
 }

public static IEnumerator InitTable()
 {
 Tab_TableModel s_Tab_TableModel = new Tab_TableModel();
 if(s_Tab_TableModel.LoadTable(g_TableModel))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_TableModel.GetInstanceFile(),g_TableModel.Count));
 }
 yield return null;

Tab_ChargeGift s_Tab_ChargeGift = new Tab_ChargeGift();
 if(s_Tab_ChargeGift.LoadTable(g_ChargeGift))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_ChargeGift.GetInstanceFile(),g_ChargeGift.Count));
 }
 yield return null;

Tab_Copydetail s_Tab_Copydetail = new Tab_Copydetail();
 if(s_Tab_Copydetail.LoadTable(g_Copydetail))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Copydetail.GetInstanceFile(),g_Copydetail.Count));
 }
 yield return null;

Tab_Copystory s_Tab_Copystory = new Tab_Copystory();
 if(s_Tab_Copystory.LoadTable(g_Copystory))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Copystory.GetInstanceFile(),g_Copystory.Count));
 }
 yield return null;

Tab_Dictclass s_Tab_Dictclass = new Tab_Dictclass();
 if(s_Tab_Dictclass.LoadTable(g_Dictclass))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Dictclass.GetInstanceFile(),g_Dictclass.Count));
 }
 yield return null;

Tab_Drop s_Tab_Drop = new Tab_Drop();
 if(s_Tab_Drop.LoadTable(g_Drop))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Drop.GetInstanceFile(),g_Drop.Count));
 }
 yield return null;

Tab_Element s_Tab_Element = new Tab_Element();
 if(s_Tab_Element.LoadTable(g_Element))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Element.GetInstanceFile(),g_Element.Count));
 }
 yield return null;

Tab_Equip s_Tab_Equip = new Tab_Equip();
 if(s_Tab_Equip.LoadTable(g_Equip))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Equip.GetInstanceFile(),g_Equip.Count));
 }
 yield return null;

Tab_Equipback s_Tab_Equipback = new Tab_Equipback();
 if(s_Tab_Equipback.LoadTable(g_Equipback))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Equipback.GetInstanceFile(),g_Equipback.Count));
 }
 yield return null;

Tab_Equipshop s_Tab_Equipshop = new Tab_Equipshop();
 if(s_Tab_Equipshop.LoadTable(g_Equipshop))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Equipshop.GetInstanceFile(),g_Equipshop.Count));
 }
 yield return null;

Tab_Mission s_Tab_Mission = new Tab_Mission();
 if(s_Tab_Mission.LoadTable(g_Mission))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Mission.GetInstanceFile(),g_Mission.Count));
 }
 yield return null;

Tab_ParticleAnimation s_Tab_ParticleAnimation = new Tab_ParticleAnimation();
 if(s_Tab_ParticleAnimation.LoadTable(g_ParticleAnimation))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_ParticleAnimation.GetInstanceFile(),g_ParticleAnimation.Count));
 }
 yield return null;

Tab_Platformrubyconfig s_Tab_Platformrubyconfig = new Tab_Platformrubyconfig();
 if(s_Tab_Platformrubyconfig.LoadTable(g_Platformrubyconfig))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Platformrubyconfig.GetInstanceFile(),g_Platformrubyconfig.Count));
 }
 yield return null;

Tab_Powershop s_Tab_Powershop = new Tab_Powershop();
 if(s_Tab_Powershop.LoadTable(g_Powershop))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Powershop.GetInstanceFile(),g_Powershop.Count));
 }
 yield return null;

Tab_PushMessage s_Tab_PushMessage = new Tab_PushMessage();
 if(s_Tab_PushMessage.LoadTable(g_PushMessage))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_PushMessage.GetInstanceFile(),g_PushMessage.Count));
 }
 yield return null;

Tab_Rubyshop s_Tab_Rubyshop = new Tab_Rubyshop();
 if(s_Tab_Rubyshop.LoadTable(g_Rubyshop))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Rubyshop.GetInstanceFile(),g_Rubyshop.Count));
 }
 yield return null;

Tab_Sign s_Tab_Sign = new Tab_Sign();
 if(s_Tab_Sign.LoadTable(g_Sign))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Sign.GetInstanceFile(),g_Sign.Count));
 }
 yield return null;

Tab_Square s_Tab_Square = new Tab_Square();
 if(s_Tab_Square.LoadTable(g_Square))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Square.GetInstanceFile(),g_Square.Count));
 }
 yield return null;

Tab_TAchievement s_Tab_TAchievement = new Tab_TAchievement();
 if(s_Tab_TAchievement.LoadTable(g_TAchievement))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_TAchievement.GetInstanceFile(),g_TAchievement.Count));
 }
 yield return null;

Tab_TAchievementSpecialChapter s_Tab_TAchievementSpecialChapter = new Tab_TAchievementSpecialChapter();
 if(s_Tab_TAchievementSpecialChapter.LoadTable(g_TAchievementSpecialChapter))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_TAchievementSpecialChapter.GetInstanceFile(),g_TAchievementSpecialChapter.Count));
 }
 yield return null;

Tab_TItem s_Tab_TItem = new Tab_TItem();
 if(s_Tab_TItem.LoadTable(g_TItem))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_TItem.GetInstanceFile(),g_TItem.Count));
 }
 yield return null;

Tab_TRelation s_Tab_TRelation = new Tab_TRelation();
 if(s_Tab_TRelation.LoadTable(g_TRelation))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_TRelation.GetInstanceFile(),g_TRelation.Count));
 }
 yield return null;

Tab_TSpecialChapter s_Tab_TSpecialChapter = new Tab_TSpecialChapter();
 if(s_Tab_TSpecialChapter.LoadTable(g_TSpecialChapter))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_TSpecialChapter.GetInstanceFile(),g_TSpecialChapter.Count));
 }
 yield return null;

Tab_TUser s_Tab_TUser = new Tab_TUser();
 if(s_Tab_TUser.LoadTable(g_TUser))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_TUser.GetInstanceFile(),g_TUser.Count));
 }
 yield return null;

Tab_TUserCopy s_Tab_TUserCopy = new Tab_TUserCopy();
 if(s_Tab_TUserCopy.LoadTable(g_TUserCopy))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_TUserCopy.GetInstanceFile(),g_TUserCopy.Count));
 }
 yield return null;

Tab_TUserEquip s_Tab_TUserEquip = new Tab_TUserEquip();
 if(s_Tab_TUserEquip.LoadTable(g_TUserEquip))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_TUserEquip.GetInstanceFile(),g_TUserEquip.Count));
 }
 yield return null;

Tab_TourContent s_Tab_TourContent = new Tab_TourContent();
 if(s_Tab_TourContent.LoadTable(g_TourContent))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_TourContent.GetInstanceFile(),g_TourContent.Count));
 }
 yield return null;

Tab_Zhuanpan s_Tab_Zhuanpan = new Tab_Zhuanpan();
 if(s_Tab_Zhuanpan.LoadTable(g_Zhuanpan))
 {
 //Debug.Log(string.Format("Load Table:{0} OK! Record({1})",s_Tab_Zhuanpan.GetInstanceFile(),g_Zhuanpan.Count));
 }
 yield return null;


 }

public static Tab_TableModel GetTableModelByID(int nIdex)
 {
 if( GetTableModel().ContainsKey(nIdex))
 {
 return g_TableModel[nIdex] as Tab_TableModel;
 }
 return null;
 }

public static Tab_ChargeGift GetChargeGiftByID(int nIdex)
 {
 if( GetChargeGift().ContainsKey(nIdex))
 {
 return g_ChargeGift[nIdex] as Tab_ChargeGift;
 }
 return null;
 }

public static Tab_Copydetail GetCopydetailByID(int nIdex)
 {
 if( GetCopydetail().ContainsKey(nIdex))
 {
 return g_Copydetail[nIdex] as Tab_Copydetail;
 }
 return null;
 }

public static Tab_Copystory GetCopystoryByID(int nIdex)
 {
 if( GetCopystory().ContainsKey(nIdex))
 {
 return g_Copystory[nIdex] as Tab_Copystory;
 }
 return null;
 }

public static Tab_Dictclass GetDictclassByID(int nIdex)
 {
 if( GetDictclass().ContainsKey(nIdex))
 {
 return g_Dictclass[nIdex] as Tab_Dictclass;
 }
 return null;
 }

public static Tab_Drop GetDropByID(int nIdex)
 {
 if( GetDrop().ContainsKey(nIdex))
 {
 return g_Drop[nIdex] as Tab_Drop;
 }
 return null;
 }

public static Tab_Element GetElementByID(int nIdex)
 {
 if( GetElement().ContainsKey(nIdex))
 {
 return g_Element[nIdex] as Tab_Element;
 }
 return null;
 }

public static Tab_Equip GetEquipByID(int nIdex)
 {
 if( GetEquip().ContainsKey(nIdex))
 {
 return g_Equip[nIdex] as Tab_Equip;
 }
 return null;
 }

public static Tab_Equipback GetEquipbackByID(int nIdex)
 {
 if( GetEquipback().ContainsKey(nIdex))
 {
 return g_Equipback[nIdex] as Tab_Equipback;
 }
 return null;
 }

public static Tab_Equipshop GetEquipshopByID(int nIdex)
 {
 if( GetEquipshop().ContainsKey(nIdex))
 {
 return g_Equipshop[nIdex] as Tab_Equipshop;
 }
 return null;
 }

public static Tab_Mission GetMissionByID(int nIdex)
 {
 if( GetMission().ContainsKey(nIdex))
 {
 return g_Mission[nIdex] as Tab_Mission;
 }
 return null;
 }

public static Tab_ParticleAnimation GetParticleAnimationByID(int nIdex)
 {
 if( GetParticleAnimation().ContainsKey(nIdex))
 {
 return g_ParticleAnimation[nIdex] as Tab_ParticleAnimation;
 }
 return null;
 }

public static Tab_Platformrubyconfig GetPlatformrubyconfigByID(int nIdex)
 {
 if( GetPlatformrubyconfig().ContainsKey(nIdex))
 {
 return g_Platformrubyconfig[nIdex] as Tab_Platformrubyconfig;
 }
 return null;
 }

public static Tab_Powershop GetPowershopByID(int nIdex)
 {
 if( GetPowershop().ContainsKey(nIdex))
 {
 return g_Powershop[nIdex] as Tab_Powershop;
 }
 return null;
 }

public static Tab_PushMessage GetPushMessageByID(int nIdex)
 {
 if( GetPushMessage().ContainsKey(nIdex))
 {
 return g_PushMessage[nIdex] as Tab_PushMessage;
 }
 return null;
 }

public static Tab_Rubyshop GetRubyshopByID(int nIdex)
 {
 if( GetRubyshop().ContainsKey(nIdex))
 {
 return g_Rubyshop[nIdex] as Tab_Rubyshop;
 }
 return null;
 }

public static Tab_Sign GetSignByID(int nIdex)
 {
 if( GetSign().ContainsKey(nIdex))
 {
 return g_Sign[nIdex] as Tab_Sign;
 }
 return null;
 }

public static Tab_Square GetSquareByID(int nIdex)
 {
 if( GetSquare().ContainsKey(nIdex))
 {
 return g_Square[nIdex] as Tab_Square;
 }
 return null;
 }

public static Tab_TAchievement GetTAchievementByID(int nIdex)
 {
 if( GetTAchievement().ContainsKey(nIdex))
 {
 return g_TAchievement[nIdex] as Tab_TAchievement;
 }
 return null;
 }

public static Tab_TAchievementSpecialChapter GetTAchievementSpecialChapterByID(int nIdex)
 {
 if( GetTAchievementSpecialChapter().ContainsKey(nIdex))
 {
 return g_TAchievementSpecialChapter[nIdex] as Tab_TAchievementSpecialChapter;
 }
 return null;
 }

public static Tab_TItem GetTItemByID(int nIdex)
 {
 if( GetTItem().ContainsKey(nIdex))
 {
 return g_TItem[nIdex] as Tab_TItem;
 }
 return null;
 }

public static Tab_TRelation GetTRelationByID(int nIdex)
 {
 if( GetTRelation().ContainsKey(nIdex))
 {
 return g_TRelation[nIdex] as Tab_TRelation;
 }
 return null;
 }

public static Tab_TSpecialChapter GetTSpecialChapterByID(int nIdex)
 {
 if( GetTSpecialChapter().ContainsKey(nIdex))
 {
 return g_TSpecialChapter[nIdex] as Tab_TSpecialChapter;
 }
 return null;
 }

public static Tab_TUser GetTUserByID(int nIdex)
 {
 if( GetTUser().ContainsKey(nIdex))
 {
 return g_TUser[nIdex] as Tab_TUser;
 }
 return null;
 }

public static Tab_TUserCopy GetTUserCopyByID(int nIdex)
 {
 if( GetTUserCopy().ContainsKey(nIdex))
 {
 return g_TUserCopy[nIdex] as Tab_TUserCopy;
 }
 return null;
 }

public static Tab_TUserEquip GetTUserEquipByID(int nIdex)
 {
 if( GetTUserEquip().ContainsKey(nIdex))
 {
 return g_TUserEquip[nIdex] as Tab_TUserEquip;
 }
 return null;
 }

public static Tab_TourContent GetTourContentByID(int nIdex)
 {
 if( GetTourContent().ContainsKey(nIdex))
 {
 return g_TourContent[nIdex] as Tab_TourContent;
 }
 return null;
 }

public static Tab_Zhuanpan GetZhuanpanByID(int nIdex)
 {
 if( GetZhuanpan().ContainsKey(nIdex))
 {
 return g_Zhuanpan[nIdex] as Tab_Zhuanpan;
 }
 return null;
 }


}
}

