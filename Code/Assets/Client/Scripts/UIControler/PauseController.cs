using UnityEngine;
using System.Collections;

public class PauseController : Page
{

	public GameObject music;
	public GameObject musicP;
	public GameObject power;
    public GameObject LinkObj;

    protected override void DoOpen()
    {	
		Time.timeScale = 0;
		Transform close = music.transform.Find("close");
		close.gameObject.SetActive(!LocalDataBase.Instance().GetSysState(SystemLock.Music));

		close = musicP.transform.Find("close");
		close.gameObject.SetActive(!LocalDataBase.Instance().GetSysState(SystemLock.MusicP));

		close = power.transform.Find("close");
		close.gameObject.SetActive(!LocalDataBase.Instance().GetSysState(SystemLock.Power));

		OnFreshUI();

	}

	protected override void DoClose()
{
		Time.timeScale = 1;
	}

	public void OnClose(){
        this.Close();
	}
	
	public void OnHelp(){
        this.Close();
	}



	public void OnRestart(){
        int power = LocalDataBase.Instance().GetDataNum(DataType.power);
        if (power > LocalDataBase.costPower)
        {
            this.Close();
            LocalDataBase.Instance().DecreaseDataNum(DataType.power, LocalDataBase.costPower);
            EliminateLogic.Instance.GetEliminatePlayer().ReStartLevel();
        }
        else
        {
            BoxManager.Instance.ShowPopupMessage(LanguageManger.GetMe().GetWords("L_1052"));
        }
	}
	
	public void OnReturn(){
        this.Close();
		EliminateLogic.Instance.GetEliminatePlayer().ReturnToMain();
	}
	
	public void OnMusic(){
		Transform close = music.transform.Find("close");
		close.gameObject.SetActive(!close.gameObject.activeSelf);
		LocalDataBase.Instance().SetSysState(SystemLock.Music,!close.gameObject.activeSelf);
		OnFreshUI();
	}
	
	public void OnMusicPlus(){
		Transform close = musicP.transform.Find("close");
		close.gameObject.SetActive(!close.gameObject.activeSelf);
		LocalDataBase.Instance().SetSysState(SystemLock.MusicP,!close.gameObject.activeSelf);
		OnFreshUI();
	}	
	
	public void OnPower(){
		Transform close = power.transform.Find("close");
		close.gameObject.SetActive(!close.gameObject.activeSelf);
		LocalDataBase.Instance().SetSysState(SystemLock.Power,!close.gameObject.activeSelf);
		OnFreshUI();
	}	

	private void OnFreshUI(){
		Transform close = music.transform.Find("close");
		Transform Label = music.transform.Find("label");
		Transform sprite = music.transform.Find("sprite");
		if(!close.gameObject.activeSelf){
			AudioManger.Instance.Open();
			Label.GetComponent<UILabel>().text = LanguageManger.GetMe().GetWords("SET_001");
		}else{
			AudioManger.Instance.Close();
			Label.GetComponent<UILabel>().text = LanguageManger.GetMe().GetWords("SET_002");
		}

		close = musicP.transform.Find("close");
		Label = musicP.transform.Find("label");
		sprite = musicP.transform.Find("sprite");
		if(!close.gameObject.activeSelf){
			SoundEffect.Instance.TurnVolume(1);
			Label.GetComponent<UILabel>().text = LanguageManger.GetMe().GetWords("SET_003");
		}else{
			SoundEffect.Instance.TurnVolume(0);
			Label.GetComponent<UILabel>().text = LanguageManger.GetMe().GetWords("SET_004");
		}

		close = power.transform.Find("close");
		Label = power.transform.Find("label");
		sprite = power.transform.Find("sprite");
		if(!close.gameObject.activeSelf){
			Label.GetComponent<UILabel>().text = LanguageManger.GetMe().GetWords("SET_005");
		}else{
			Label.GetComponent<UILabel>().text = LanguageManger.GetMe().GetWords("SET_006");
		}

	}

    public void LinkUs()
    {
        LinkObj.SetActive(true);
    }

    public void OnCloseLink()
    {
        LinkObj.SetActive(false);
    }
}
