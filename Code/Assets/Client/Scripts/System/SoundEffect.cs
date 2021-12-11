using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GCGame.Table;

public class SoundEffect :MonoBehaviour
{
    private static SoundEffect instance;

    public static SoundEffect Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SoundEffect>();
            }
            if (instance == null)
            {
                instance = new GameObject("SoundEffect").AddComponent<SoundEffect>();
            }
            return instance;
        }

    }



    public static string move =  "yidong" ;//移动
    public static string touch = "yuansuanxia";//按下
    public static string buySuccess = "BuyItem";//购买成功
    public static string openPage = "jiemianjinru";//打开页面
    public static string stepToEffect = "dialog";//步数转特效
    public static string missionEffect = "shoujiyige";//收集一个任务元素
    public static string missionCompletedEffect = "shoujiman";//收集满一类任务元素
    public static string hitTip = "xiaochutishi";//提示
    public static string tiliHip = "SG_explode";//体力



    public float valum = 1;
    //private bool paused = false;
	public Dictionary<string,AudioClip> clips = new Dictionary<string, AudioClip>();
	public Dictionary<string,int> clipPlayTimes = new Dictionary<string, int>();

    public void PreLoadSoundResource()
    {
        string[] allsound = new string[] { move, touch, buySuccess, openPage, stepToEffect, missionEffect, missionCompletedEffect, hitTip, tiliHip };

        foreach (string sound in allsound)
        {
            string filePath = "Sounds/" + sound;
            if (!clips.ContainsKey(filePath))
            {
                AudioClip clip = Resources.Load(filePath) as AudioClip;
                clips.Add(filePath, clip);
                clipPlayTimes.Add(filePath, 0);
            }
        }

        Hashtable tab_eles = TableManager.GetElement();
        foreach (DictionaryEntry de in tab_eles)
        {
            Tab_Element tab_ele = (Tab_Element)de.Value;
            string filePath = "Sounds/" + tab_ele.EliminateSound;
            if (!clips.ContainsKey(filePath) && tab_ele.EliminateSound != "None")
            {
                AudioClip clip = Resources.Load(filePath) as AudioClip;
                clips.Add(filePath, clip);
                clipPlayTimes.Add(filePath, 0);
            }

            filePath = "Sounds/" + tab_ele.ProduceSound;
            if (!clips.ContainsKey(filePath) && tab_ele.ProduceSound != "None")
            {
                AudioClip clip = Resources.Load(filePath) as AudioClip;
                clips.Add(filePath, clip);
                clipPlayTimes.Add(filePath, 0);
            }
        }

        Hashtable tab_squs = TableManager.GetSquare();
        foreach (DictionaryEntry de in tab_squs)
        {
            Tab_Square tab_ele = (Tab_Square)de.Value;
            string filePath = "Sounds/" + tab_ele.EliminateSound;
            if (!clips.ContainsKey(filePath) && tab_ele.EliminateSound != "None")
            {
                AudioClip clip = Resources.Load(filePath) as AudioClip;
                clips.Add(filePath, clip);
                clipPlayTimes.Add(filePath, 0);
            }

            filePath = "Sounds/" + tab_ele.ProduceSound;
            if (!clips.ContainsKey(filePath) && tab_ele.ProduceSound != "None")
            {
                AudioClip clip = Resources.Load(filePath) as AudioClip;
                clips.Add(filePath, clip);
                clipPlayTimes.Add(filePath, 0);
            }
        }

    }

    public void ClearSounds()
    {
        foreach (AudioClip clip in clips.Values)
        {
            Resources.UnloadAsset(clip);
        }
        clips.Clear();
        clipPlayTimes.Clear();
    }



	public void PlaySound(string[] soundNames,int index)
	{
        PlaySound(soundNames[index]);
	}

	private void PlayCompletedCallback(GameObject go){
		clipPlayTimes[go.name]--;
		WidgetBufferManager.Instance.DestroyWidgetObj("SoundObject",go);
	}

	public void PlaySoundRandom(string[] soundNames)
	{

		int rnd = Random.Range(0,soundNames.Length);
		PlaySound(soundNames,rnd);
	}

    public void PlaySound(string clipname)
    {
        if ( valum == 0)
            return;
        if (string.IsNullOrEmpty(clipname))
            return;
        if (clipname == "None")
            return;
        string filePath = "Sounds/" + clipname;
        if (!clips.ContainsKey(filePath))
        {
            AudioClip clip = Resources.Load(filePath) as AudioClip;
            if (clip == null)
            {
                SystemConfig.LogError(filePath+"null");
                return;
            }
            clips.Add(filePath, clip );
            clipPlayTimes.Add(filePath, 1);
        }
        else
        {
            if (clipPlayTimes[filePath] >= 3)
            {
                return;
            }
            clipPlayTimes[filePath]++;
        }

        GameObject soundObject = WidgetBufferManager.Instance.loadWidget("SoundObject");
        soundObject.name = filePath;
        soundObject.GetComponent<AudioSource>().volume = valum;
        soundObject.GetComponent<AudioSource>().clip = clips[filePath];
        soundObject.GetComponent<AudioSource>().Play();
        soundObject.GetComponent<TimeEventObject>().SetOnTimeFinished(PlayCompletedCallback, clips[filePath].length);
    }

    //public void Stop()
    //{
    //    paused = true;
    //}
	
	public void TurnVolume( float volume)
	{
		valum = Mathf.Clamp01(volume);
	}
}
