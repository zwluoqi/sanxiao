using UnityEngine;
using System.Collections;

using GCGame.Table;

public class SceneManager :MonoBehaviour{

	public GameObject UI;
	public GameObject Game;
	public GameObject Login;
    public GameObject NetWorkBox;
    public AdultSize adultSize;
    public Camera mainCamera;
    public EleUIController eleController;

	public static SceneManager Instance;

#if UNITY_EDITOR
    void OnGUI()
    {
        //重置;
        if (GUI.Button(new Rect(Screen.width - 200, 20, 160, 40), "ResetData"))
        {
            PlayerPrefs.DeleteAll();
        }
    }
#endif

    void Awake(){

		Instance = this;
        Random.seed = (int)System.DateTime.Now.Ticks;
//		LanguageManger.GetMe ().ExchangeLanguage (LanguageType.LANGUAGE_ENGLISH);
        StartCoroutine(_OnAwake());
	}

    IEnumerator _OnAwake()
    {
        //加载txt文件;
        yield return SceneManager.Instance.StartCoroutine(TableManager.InitTable());
				DictTool.Initialize();

        Debug.Log("_OnAwake");
        AudioManger.Instance.Init();
		if (SystemConfig.Instance.platformId == PlatformId.Appstore)
        {
            #if UNITY_IOS
            UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
            #endif
        }

        //请到 http://www.umeng.com/analytics 获取app key
        //GA.StartWithAppKeyAndChannelId("553d9d9867e58ee4790047f2", SystemConfig.Instance.platformID.ToString());

        //调试时开启日志 发布时设置为false
        //GA.SetLogEnabled(false);
    }

	void Update(){

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BoxManager.Instance.ShowMessage(LanguageManger.GetMe().GetWords("L_1018"));
            UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += ExitBtn;
        }
		NetManager.Instance.NetTick (Time.deltaTime, 1);
        //if (LocalDataBase.LoadResourceCompleted)
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        if (EliminateLogic.Instance.GetEliminatePlayer() != null)
        //        {
        //            PageManager.Instance.OpenPage("PauseController","");
        //        }
        //        else
        //        {
        //            BoxManager.Instance.ShowMessage(LanguageManger.GetMe().GetWords(LangID.L_1018));
        //            UIEventListener.Get(BoxManager.Instance.buttonOk).onClick += ExitBtn;
        //        }
        //    }
        //}
	}

    public void ExitImilite()
    {
				//System.Diagnostics.Process.GetCurrentProcess ().Kill ();
				Application.Quit();
    }

	public void ExitBtn(GameObject go){
		StartCoroutine(QuitApp());
	}

	IEnumerator QuitApp(){
        //cn.sharesdk.unity3d.ShareSDK.close();
		yield return new WaitForSeconds(0.4f);
		Application.Quit();
	}

    public void LoginToUI()
    {
        //PageManager.Instance.CloseCurrentWin();
        adultSize.AdustUI();
        Login.SetActive(false);
        Login.GetComponent<LoginController>().OnUnLoadAssets();
        Destroy(Login);
        UI.SetActive(true);
        AudioManger.Instance.PlayAudio(AudioManger.uiBackGround);
    }

	public void UIToGame(){
        //PageManager.Instance.CloseCurrentWin();
        
		UI.SetActive(false);
        UI.GetComponent<UIController>().map.UnLoadAssets();
        EleUIController.Instance.RestoreTexturesAssets();
		Game.SetActive(true);
		EliminateLogic.Instance.StartEleminate();
        AudioManger.Instance.PlayAudio(AudioManger.gameGroundPath);
        //adultSize.AdustGameAndLogin();
	}

	public void GameToUI(){
        //PageManager.Instance.CloseCurrentWin();
		EliminateLogic.Instance.EndEleminate();
        
		Game.SetActive(false);
        EleUIController.Instance.UnLoadAssets();
        UI.GetComponent<UIController>().map.RestoreTexturesAssets();
		UI.SetActive(true);
		UI.GetComponent<UIController>().map.UpdateCopyStateAfterGame();
        AudioManger.Instance.PlayAudio(AudioManger.uiBackGround);
        //adultSize.AdustUI();
	}

	public void LoginToGame(){
        
		Login.SetActive(false);
		Game.SetActive(true);
		EliminateLogic.Instance.StartEleminate();
        AudioManger.Instance.PlayAudio(AudioManger.gameGroundPath);
        //adultSize.AdustGameAndLogin();
	}

    void OnApplicationPause(bool pause)
    {
#if UNITY_IOS
        SDKObjecty.AddNotification(pause);
#endif
    }
}
