using UnityEngine;
using System.Collections;

public class DefeatedController : Page {
	
	public UILabel level;
	public UILabel targetScore;
    public GameObject returnBtn;
	
	protected override void DoOpen(){
		level.text = LocalDataBase.Instance().GetSelectCopyLevel().ToString();

		//targetScore.text = LevelData.requestScore.ToString();
		targetScore.text = MissionManager.Instance.completedScore.ToString();

        #region guild
        if (PlayerPrefs.GetInt("CopyEnd", -1) == -1)
        {
            PlayerPrefs.SetInt("CopyEnd", 1);
            GuideManager.Instance.ShowForceGuide(returnBtn, true, "", Vector3.zero);
        }
        #endregion

        //Umeng.GA.FailLevel("level" + LevelData.currentLevel);
	}
	
	public void OnReStartBtn(){
        this.Close();
		EliminateLogic.Instance.GetEliminatePlayer().ReturnToMain();
		PageManager.Instance.OpenPage("CopyBeforeController","");
	}
	
	public void OnReturnBtn()
    {
        #region guild
        GuideManager.Instance.HideGuide();
        #endregion
        this.Close();
        EliminateLogic.Instance.GetEliminatePlayer().ReturnToMain();
	}
}
