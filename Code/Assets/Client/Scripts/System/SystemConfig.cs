using UnityEngine;
using System.Collections;

using System;

public class SystemConfig :MonoBehaviour{


    private static SystemConfig instance;

    public static SystemConfig Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<SystemConfig>();
            }
            if (instance == null)
            {
                instance = new GameObject("SystemConfig").AddComponent<SystemConfig>();
            }
            return instance;
        }

    }

	public static string appurl = "https://itunes.apple.com/cn/app/dai-meng-xiao-xiao-kan/id959344886?mt=8&from=timeline&isappinstalled=0";

	public static void LogWarning (object obj)
	{
		#if UNITY_EDITOR
		if(DebugLog){
			Debug.LogWarning(obj);
		}else{
			;
		}
		#endif
	}

	public static void LogError (object obj)
	{
		#if UNITY_EDITOR
		if(DebugLog){
			Debug.LogError(obj);
		}else{
			;
		}
		#endif
	}

    static public void Log(object obj)
    {
#if UNITY_EDITOR
        if (DebugLog)
        {
            Debug.Log(obj);
        }
        else
        {
            ;
        }
#endif
    }

    public static float standard_width = 720;
    public static float standard_height = 1280;
    public static int manualHeight = 1280;

	public static bool DebugLog = false;

	public float standard_aspect{
		get{
			return standard_width/standard_height;
		}
	}
	
	
	public static Color[] backColors = new Color[]{
		new Color(1,1,1),//l baise		
		new Color(173/255f,216/255f,230/255f),//l blue
		new Color(240/255f,128/255f,128/255f),//l coral
		new Color(224/255f,255f/255f,255f/255f),//l cyan
		new Color(144/255f,238/255f,144/255f),//l green
		new Color(211/255f,211/255f,211/255f),//l grey
		new Color(255f/255f,182/255f,193/255f),//l pink
		new Color(255f/255f,160/255f,122/255f),//l salmon
		new Color(32/255f,178/255f,170/255f),//l seagreen
		new Color(135/255f,206/255f,250/255f),//l skyblue
		new Color(255f/255f,69/255f,0/255f),//l Orangered
		
	};



	public static string goldSpriteName = "jinbi";
	public static string zhuanshiSpriteName = "baoshi";
	public static string powerSpriteName = "tili";


	public PlatformId platformId = PlatformId.Appstore;//平台id

    public static int stepShangping = 100;
    public static int addSecondShangping = 101;
}
