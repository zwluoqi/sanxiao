using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GCGame.Table;


public sealed class EffectManager 
{
    private static EffectManager instance;
    private bool loadResouceComplete = false;
    public static EffectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EffectManager();
            }
            return instance;
        }
    }



    private EliminatePlayer m_player = null;
    //private IList<EffectBase> m_EffectList = new List<EffectBase>();
    public Dictionary<int, EffectBase> m_EffectCacheList = new Dictionary<int, EffectBase>();
	
	public Dictionary<string,GameObject> m_EffectGameObjectList = new Dictionary<string,GameObject>();

    private GameObject objParticles;
    public EffectManager()
    {
    }

    public bool Init(EliminatePlayer palyer)
    {
        m_player = palyer;
        //if (m_EffectCacheList.Count <= 0) { 
        //CreateEffectCache();
        //    }
        return true;
    }


    public void CreateEffectCache()
    {
        objParticles = new GameObject("Particles");
        objParticles.SetActive(false);
        Hashtable tabHash = TableManager.GetParticleAnimation();

        foreach (DictionaryEntry keyvalue in tabHash)
        {
            Tab_ParticleAnimation tab_par = (Tab_ParticleAnimation)keyvalue.Value;
            EffectBase particle = new EffectParticle(null, tab_par.EffectParticle, "");
            particle.durationTime = tab_par.EndSpark - tab_par.StartSpark;
            particle.delaydestroyTime = tab_par.DestroySpark - tab_par.StartSpark;
            particle.delayStartTime = tab_par.StartSpark;
            m_EffectCacheList[(int)keyvalue.Key]= particle;
           // particle.LoadResource();
        }

    }

    public IEnumerator LoadResources()
    {
        loadResouceComplete = false;

        foreach (EffectBase effect in m_EffectCacheList.Values)
        {
            effect.LoadResource();
            yield return null;
        }
        loadResouceComplete = true;
        yield return null;
    }

    public bool LoadResouceCompleted()
    {
        return loadResouceComplete;
    }

    public GameObject CreateGameObjectLib(string m_EffectName)
    {
        GameObject m_EffectLib = GameObject.Instantiate( Resources.Load("EffectParticle/" + m_EffectName)) as GameObject;
        m_EffectLib.transform.parent = objParticles.transform;
        m_EffectLib.transform.localPosition = Vector3.zero;
        m_EffectLib.transform.localScale = Vector3.one;
        if (m_EffectLib != null)
        {
            m_EffectGameObjectList.Add(m_EffectName, m_EffectLib);
        }
        return m_EffectLib;
    }

    public void DestroyEffect()
    {
        foreach (GameObject effectGameObject in m_EffectGameObjectList.Values)
        {
            GameObject.Destroy(effectGameObject);
        }
        m_EffectGameObjectList.Clear();

        m_EffectCacheList.Clear();
    }

}

