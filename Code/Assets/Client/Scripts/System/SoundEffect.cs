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



    public static string move =  "yidong" ;//�ƶ�
    public static string touch = "yuansuanxia";//����
    public static string buySuccess = "BuyItem";//����ɹ�
    public static string openPage = "jiemianjinru";//��ҳ��
    public static string stepToEffect = "dialog";//����ת��Ч
    public static string missionEffect = "shoujiyige";//�ռ�һ������Ԫ��
    public static string missionCompletedEffect = "shoujiman";//�ռ���һ������Ԫ��
    public static string hitTip = "xiaochutishi";//��ʾ
    public static string tiliHip = "SG_explode";//����



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
