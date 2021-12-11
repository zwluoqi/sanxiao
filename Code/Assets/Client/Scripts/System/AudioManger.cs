using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class AudioManger :MonoBehaviour {


    private static AudioManger instance;

    public static AudioManger Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<AudioManger>();
            }
            if (instance == null)
            {
                instance = new GameObject("AudioManger").AddComponent<AudioManger>();
            }
            return instance;
        }

    }



    public const string gameGroundPath = "Sounds/background";
    public const string uiBackGround = "Sounds/xuanguanjiemian";
	private AudioClip clip;
	private AudioSource source;

    private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();


    public void PreLoadAudioResource()
    {
        AudioClip clip = Resources.Load(gameGroundPath) as AudioClip;
        clips.Add(gameGroundPath, clip);
        clip = Resources.Load(uiBackGround) as AudioClip;
        clips.Add(uiBackGround, clip);

    }

    public void Init()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

	public void PlayAudio(string clipname){
        if (string.IsNullOrEmpty(clipname))
            return;

        if (!clips.ContainsKey(clipname))
        {
            return;
        }
        AudioClip clip = clips[clipname];

        if (source != null)
        {
            source.clip = clip;
            source.loop = true;
            source.Play();
        }
	}
	
	public void StopAudio(){
        if (source != null)
        {
            source.Pause();
        }
	}

	public void Open(){
		TurnVolume(0.5f);
	}

	public void Close(){
		TurnVolume(0);
	}

    public void Pause()
    {
        if (source.isPlaying)
        {
            source.Pause();
        }
    }

    public void Resume()
    {
        if (!source.isPlaying)
        {
            source.Play();
        }
    }

	public void TurnVolume(float volume){
		if(source == null)
			return;
		if(source != null){
			source.volume = Mathf.Clamp01(volume);
		}
		if(source.isPlaying && source.volume == 0){
			source.Pause();
		}else if(!source.isPlaying && source.volume != 0){
			source.Play();
		}
	}

}
