//This code create by CodeEngine mrd.cyou.com ,don't modify
using System;
 using System.Collections.Generic;
 using System.Collections;

namespace GCGame.Table{

[Serializable]
 public class Tab_ParticleAnimation : ITableOperate
{ private const string TAB_FILE_DATA = "particleAnimation.txt";
 public enum _ID
 {
 INVLAID_INDEX=-1,
ID_EFFECTPARTICLE,
ID_STARTSPARK,
ID_ENDSPARK,
ID_DESTROYSPARK,
MAX_RECORD
 }
 public string GetInstanceFile(){return TAB_FILE_DATA; }

private float m_DestroySpark;
 public float DestroySpark { get{ return m_DestroySpark;}}

private string m_EffectParticle;
 public string EffectParticle { get{ return m_EffectParticle;}}

private float m_EndSpark;
 public float EndSpark { get{ return m_EndSpark;}}

private float m_StartSpark;
 public float StartSpark { get{ return m_StartSpark;}}

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
 Tab_ParticleAnimation _values = new Tab_ParticleAnimation();
 _values.m_DestroySpark =  Convert.ToSingle(valuesList[(int)_ID.ID_DESTROYSPARK] as string);
_values.m_EffectParticle =  valuesList[(int)_ID.ID_EFFECTPARTICLE] as string;
_values.m_EndSpark =  Convert.ToSingle(valuesList[(int)_ID.ID_ENDSPARK] as string);
_values.m_StartSpark =  Convert.ToSingle(valuesList[(int)_ID.ID_STARTSPARK] as string);

 _hash[nKey] = _values; }


}
}

