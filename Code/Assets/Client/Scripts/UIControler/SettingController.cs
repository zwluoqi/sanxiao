using UnityEngine;
using System.Collections;

public class SettingController : Page {

	public GameObject music;
	public GameObject musicP;
	public GameObject power;

    public GameObject LinkObj;
		public GameObject gmObj;

    protected override void DoOpen()
    {	
				if(DictTool.GetText("gm_fun") == "open" && gmObj.activeSelf){
						gmObj.SetActive(true);
				}else{
						gmObj.SetActive(false);
				}
        Transform close = music.transform.Find("close");
		close.gameObject.SetActive(!LocalDataBase.Instance().GetSysState(SystemLock.Music));

		close = musicP.transform.Find("close");
		close.gameObject.SetActive(!LocalDataBase.Instance().GetSysState(SystemLock.MusicP));

		close = power.transform.Find("close");
		close.gameObject.SetActive(!LocalDataBase.Instance().GetSysState(SystemLock.Power));

		OnFreshUI();

        //AudioManger.Instance.Pause();
	}

	public void OnClose(){
        this.Close();
		
	}
	
	public void OnHelp(){
        this.Close();
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

	public UIInput inputLabel; 
	public void GMParse(){
		string gm_str = inputLabel.value;

		if (string.IsNullOrEmpty (gm_str)) {
			return;
				}
		char[] splits = new char[]{' '};
		string[] gm_pares = gm_str.Split (splits, System.StringSplitOptions.RemoveEmptyEntries);

		if (gm_pares.Length != 3) {

			return;
				}

		string add_del = gm_pares [0];
		string data_type = gm_pares [1];
		int num = 0;
		int.TryParse (gm_pares [2], out num);
		if (add_del == "del") {
			num = -num;
				}
		switch (data_type) {
		case "zhuanshi":
			LocalDataBase.Instance().AddDataNum( DataType.zhuanshi,num);
			break;
		case "jinbi":
			LocalDataBase.Instance().AddDataNum( DataType.jinbi,num);
			break;
		case "power":

			LocalDataBase.Instance().AddDataNum( DataType.power,num);
			break;
		case "Hammer":

			LocalDataBase.Instance().AddEquipNum( EquipEnumID.Hammer,num);

			break;
		case "AddStep":
			LocalDataBase.Instance().AddEquipNum( EquipEnumID.AddStep,num);
				break;
		case "AddTime":
			LocalDataBase.Instance().AddEquipNum( EquipEnumID.AddTime,num);
			break;
		case "ResetItem":
			LocalDataBase.Instance().AddEquipNum( EquipEnumID.ResetItem,num);
			break;
		case "Exchange":
			LocalDataBase.Instance().AddEquipNum( EquipEnumID.Exchange,num);
			break;
		case "RowColEliminate":
			LocalDataBase.Instance().AddEquipNum( EquipEnumID.RowColEliminate,num);
			break;
		case "BomEffect":
			LocalDataBase.Instance().AddEquipNum( EquipEnumID.BomEffect,num);
			break;
		case "desStep":
			LocalDataBase.Instance().AddEquipNum( EquipEnumID.desStep,num);
			break;
		case "Bomb_SameCor":
			LocalDataBase.Instance().AddEquipNum( EquipEnumID.Bomb_SameCor,num);
			break;
		case "copylev":
		{
			LocalDataBase.Instance().SetSelectCopyLevel(num);
		}
			break;
		case "copystar":
		{
			for(int i=1;i<=150;i++){
				if(num<=0)
					break;
				LocalDataBase.SetCopyStar(i,num>=3?3:num);
				num -=3;
			}
		}
			break;
		default:
			BoxManager.Instance.ShowPopupMessage("Error");
			break;
				}
		BoxManager.Instance.ShowPopupMessage ("success!!!!!!!!");
	}


    public void OnExit()
    {
        //SceneManager.Instance.ExitImilite();
        Application.Quit();
    }
}
